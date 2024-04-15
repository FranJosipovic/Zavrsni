import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { GithubMemebershipStatus, GithubService } from '../../../features/main/services/github.service';
import { UserStore } from '../../../store/user.store';
import { map } from 'rxjs';
import { HttpResponseModel } from '../../models/http.response';

export const githubOrganizationConfirmedResolver: ResolveFn<boolean> = (route, state) => {
  let service = inject(GithubService)
  let userStore = inject(UserStore)
  let userAuthState = userStore.getState()
  return service.getOrganizationMembershipStatus(userAuthState.user?.gitHubUser!).pipe(
    map((res:HttpResponseModel<GithubMemebershipStatus>) => {
      return res.data?.status == 'active'
    })
  )
};
