import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, of } from 'rxjs';
import {
  HttpResponseModel,
  ResponseCollection,
} from '../../../../../../core/models/http.response';

@Injectable({
  providedIn: 'root',
})
export class WikiService {
  constructor(private httpClient: HttpClient) {}

  getWikis(
    projectId: string
  ): Observable<HttpResponseModel<ResponseCollection<ProjectWikiNode>>> {
    return this.httpClient
      .get(`https://localhost:7048/api/ProjectWikis/${projectId}`)
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

  getWikiData(wikiId: string): Observable<HttpResponseModel<ProjectWikiData>> {
    return this.httpClient
      .get(`https://localhost:7048/api/ProjectWikis/wikis/${wikiId}`)
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

  updateWiki(
    wikiId: string,
    content: string
  ): Observable<HttpResponseModel<ProjectWikiData>> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.httpClient
      .put(
        `https://localhost:7048/api/ProjectWikis/wikis/${wikiId}`,
        { content },
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

  createWiki(
    content: string,
    title: string,
    projectId: string,
    createdById: string,
    parentId: string | null
  ): Observable<HttpResponseModel<ProjectWikiData>> {
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.httpClient
      .post(
        `https://localhost:7048/api/ProjectWikis`,
        { title, content, projectId, createdById, parentId },
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

export interface ProjectWikiNode {
  id: string;
  parentId: string;
  title: string;
  content: string;
  children: ProjectWikiNode[];
}

export interface ProjectWikiData {
  id: string;
  title: string;
  createdOn: string;
  content: string;
  createdBy: string;
}
