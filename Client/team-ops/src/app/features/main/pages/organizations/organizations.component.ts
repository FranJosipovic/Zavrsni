import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { User } from '../../../../core/user/interfaces';
import { UserStore } from '../../../../store/user.store';
import {
  Organization,
  Project,
  OrganizationService,
} from '../../services/organization.service';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize, of, switchMap } from 'rxjs';
import { GithubService } from '../../services/github.service';

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

  public loadingProjects = false;

  constructor(
    private userStore: UserStore,
    private router: Router,
    private organizationsService: OrganizationService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private githubService: GithubService
  ) {
    this.userState = this.userStore.getState().user!;
  }

  public hasConfirmedOrganization: boolean = false;

  ngOnInit(): void {
    this.loadingProjects = true;
    this.githubService
      .getOrganizationMembershipStatus(this.userState.gitHubUser)
      .pipe(
        switchMap((data) => {
          this.hasConfirmedOrganization = data.data?.status === 'active';
          console.log(this.hasConfirmedOrganization);
          if (this.hasConfirmedOrganization) {
            return this.organizationsService.getOrganizationsWithUser(
              this.userState.id
            );
          } else {
            return of(null);
          }
        })
      )
      .subscribe((response) => {
        if (response) {
          if (response.isSuccess) {
            this.organizations = response.data?.items ?? [];

            this.organizations.forEach((org) => {
              if (org.ownerId === this.userState.id) {
                this.myOrganizations.push(org);
              } else {
                this.participatingOrganizations.push(org);
              }
            });

            this.selectedOrganization = this.organizations[0] ?? null;
            this.projects = this.selectedOrganization?.projects ?? [];
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
        }
        this.loadingProjects = false;
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
    console.log(projectName);
    this.router.navigate([
      'main',
      this.selectedOrganization?.name,
      projectName,
    ]);
  }
}
