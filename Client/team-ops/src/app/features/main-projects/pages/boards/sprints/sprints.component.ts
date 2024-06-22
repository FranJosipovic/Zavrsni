import { Component, OnInit } from '@angular/core';
import {
  BoardsService,
  CreateWorkItemDTO,
  Iteration,
  UpdateWorkItemDTO,
  UserStoryWithChildren,
  UserStory_Task_BugFix,
  WorkItems_ProjectUser,
} from '../services/boards.service';
import {
  HttpResponseModel,
  ResponseCollection,
} from '../../../../../core/models/http.response';
import {
  workItemsPriority,
  workItemsStatus,
  workItemsType,
} from '../common/enums';
import { FormControl } from '@angular/forms';
import { UserStore } from '../../../../../store/user.store';

@Component({
  selector: 'app-sprints',
  templateUrl: './sprints.component.html',
  styleUrl: './sprints.component.scss',
})
export class SprintsComponent implements OnInit {
  private projectId;
  public iterations: Iteration[] = [];
  public selectedIteration: string | null = null;

  public workItemsStatus = workItemsStatus;

  public userStories: UserStoryWithChildren[] = [];

  public assignedToInitials = 'UN';
  constructor(
    private boardsService: BoardsService,
    private userState: UserStore
  ) {
    this.projectId = localStorage.getItem('projectId')!;
    this.assignedToId.valueChanges.subscribe((value) => {
      if (!value) {
        this.assignedToInitials = 'UN';
      } else {
        let user = this.projectUsers.find((item) => item.id == value);
        this.assignedToInitials = user?.name[0]! + user?.surname[0]!;
      }
    });
  }

  public projectUsers: WorkItems_ProjectUser[] = [];

  ngOnInit(): void {
    this.loadITerations();
  }

  private loadUsers() {
    this.boardsService.getProjectUsers(this.projectId).subscribe((data) => {
      console.log(data);
      if (data.isSuccess) {
        this.projectUsers = data.data?.items!;
      }
    });
  }

  private loadITerations() {
    this.boardsService.getIterations(this.projectId).subscribe((data) => {
      if (data.isSuccess) {
        console.log(data);
        this.iterations = data.data?.items!;
        this.selectedIteration = this.iterations[0]?.id ?? null;
        if (this.selectedIteration) {
          this.loadWorkItems();
        }
      }
    });
  }

  private loadWorkItems() {
    this.boardsService
      .getWorkItemsByIteration(this.selectedIteration!)
      .subscribe(
        (
          data: HttpResponseModel<ResponseCollection<UserStoryWithChildren>>
        ) => {
          if (data.isSuccess) {
            this.userStories = data.data?.items!;
            console.log(this.userStories);
            this.loadUsers();
          }
        }
      );
  }

  createNewIteration() {
    console.log('hellp');
    let request = {
      projectId: this.projectId,
      startsAt: new Date(),
      endsAt: new Date(new Date().setDate(new Date().getDate() + 14)),
    };
    this.boardsService.createNewIteration(request).subscribe((data) => {
      this.loadITerations();
    });
  }

  onIterationChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const selectedValue = selectElement.value;

