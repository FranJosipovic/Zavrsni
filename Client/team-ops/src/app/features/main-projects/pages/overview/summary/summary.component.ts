import { Component, OnInit } from '@angular/core';
import {
  SummaryService,
  Summary_ProjectDetails,
  Summary_ProjectUser,
} from './services/summary.service';
import { UserStore } from '../../../../../store/user.store';
import { User } from '../../../../../core/user/interfaces';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.scss',
})
export class SummaryComponent implements OnInit {
  private projectId;
  private organizationId;
  public projectUsers: Summary_ProjectUser[] = [];
  public userState: User;

  public projectDetails: Summary_ProjectDetails | null = null;

  public organizationUsers: Summary_ProjectUser[] = [];

  constructor(
    private summaryService: SummaryService,
    private userStore: UserStore
  ) {
    this.projectId = localStorage.getItem('projectId')!;
    this.organizationId = localStorage.getItem('organizationId')!;
    this.userState = userStore.getState().user!;
  }

  ngOnInit(): void {
    this.loadDetails();
  }

  private loadDetails() {
    this.summaryService.getProjectDetails(this.projectId).subscribe((data) => {
      if (data.isSuccess) {
        this.projectDetails = data.data;
        this.loadProjectUsers();
      }
    });
  }

  displayedProjectUsersColumns: string[] = [
    'Name',
    'Surname',
    'Username',
    'Email',
  ];

  private loadProjectUsers() {
    this.summaryService.getProjectUsers(this.projectId).subscribe((data) => {
      if (data.isSuccess) {
        this.projectUsers = data.data?.items!;
        this.loadOrganizationUsers();
      }
    });
  }

  displayedOrganizationUsersColumns: string[] = [
    'Name',
    'Surname',
    'Username',
    'Email',
    'Action',
  ];
  private loadOrganizationUsers() {
    this.summaryService
      .getOrganizationUsers(this.organizationId)
      .subscribe((data) => {
        if (data.isSuccess) {
          this.organizationUsers = data.data?.items!.filter(
            (organizationUser) =>
              !this.projectUsers.some(
                (projectUser) => projectUser.id === organizationUser.id
              )
          )!;
        }
      });
  }

  addUserToProject(user: Summary_ProjectUser) {
    this.summaryService
      .addUserToProject(user.id, this.projectId, this.userState.id)
      .subscribe((data) => {
        if (data.isSuccess) {
          this.loadDetails();
        }
      });
  }
}
