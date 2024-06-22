import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import {
  WorkItemCardDTO,
  WorkItems_ProjectUser,
} from '../../../services/boards.service';
import { workItemsStatus, workItemsType } from '../../../common/enums';
import {
  MatDialog,
  MatDialogConfig,
  MatDialogRef,
} from '@angular/material/dialog';
import { WorkItemDetailsDialogComponent } from '../work-item-details-dialog/work-item-details-dialog.component';

@Component({
  selector: 'app-work-item-card',
  templateUrl: './work-item-card.component.html',
  styleUrl: './work-item-card.component.scss',
})
export class WorkItemCardComponent implements OnInit {
  @Input({ required: true }) workItem!: WorkItemCardDTO;
  @Input({ required: true }) projectUsers!: WorkItems_ProjectUser[];
  @Output() onDeleteClicked: EventEmitter<{
    id: string;
    workItemType: workItemsType;
  }> = new EventEmitter();
  @Output() workItemUpdated: EventEmitter<void> = new EventEmitter();
  @Output() workItemAssignedToUpdate: EventEmitter<{
    workItemId: string;
    assignedToId: string;
  }> = new EventEmitter();

  workItemsType = workItemsType

  onDelete() {
    this.onDeleteClicked.emit({
      id: this.workItem.id,
      workItemType: this.workItem.type,
    });
  }

  public assignedToInitials = '';
  public statusColor = '';
  public statusString = '';

  constructor(public dialog: MatDialog) {}

  openDetails() {
    const dialogRef = this.dialog.open(WorkItemDetailsDialogComponent, {
      height: '500px',
      width: '700px',

      data: { workItem: this.workItem },
    });
    dialogRef.afterClosed().subscribe((result) => {
      console.log(result);
      if (result.updated) {
        this.workItemUpdated.emit();
      }
    });
  }

  ngOnInit(): void {
    console.log(this.projectUsers);

    let name = this.workItem.assignedTo.name.split(' ');
    let unasigned = name[1] ? false : true;
    this.assignedToInitials = unasigned
      ? name[0][0].toUpperCase() + name[0][1].toUpperCase()
      : name[0][0].toUpperCase() + name[1][0].toUpperCase();

    this.statusString =
      workItemsStatus[this.workItem.status as unknown as number].toString();
    this.statusColor = this.getStatusColor(this.workItem.status);
  }

  onAssignedToUpdate(user: WorkItems_ProjectUser) {
    this.workItemAssignedToUpdate.emit({
      workItemId: this.workItem.id,
      assignedToId: user.id,
    });
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
}
