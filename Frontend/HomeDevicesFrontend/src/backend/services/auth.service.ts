import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { LoginRequest } from '../models/in/login-request';
import { LoginResponse } from '../models/out/login-response';
import { SessionApiRepositoryService } from '../repositories/session-api-repository.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private sessionApiRepository: SessionApiRepositoryService) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.sessionApiRepository.login(credentials).pipe(
      tap((response) => {
        localStorage.setItem('userRole', response.userRole);
        localStorage.setItem('token', response.token);
        localStorage.setItem('userId', response.userId);
      })
    );
  }

  logout(): void {
    localStorage.clear();
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }
}
