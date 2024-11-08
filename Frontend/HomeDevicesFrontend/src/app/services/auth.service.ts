import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { LoginRequest } from '../interface/login-request';
import { LoginResponse } from '../interface/login-response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly _apiUrl = 'http://localhost:5242/api/login';

  constructor(private http:HttpClient) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this._apiUrl, credentials).pipe(
      tap((response) => {
        // Guarda el token en localStorage o sessionStorage
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
