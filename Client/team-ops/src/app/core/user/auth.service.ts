import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, of } from 'rxjs';
import { SignInRequest, SignInResponse, SignUpRequest, SignUpResponse } from './interfaces';
import { HttpResponseModel } from '../models/http.response';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private httpClient: HttpClient) {}

  public signIn(
    requestModel: SignInRequest
  ): Observable<HttpResponseModel<SignInResponse>> {
    return this.httpClient
      .post('https://localhost:7048/api/User/signin', requestModel)
      .pipe(
        map((response: any) => response),
        catchError((error) => {
          return of({
            statusCode: error.error.statusCode,
            isSuccess: error.error.isSuccess,
            data: error.error.data,
            message: error.error.message,
          });
        })
      );
  }

  public signUp(
    requestModel: SignUpRequest
  ): Observable<HttpResponseModel<SignUpResponse>> {
    return this.httpClient
      .post('https://localhost:7048/api/User/signup', requestModel)
      .pipe(
        map((response: any) => response),
        catchError((error) => {
          return of({
            statusCode: error.error.statusCode,
            isSuccess: error.error.isSuccess,
            data: error.error.data,
            message: error.error.message,
          });
        })
      );
  }

}
