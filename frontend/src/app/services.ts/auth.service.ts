// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5291/api/Auth';

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<{ token: string, success: boolean }> {
    return this.http.post<{ token: string, success: boolean }>(`${this.apiUrl}/login`, { username, password })
      .pipe(
        tap(res => {
          if (res.success && res.token) {
            localStorage.setItem('token', res.token);
          }
        })
      );
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}

