import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, of } from 'rxjs';
import { HttpResponseModel, ResponseCollection } from '../../../core/models/http.response';

@Injectable({
  providedIn: 'root',
})
export class GithubService {
  constructor(private httpClient: HttpClient) {}

  searchUser(
    q: string
  ): Observable<HttpResponseModel<ResponseCollection<GithubSearchItem>>> {
    return this.httpClient
      .get(`https://localhost:7048/api/User/user/github/${q}`)
      .pipe(
        map((response: any) => response),
        catchError((error) => {
          return of({
            statusCode: error.error.statusCode ?? 500,
            isSuccess: error.error.isSuccess ?? false,
            data: error.error.data ?? null,
            message: error.error.message ?? 'internal server error',
          });
        })
      );
  }

  getOrganizationMembershipStatus(
    githubUsername: string
  ): Observable<HttpResponseModel<GithubMemebershipStatus>> {
    return this.httpClient
      .get(`https://localhost:7048/api/User/user/github-membership-status/${githubUsername}`)
      .pipe(
        map((response: any) => response),
        catchError((error) => {
          return of({
            statusCode: error.error.statusCode ?? 500,
            isSuccess: error.error.isSuccess ?? false,
            data: error.error.data ?? null,
            message: error.error.message ?? 'internal server error',
          });
        })
      );
  }
}

export interface GithubMemebershipStatus{
  status: string
}

export interface GithubSearchItem {
  username: string;
  id: number;
  avatarUrl: string;
}
