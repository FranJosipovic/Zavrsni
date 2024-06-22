import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, catchError, of, Observable } from 'rxjs';
import {
  HttpResponseModel,
  ResponseCollection,
} from '../../../../../../core/models/http.response';

@Injectable({
  providedIn: 'root',
})
export class SummaryService {
  constructor(private httpClient: HttpClient) {}

  getProjectUsers(
    projectId: string
  ): Observable<HttpResponseModel<ResponseCollection<Summary_ProjectUser>>> {
    return this.httpClient
      .get(`https://localhost:7048/api/User/by-project/${projectId}`)
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

  getProjectDetails(
    projectId: string
  ): Observable<HttpResponseModel<Summary_ProjectDetails>> {
    return this.httpClient
      .get(`https://localhost:7048/api/Project/details/${projectId}`)
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

  getOrganizationUsers(
    organizationId: string
  ): Observable<HttpResponseModel<ResponseCollection<Summary_ProjectUser>>> {
    return this.httpClient
      .get(`https://localhost:7048/api/User/by-organization/${organizationId}`)
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

  addUserToProject(
    userId: string,
    projectId: string,
    ownerId: string
  ): Observable<HttpResponseModel<null>> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    return this.httpClient
      .post(
        'https://localhost:7048/api/Project/user-to-project',
        { userId, projectId, ownerId },
        {
          headers,
        }
      )
      .pipe(
        map((response: any) => response),
        catchError((error) => {
          console.log(error);
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

export interface Summary_ProjectUser {
  name: string;
  surname: string;
  email: string;
  id: string;
}

export interface Summary_ProjectDetails {
  title: string;
  userStories: number;
  task_bugs: number;
  iterations: number;
  users: number;
}
