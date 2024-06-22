import { Component, OnInit } from '@angular/core';
import { BoardsService, WorkItem } from '../services/boards.service';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { CreateWorkItemComponent } from '../dialogs/create-work-item/create-work-item.component';
import { Console } from 'console';

@Component({
  selector: 'app-work-items',
  templateUrl: './work-items.component.html',
  styleUrl: './work-items.component.scss',
})
export class WorkItemsComponent implements OnInit {
  private projectId;
  public workItems: WorkItem[] = [];

  public columnsToDisplay = [
    'title',
    'description',
    'createdBy',
    'assignedTo',
    'type',
    'priority',
    'status',
  ];

  constructor(
    private boardsService: BoardsService,
    private toastr: ToastrService,
    private dialog: MatDialog
  ) {
    this.projectId = localStorage.getItem('projectId')!;
  }

  ngOnInit(): void {
    this.loadWorkItems();
  }
  openCreateWorkItemModal() {
    const dialogRef = this.dialog.open(CreateWorkItemComponent, {
      height: '500px',
      width: '620px',
      data: {},
    });
    dialogRef.afterClosed().subscribe((result) => {
      this.loadWorkItems();
    });
  }

  loadWorkItems() {
    console.log("load")
    this.boardsService.getWorkItems(this.projectId).subscribe((data) => {
      console.log(data);
      if (data.isSuccess) {
        this.workItems = data.data?.items!;
      } else {
        this.toastr.show(
          data.message,
          'Result',
          {
            closeButton: true,
            timeOut: 3500,
          },
          'toast-error'
        );
      }
    });
  }
}
