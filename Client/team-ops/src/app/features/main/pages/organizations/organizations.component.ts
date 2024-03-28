import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { User } from '../../../../core/user/interfaces';
import { UserStore } from '../../../../store/user.store';
import {
  Organization,
  Project,
  OrganizationService,
} from '../../services/organization.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-organizations',
  templateUrl: './organizations.component.html',
  styleUrl: './organizations.component.scss',
})
export class OrganizationsComponent implements OnInit {
  public userState: User;

  public organizations: Organization[] = [];

  public myOrganizations: Organization[] = [];
  public participatingOrganizations: Organization[] = [];

  public projects: Project[] = [];

  public selectedOrganization: Organization | null = null;

  public loadingProjects = true;

  constructor(
    private userStore: UserStore,
    private router: Router,
    private organizationsService: OrganizationService,
    private toastr: ToastrService
  ) {
    this.userState = this.userStore.getState().user!;
  }

  ngOnInit(): void {
    this.organizationsService
      .getOrganizationsWithUser(this.userState.id)
      .subscribe((response) => {
        console.log(response);
        if (response.isSuccess) {
          this.organizations = response.data?.items ?? [];

          this.organizations.forEach((org) => {
            if (org.ownerId === this.userState.id) {
              this.myOrganizations.push(org);
            } else {
              this.participatingOrganizations.push(org);
            }
          });

          this.selectedOrganization = this.organizations[0];
          this.projects = this.selectedOrganization.projects;
          this.loadingProjects = false;
        } else {
          this.toastr.show(
            response.message,
            'Organizations',
            {
              closeButton: true,
              timeOut: 3500,
            },
            'toast-error'
          );
        }
      });
  }

  onOrganizationSelect(organizationId: string): void {
    if (this.selectedOrganization?.id == organizationId) return;
    this.organizations.forEach((organization) => {
      if (organization.id == organizationId) {
        this.selectedOrganization = organization;
        this.projects = organization.projects;
      }
    });
  }

  onSettingsClick() {
    this.router.navigate(['main', this.selectedOrganization?.name, 'settings']);
  }

  onProjectNavigate(projectName: string) {
    console.log(projectName)
    this.router.navigate([
      'main',
      this.selectedOrganization?.name,
      projectName,
    ]);
  }
}
