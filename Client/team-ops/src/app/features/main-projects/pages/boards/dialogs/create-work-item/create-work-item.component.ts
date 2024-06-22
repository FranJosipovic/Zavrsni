import { Component, OnInit } from '@angular/core';
import { workItemsPriority, workItemsType } from '../../common/enums';
import {
  BoardsService,
  CreateWorkItemDTO,
  Iteration,
  WorkItem_UserStory,
  WorkItems_ProjectUser,
} from '../../services/boards.service';
import { FormControl } from '@angular/forms';
import { UserStore } from '../../../../../../store/user.store';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-work-item',
  templateUrl: './create-work-item.component.html',
  styleUrl: './create-work-item.component.scss',
})
export class CreateWorkItemComponent implements OnInit {
  private projectId;
  public workItemTypes = workItemsType;
  public workItemsPriorities = workItemsPriority;

  public iterations: Iteration[] = [];
  public projectUsers: WorkItems_ProjectUser[] = [];

  constructor(
    private boardService: BoardsService,
    private userStore: UserStore,
    public dialogRef: MatDialogRef<CreateWorkItemComponent>
  ) {
    this.projectId = localStorage.getItem('projectId')!;
  }
  ngOnInit(): void {
    this.boardService.getIterations(this.projectId).subscribe((data) => {
      if (data.isSuccess) {
        this.iterations = data.data?.items!;
        this.iterationId.setValue(this.iterations[0]?.id);
      }
    });

    this.boardService.getProjectUsers(this.projectId).subscribe((data) => {
      if (data.isSuccess) {
        this.projectUsers = data.data?.items!;
      }
    });

    //user story belongs to specific iteration
    this.parentId.valueChanges.subscribe((value) => {
      if (!this.userStorySelected) {
        if (value) {
          let parentWorkItem = this.parentWorkItems.find(item => item.id == value);
          this.iterationId.setValue(parentWorkItem?.iterationId!);
        }
      }
    });
  }

  public userStorySelected = true;

  public parentWorkItems: WorkItem_UserStory[] = [];

  onWorkItemTypeChange(event: Event) {
    const target = event.target as HTMLSelectElement;

    if (target && target.value) {
      const value = target.value;

      //if it isn't user story
      if (value != this.workItemTypes.User_Story.toString()) {
        this.userStorySelected = false;

        this.boardService
          .getUserStoriesByProject(this.projectId)
          .subscribe((data) => {
            if (data.isSuccess) {
              if (data.data?.count == 0) {
                this.userStorySelected = true;
                return;
              }
              this.parentWorkItems = data.data?.items!;
              this.parentId.setValue(this.parentWorkItems[0].id);
              this.iterationId.setValue(this.parentWorkItems[0].iterationId);
            } else {
              this.userStorySelected = true;
            }
          });
      } else {
        this.userStorySelected = true;
        this.parentId.setValue(null);
      }
    }
  }

  onIterationChange(event: Event) {
    const target = event.target as HTMLSelectElement;

    if (target && target.value) {
      let value = target.value;
      this.iterationId.setValue(value);
    }
  }

  public workItemType = new FormControl<workItemsType>(
    workItemsType.User_Story
  );
  public title = new FormControl<string>('');
  public description = new FormControl<string>('');
  public iterationId = new FormControl<string | null>(null);
  public assignTo = new FormControl<string | null>(null);
  public priority = new FormControl<workItemsPriority>(workItemsPriority.LOW);
  public parentId = new FormControl<string | null>(null);

  onSubmit(e: Event) {
    e.preventDefault();
    this.boardService
      .createNewWorkItem({
        title: this.title.value,
        description: this.description.value,
        priority: this.priority.value as number,
        type: this.workItemType.value as number,
        parentId: this.parentId.value,
        assignedToId: this.assignTo.value,
        iterationId: this.iterationId.value,
        createdById: this.userStore.getState().user?.id,
      } as CreateWorkItemDTO)
      .subscribe((data) => {
        console.log(data);
      });

    this.dialogRef.close();
  }
}
