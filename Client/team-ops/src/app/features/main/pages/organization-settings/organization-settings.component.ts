import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import {
  OrganizationDetails,
  OrganizationService,
} from '../../services/organization.service';
import { ToastrService } from 'ngx-toastr';
import { FormControl, FormGroup } from '@angular/forms';
import { User } from '../../../../core/user/interfaces';
import { UserStore } from '../../../../store/user.store';

@Component({
  selector: 'app-organization-settings',
  templateUrl: './organization-settings.component.html',
  styleUrl: './organization-settings.component.scss',
})
export class OrganizationSettingsComponent implements OnInit {
  public userState: User;
  public isReadOnly = true

  public overviewSection = 'overview'
  public projectsSection = 'projects'

  public settingsSections = [this.overviewSection,this.projectsSection]

  public selectedSection = this.settingsSections[0]

  onSectionSelect(settingsSection:string){
    this.selectedSection = settingsSection
  }

  public organizationId: string;
  public organizationDetails: OrganizationDetails | null = null;

  constructor(private userStore: UserStore,
    private route: ActivatedRoute,
    private organizationService: OrganizationService,
    private toastr: ToastrService
  ) {
    this.userState = this.userStore.getState().user!;
    this.organizationId = this.route.snapshot.paramMap.get('organizationId')!;
  }

  //============Overview section logic============
  overviewForm = new FormGroup({
    name: new FormControl(''),
    description: new FormControl(''),
  });

  get overviewFormDisabled() {
    return (
      this.overviewForm.value.name === this.organizationDetails?.name &&
      this.overviewForm.value.description ===
        this.organizationDetails?.description
    );
  }

  onOverviewSubmit() {
    console.log('submit');
    this.organizationService
      .updateOrganization({
        id: this.organizationId,
        name: this.overviewForm.value.name ?? this.organizationDetails?.name!,
        description:
          this.overviewForm.value.description ??
          this.organizationDetails?.description!,
      })
      .subscribe((data) => {
        if (data.isSuccess) {
          this.organizationDetails!.description = data.data?.description!;
          this.organizationDetails!.name = data.data?.name!;
          this.overviewForm.patchValue({
            name: data.data?.name,
            description: data.data?.description,
          });
        }
        this.toastr.show(
          data.message,
          'Organization Update',
          {
            closeButton: true,
            timeOut: 3500,
          },
          data.isSuccess ? 'toast-success' : 'toast-error'
        );
      });
  }
  //============Overview section logic============

  //============Projects section logic============
  displayedColumns:string[] = ['Name','Description','Last Updated', 'Participants']
  //============Projects section logic============

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.organizationId = data['ids'].id;
      if(this.organizationId){
        this.loadOrganzationDetails()
      }
    });
  }

  loadOrganzationDetails(){
    this.organizationService
      .getOrganizationDetails(this.organizationId)
      .subscribe((data) => {
        if (data.isSuccess) {
          console.log(data)
          this.organizationDetails = data.data;
          this.overviewForm.patchValue({
            name: data.data?.name,
            description: data.data?.description,
          });
          this.isReadOnly = this.organizationDetails?.ownerId !== this.userState.id 
        } else {
          this.toastr.show(
            data.message,
            'Organization Details',
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

