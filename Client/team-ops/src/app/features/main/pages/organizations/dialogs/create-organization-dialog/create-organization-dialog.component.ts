import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { OrganizationService } from '../../../../services/organization.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-organization-dialog',
  templateUrl: './create-organization-dialog.component.html',
  styleUrl: './create-organization-dialog.component.scss',
})
export class CreateOrganizationDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<CreateOrganizationDialogComponent>,
    private organizationService: OrganizationService,
    @Inject(MAT_DIALOG_DATA) public data: { ownerId: string },
    private toastr: ToastrService
  ) {}

  public newOrganizationForm = new FormGroup({
    name: new FormControl(''),
    description: new FormControl(''),
  });

  onSubmit() {
    let name = this.newOrganizationForm.get('name')?.value!;
    let description = this.newOrganizationForm.get('description')?.value!;

    this.organizationService
      .createOrganization({
        name,
        description,
        ownerId: this.data.ownerId,
      })
      .subscribe((data) => {
        this.toastr.show(
          data.message,
          'Organizations',
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
