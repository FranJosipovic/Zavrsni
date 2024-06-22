import { Component, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { ProjectService } from '../../../../../main-projects/pages/project/services/project.service';

@Component({
  selector: 'app-create-project-dialog',
  templateUrl: './create-project-dialog.component.html',
  styleUrl: './create-project-dialog.component.scss'
})
export class CreateProjectDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<CreateProjectDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { ownerId: string, organizationId:string },
    private toastr: ToastrService,
    private projectService: ProjectService
  ) {}

  public newProjectForm = new FormGroup({
    name: new FormControl(''),
    description: new FormControl(''),
  });

  onSubmit() {
    let name = this.newProjectForm.get('name')?.value!;
    let description = this.newProjectForm.get('description')?.value!;

    this.projectService
      .createProject({
        name,
        description,
        organizationOwnerId: this.data.ownerId,
        organizationId: this.data.organizationId
      })
      .subscribe((data) => {
        this.toastr.show(
          data.message,
          'Projects',
          {
            closeButton: true,
            timeOut: 3500,
          },
          data.isSuccess ? 'toast-success' : 'toast-error'
        );
        this.dialogRef.close({ refresh: data.isSuccess });
      });
  }
}
