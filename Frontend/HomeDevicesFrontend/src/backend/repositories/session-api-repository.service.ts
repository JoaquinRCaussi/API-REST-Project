import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import domovizApi from '../../environments/environment.local';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SessionApiRepositoryService extends ApiRepository {
  constructor(http: HttpClient) {
    super(domovizApi.domovizApi, 'login', http);
  }

  public login(
    credentials: LoginRequest
  ): Observable<LoginResponse> {
    return this.post(credentials);
  }
}