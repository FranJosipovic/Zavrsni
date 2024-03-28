import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../../../core/user/interfaces';
import { Observable, catchError, map, of } from 'rxjs';
import { HttpResponseModel } from '../../../core/models/http.response';

@Injectable({
  providedIn: 'root',
})
export class OrganizationService {
  constructor(private httpClient: HttpClient) {}

  getOrganizationsWithUser(
    userId: string
  ): Observable<HttpResponseModel<OrganizationsResponse>> {
    return this.httpClient
      .get(`https://localhost:7048/api/Organization/with-user/${userId}`)
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

  getOrganizationDetails(
    organizationId: string
  ): Observable<HttpResponseModel<OrganizationDetails>> {
    return this.httpClient
      .get(`https://localhost:7048/api/Organization/${organizationId}`)
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

  updateOrganization(
    updateOrganizationDTO: UpdateOrganizationDTO
  ): Observable<HttpResponseModel<Organization>> {
    return this.httpClient.put(
      'https://localhost:7048/api/Organization/update',
      updateOrganizationDTO
    ).pipe(
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

export type Organization = {
  id: string;
  ownerId: string;
  name: string;
  description: string;
  owner: User;
  projects: Project[];
};

export type OrganizationsResponse = {
  items: Organization[];
  count: number;
};

export type Project = {
  id: string;
  description: string;
  name: string;
  organizationId: string;
};

export interface OrganizationDetails extends Organization {
  users: User[];
}

export type UpdateOrganizationDTO = {
  id: string;
  name: string;
  description: string;
};
