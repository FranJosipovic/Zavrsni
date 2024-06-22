import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../../../core/user/interfaces';
import { Observable, catchError, map, of } from 'rxjs';
import {
  HttpResponseModel,
  ResponseCollection,
} from '../../../core/models/http.response';

@Injectable({
  providedIn: 'root',
})
export class OrganizationService {
  constructor(private httpClient: HttpClient) {}

  createOrganization(
    updateOrganizationDTO: CreateOrganizationDTO
  ): Observable<HttpResponseModel<Organization>> {
    return this.httpClient
      .post('https://localhost:7048/api/Organization', updateOrganizationDTO)
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
    return this.httpClient
      .put(
        'https://localhost:7048/api/Organization/update',
        updateOrganizationDTO
      )
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

  getUsers(
    organizationId: string
  ): Observable<HttpResponseModel<ResponseCollection<OrganizationUser>>> {
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

  searchUsers(
    q: string
  ): Observable<HttpResponseModel<ResponseCollection<OrganizationUser>>> {
    return this.httpClient
      .get(`https://localhost:7048/api/User/user/${q}`)
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

  addUserToOrganization(userId: string, organizationId: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    return this.httpClient
      .post(
        'https://localhost:7048/api/Organization/add-user',
        {
          userId,
          organizationId,
        },
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
export type CreateOrganizationDTO = {
  name: string;
  description: string;
  ownerId: string;
};

export interface OrganizationUser {
  id: string;
  email: string;
  name: string;
  surname: string;
  username: string;
}
