import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GetUserByMailResponse } from '../models/out/get-user-by-mail-response';
import { UserApiRepository } from '../repositories/user-api-repository.service';
import { UserResponse } from "../models/out/user-response";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private userApiRepository: UserApiRepository) {}

  getUserByMail(mail: string): Observable<GetUserByMailResponse> {
    return this.userApiRepository.getUserByMail(mail);
  }

  getUser(userId: string): Observable<GetUserByMailResponse> {
    return this.userApiRepository.getUser(userId);
  }

  getUsers(): Observable<UserResponse[]> {
    return this.userApiRepository.getUsers();
  }

}
