import { Injectable } from '@angular/core';
import { signUpRequest } from '../models/in/signuprequest';
import { SignUpResponse } from '../models/out/signupresponse';
import { HomeOwnerRepository } from '../repositories/homeOwner.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeOwnerService {
  constructor(private homeOwnerRepository: HomeOwnerRepository) {}

  signup(credentials: signUpRequest): Observable<SignUpResponse> {
    return this.homeOwnerRepository.signup(credentials);
  }
  }