    this.selectedIteration = selectedValue;
    this.loadWorkItems();
  }

  public showCreateOptions = false;
  public selectedUserStoryCreateOptions: string | null = null;
  private toggleChildrenSelected = false;
  toggleCreateOptions(userStoryId: string) {
    if (!this.toggleChildrenSelected) {
      if (this.newBugFixSelected == true || this.newTaskSelected == true) {
        this.newBugFixSelected = false;
        this.newTaskSelected = false;
      }

      this.showCreateOptions = !this.showCreateOptions;

      if (this.showCreateOptions) {
        this.selectedUserStoryCreateOptions = userStoryId;
      }

      if (userStoryId != this.selectedUserStoryCreateOptions) {
        this.showCreateOptions = true;
        this.selectedUserStoryCreateOptions = userStoryId;
      }
    }
    this.toggleChildrenSelected = false;
  }

  public title = new FormControl('');
  public assignedToId = new FormControl<string | null>(null);

  public newTaskSelected = false;
  public newBugFixSelected = false;

  selectNewTask() {
    this.toggleChildrenSelected = true;
    this.newTaskSelected = true;
    this.showCreateOptions = false;
  }
  selectNewBugFix() {
    this.toggleChildrenSelected = true;
    this.showCreateOptions = false;
    this.newBugFixSelected = true;
  }

  onCreateNewWorkItem(parentId: string) {
    let req: CreateWorkItemDTO = {
      parentId,
      priority: workItemsPriority.LOW,
      title: this.title.value!,
      description: '',
      type: this.newTaskSelected ? workItemsType.Task : workItemsType.Bug_Fix,
      createdById: this.userState.getState().user?.id!,
      assignedToId: this.assignedToId.value,
      iterationId: this.selectedIteration!,
    };
    this.boardsService.createNewWorkItem(req).subscribe((data) => {
      console.log(data);
      this.loadWorkItems();
    });
    this.title.reset();
    this.assignedToId.reset();
    this.newBugFixSelected = false;
    this.newTaskSelected = false;
    this.showCreateOptions = false;
  }

  removeWorkItem(id: string) {
    this.boardsService.deleteWorkItem(id).subscribe((data) => {
      if (data.isSuccess) {
        this.userStories = this.userStories.map((userStory) => {
          return {
            ...userStory,
            children: userStory.children.filter((child) => child.id !== id),
          };
        });
      }
    });
  }

  handleDeleteEvent(event: { id: string; workItemType: workItemsType }) {
    if (event.workItemType == workItemsType.User_Story) return;
    this.removeWorkItem(event.id);
  }

  updateWorkItemStatus(
    workItem: UserStory_Task_BugFix,
    newStatus: workItemsStatus
  ) {
    workItem.status = newStatus;
    this.boardsService
      .updateWorkItem({
        ...workItem,
        assignedToId: workItem.assignedTo.id,
      } as UpdateWorkItemDTO)
      .subscribe((data) => {
        if (data.isSuccess) {
          this.userStories = this.userStories.map((userStory) => {
            return {
              ...userStory,
              children: userStory.children.map((child) => {
                if (child.id === data.data!.id) {
                  return { ...child, status: data.data!.status };
                }
                return child;
              }),
            };
          });
        }
      });
  }

  handleUpdateAssignedToEvent(event: {
    workItemId: string;
    assignedToId: string;
  }) {
    let workItem: UserStory_Task_BugFix | null = null;

    // Search for the work item in the user stories
    this.userStories.forEach((userStory) => {
      userStory.children.forEach((wI) => {
        if (wI.id === event.workItemId) {
          workItem = wI;
        }
      });
    });
    // Check if the work item was found before updating
    if (workItem !== null) {
      this.updateWorkItemAssignedTo(workItem, event.assignedToId);
    } else {
      console.error(`Work item with ID ${event.workItemId} not found.`);
    }
  }

  updateWorkItemAssignedTo(
    workItem: UserStory_Task_BugFix,
    newAssignedTo: string
  ) {
    this.boardsService
      .updateWorkItem({
        ...workItem,
        assignedToId: newAssignedTo,
      } as UpdateWorkItemDTO)
      .subscribe((data) => {
        if (data.isSuccess) {
          this.userStories = this.userStories.map((userStory) => {
            return {
              ...userStory,
              children: userStory.children.map((child) => {
                if (child.id === data.data!.id) {
                  return { ...child, assignedTo: data.data!.assignedTo };
                }
                return child;
              }),
            };
          });
        }
        // this.loadWorkItems()
      });
  }

  onWorkItemUpdate() {
    this.loadWorkItems();
  }

  draggedItem: UserStory_Task_BugFix | null = null;

  allowDrop(e: Event, userStoryId: string) {
    if (this.draggedItem?.parentId === userStoryId) {
      e.preventDefault();
      (e.currentTarget as HTMLElement).classList.add('drag-over');
    }
  }

  drag(e: DragEvent, workItem: UserStory_Task_BugFix) {
    this.draggedItem = workItem;
    e.dataTransfer?.setData('workItem', JSON.stringify(workItem));
  }

  drop(e: DragEvent, workItemsStatus: workItemsStatus, userStoryId: string) {
    e.preventDefault();
    (e.currentTarget as HTMLElement).classList.remove('drag-over');
    const workItem = JSON.parse(e.dataTransfer?.getData('workItem')!);
    if (workItem.parentId == userStoryId) {
      this.updateWorkItemStatus(workItem, workItemsStatus);
    }
    this.draggedItem = null;
  }

  removeBgClass(e: Event) {
    (e.currentTarget as HTMLElement).classList.remove('drag-over');
  }
}
