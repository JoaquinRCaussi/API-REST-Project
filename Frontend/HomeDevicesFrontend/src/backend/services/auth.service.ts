import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';
import { SessionApiRepositoryService } from '../repositories/session-api-repository.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private sessionApiRepository: SessionApiRepositoryService) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.sessionApiRepository.login(credentials).pipe(
      tap((response) => {
        localStorage.setItem('token', response.token);
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }
}
