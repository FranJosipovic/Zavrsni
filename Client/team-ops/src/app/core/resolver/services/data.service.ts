import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  constructor(private httpClient: HttpClient) {}

  resolveOrganizationId(organizationName: string): Observable<any> {
    return this.httpClient
      .get(
        `https://localhost:7048/api/Organization/${organizationName}/organizationId`
      )
      .pipe(map((res: any) => res.data));
  }

  resolveProjectId(projectName: string, organizationId: string): Observable<any> {
    return this.httpClient
      .get(
        `https://localhost:7048/api/Project/${projectName}/projectId/${organizationId}`
      )
      .pipe(map((res: any) => res.data));
  }
}
