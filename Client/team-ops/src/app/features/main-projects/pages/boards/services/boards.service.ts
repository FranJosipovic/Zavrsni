import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  HttpResponseModel,
  ResponseCollection,
} from '../../../../../core/models/http.response';
import { Observable, map, catchError, of } from 'rxjs';
import {
  workItemsPriority,
  workItemsStatus,
  workItemsType,
} from '../common/enums';
import { kStringMaxLength } from 'buffer';

@Injectable({
  providedIn: 'root',
})
export class BoardsService {
  constructor(private httpClient: HttpClient) {}

  getWorkItems(
    projectId: string
  ): Observable<HttpResponseModel<ResponseCollection<WorkItem>>> {
    return this.httpClient
      .get(`https://localhost:7048/api/WorkItems/${projectId}`)
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

  getWorkItemsByIteration(
    iterationId: string
  ): Observable<HttpResponseModel<ResponseCollection<UserStoryWithChildren>>> {
    return this.httpClient
      .get(`https://localhost:7048/api/WorkItems/by-iteration/${iterationId}`)
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

  getIterations(
    projectId: string
  ): Observable<HttpResponseModel<ResponseCollection<Iteration>>> {
    return this.httpClient
      .get(`https://localhost:7048/api/Iterations/${projectId}`)
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

  getProjectUsers(
    projectId: string
  ): Observable<HttpResponseModel<ResponseCollection<WorkItems_ProjectUser>>> {
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

  getUserStoriesByProject(
    projectId: string
  ): Observable<HttpResponseModel<ResponseCollection<WorkItem_UserStory>>> {
    return this.httpClient
      .get(`https://localhost:7048/api/WorkItems/user-story/${projectId}`)
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

  createNewWorkItem(
    createWorkItemDTO: CreateWorkItemDTO
  ): Observable<HttpResponseModel<WorkItem>> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });

    createWorkItemDTO.priority = Number(createWorkItemDTO.priority);
    createWorkItemDTO.type = Number(createWorkItemDTO.type);

    console.log(createWorkItemDTO);

    return this.httpClient
      .post('https://localhost:7048/api/WorkItems', createWorkItemDTO, {
        headers,
      })
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

  createNewIteration(
    newIterationRequest: CreateNewIterationRequest
  ): Observable<HttpResponseModel<string>> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    return this.httpClient
      .post('https://localhost:7048/api/Iterations', newIterationRequest, {
        headers,
      })
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

  updateWorkItem(
    updateWorkItemDto: UpdateWorkItemDTO
  ): Observable<HttpResponseModel<UserStory_Task_BugFix>> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    return this.httpClient
      .put('https://localhost:7048/api/WorkItems', updateWorkItemDto, {
        headers,
      })
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

  deleteWorkItem(id: string): Observable<HttpResponseModel<null>> {
    return this.httpClient
      .delete(`https://localhost:7048/api/WorkItems/${id}`)
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

export interface CreateNewIterationRequest {
  projectId: string;
  startsAt: Date;
  endsAt: Date;
}

export interface WorkItems_ProjectUser {
  name: string;
  surname: string;
  id: string;
}

export interface Iteration {
  number: number;
  id: string;
}

export interface WorkItem {
  id: string;
  iterationId: string;
  parentId: string | null;
  title: string;
  description: string;
  createdBy: WorkItem_CreatedBy;
  assignedTo: WorkItem_AssignedTo;
  type: string;
  priority: string;
  status: string;
}

export interface WorkItem_CreatedBy {
  name: string;
  id: string;
}

export interface WorkItem_AssignedTo {
  name: string;
  id: string | null;
}

export interface WorkItem_UserStory {
  id: string;
  title: string;
  iterationId: string;
}

export interface CreateWorkItemDTO {
  iterationId: string;
  createdById: string;
  assignedToId: string | null;
  parentId: string | null;
  title: string;
  description: string;
  type: number;
  priority: number;
}

export interface UserStoryWithChildren extends WorkItemCardDTO {
  children: UserStory_Task_BugFix[];
}

export interface UserStory_Task_BugFix extends WorkItemCardDTO {
  parentId: string;
}

export interface WorkItemCardDTO {
  id: string;
  iterationId: string;
  createdBy: WorkItem_CreatedBy;
  assignedTo: WorkItem_AssignedTo;
  title: string;
  description: string;
  type: workItemsType;
  priority: workItemsPriority;
  status: workItemsStatus;
}

export interface UpdateWorkItemDTO {
  id: string;
  assignedToId: string | null;
  title: string;
  description: string;
  priority: workItemsPriority;
  status: workItemsStatus;
}
