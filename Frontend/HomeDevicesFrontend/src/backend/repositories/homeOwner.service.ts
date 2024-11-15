import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import domovizApi from '../../environments/environment.local';
import { Observable } from 'rxjs';
import { signUpRequest } from '../models/in/signuprequest';
import { SignUpResponse } from '../models/out/signupresponse';


@Injectable({
    providedIn: 'root',
  })
  export class HomeOwnerRepository extends ApiRepository {
    constructor(http: HttpClient) {
      super(domovizApi.domovizApi, 'home-owner', http);
    }
  
    public signup(
      credentials: signUpRequest
    ): Observable<SignUpResponse> {
      return this.post(credentials);
    }
}
