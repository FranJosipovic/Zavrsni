import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { User } from '../../../../core/user/interfaces';
import { UserStore } from '../../../../store/user.store';
import {
  Organization,
  Project,
  OrganizationService,
  OrganizationUser,
} from '../../services/organization.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CreateOrganizationDialogComponent } from './dialogs/create-organization-dialog/create-organization-dialog.component';
import { CreateProjectDialogComponent } from './dialogs/create-project-dialog/create-project-dialog.component';
import { FormControl } from '@angular/forms';

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
    private dialog: MatDialog
  ) {
    this.userState = this.userStore.getState().user!;
  }

  public hasConfirmedOrganization: boolean = false;

  ngOnInit(): void {
    this.loadOrganizations();
    this.userQueryListener();
  }

  onOrganizationSelect(organizationId: string): void {
    if (this.selectedOrganization?.id == organizationId) return;
    this.organizations.forEach((organization) => {
      if (organization.id == organizationId) {
        this.selectedOrganization = organization;
        this.projects = organization.projects;
        this.getUsers();
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

  openCreateOrganizationDialog() {
    const dialogRef = this.dialog.open(CreateOrganizationDialogComponent, {
      height: '400px',
      width: '600px',
      data: { ownerId: this.userState.id },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result.refresh) {
        this.loadOrganizations();
      }
    });
  }

  openCreateProjectDialog() {
    const dialogRef = this.dialog.open(CreateProjectDialogComponent, {
      height: '400px',
      width: '600px',
      data: {
        ownerId: this.userState.id,
        organizationId: this.selectedOrganization?.id,
      },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result.refresh) {
        this.loadOrganizations();
      }
    });
  }

  private loadOrganizations() {
    this.loadingProjects = true;
    this.organizationsService
      .getOrganizationsWithUser(this.userState.id)
      .subscribe((response) => {
        if (response) {
          if (response.isSuccess) {
            this.myOrganizations = [];
            this.participatingOrganizations = [];

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
            this.getUsers();
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

  displayedUsersColumns: string[] = ['Name', 'Surname', 'Username', 'Email'];

  public organizationUsers: OrganizationUser[] = [];

  private getUsers() {
    this.organizationsService
      .getUsers(this.selectedOrganization?.id!)
      .subscribe((data) => {
        if (data.isSuccess) {
          this.organizationUsers = data.data?.items!;
        }
      });
  }

  public usersQuery = new FormControl('');
  public searchedUsers: OrganizationUser[] = [];
  userQueryListener() {
    this.usersQuery.valueChanges.subscribe((q) => {
      if (q == '') {
        this.searchedUsers = [];
      } else {
        this.organizationsService.searchUsers(q!).subscribe((data) => {
          if (data.isSuccess) {
            this.searchedUsers = data.data?.items!.filter(
              (user) =>
                !this.organizationUsers.some(
                  (orgUser) => orgUser.id === user.id
                )
            )!;
          }
        });
      }
    });
  }

  addUserToOrganization(user: OrganizationUser) {
    this.usersQuery.setValue('');
    this.organizationsService
      .addUserToOrganization(user.id, this.selectedOrganization?.id!)
      .subscribe((data) => {
        if (data.isSuccess) {
          this.getUsers();
        }
      });
  }
}
