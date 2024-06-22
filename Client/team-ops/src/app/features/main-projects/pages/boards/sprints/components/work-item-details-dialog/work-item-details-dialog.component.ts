import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {
  BoardsService,
  UpdateWorkItemDTO,
  WorkItemCardDTO,
} from '../../../services/boards.service';
import {
  workItemsPriority,
  workItemsStatus,
  workItemsType,
} from '../../../common/enums';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-work-item-details-dialog',
  templateUrl: './work-item-details-dialog.component.html',
  styleUrl: './work-item-details-dialog.component.scss',
})
export class WorkItemDetailsDialogComponent implements OnInit {
  public createdByInitials = '';
  public assignedToInitials = '';

  public workItemsType = workItemsType;
  public workItemsStatus = workItemsStatus;
  public workItemsPriority = workItemsPriority;

  public statusColor = '';
  public priorityColor = '';

  constructor(
    private matDialogRef: MatDialogRef<WorkItemDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { workItem: WorkItemCardDTO },
    private boardService: BoardsService
  ) {}

  ngOnInit(): void {
    this.getInitials();
    this.statusColor = this.getStatusColor(this.data.workItem.status);
    this.priorityColor = this.getPriorityColor(this.data.workItem.priority);
  }

  public description = new FormControl(this.data.workItem.description);
  public title = new FormControl(this.data.workItem.title);

  get inputChanged() {
    return (
      this.description.value !== this.data.workItem.description ||
      this.title.value !== this.data.workItem.title
    );
  }

  close() {
    this.matDialogRef.close({ updated: false });
  }

  getInitials() {
    let createdByParts = this.data.workItem.createdBy.name.split(' ');
    this.createdByInitials = createdByParts[0][0] + createdByParts[1][0];

    let assignedToParts = this.data.workItem.assignedTo.name.split(' ');
    if (assignedToParts.length == 1) {
      this.assignedToInitials = assignedToParts[0][0] + assignedToParts[0][1];
    } else {
      this.assignedToInitials = assignedToParts[0][0] + assignedToParts[1][0];
    }
  }

  private getStatusColor(status: workItemsStatus) {
    switch (status) {
      case workItemsStatus.Active:
        return 'blue';
      case workItemsStatus.Closed:
        return 'green';
      case workItemsStatus.New:
        return 'purple';
      default:
        return 'blue';
    }
  }

  private getPriorityColor(priority: workItemsPriority) {
    switch (priority) {
      case workItemsPriority.LOW:
        return 'green';
      case workItemsPriority.MEDIUM:
        return 'orange';
      case workItemsPriority.HIGH:
        return 'red';
    }
  }

  saveChanges() {
    this.boardService
      .updateWorkItem({
        ...this.data.workItem,
        description: this.description.value!,
        title: this.title.value!,
        assignedToId: this.data.workItem.assignedTo.id,
      } as UpdateWorkItemDTO)
      .subscribe((data) => {
        if (data.isSuccess) {
          this.matDialogRef.close({
            updated: true,
          });
        }
      });
  }
}
