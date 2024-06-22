import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, catchError, of } from 'rxjs';
import { HttpResponseModel } from '../../../../../core/models/http.response';
import { Project } from '../../../../main/services/organization.service';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  constructor(private httpClient: HttpClient) {}

  createProject(
    createProjectDTO: CreateProjectDTO
  ): Observable<HttpResponseModel<Project>> {
    return this.httpClient
      .post('https://localhost:7048/api/Project', createProjectDTO)
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

export interface CreateProjectDTO {
  organizationId: string;
  organizationOwnerId: string;
  name: string;
  description: string;
}
