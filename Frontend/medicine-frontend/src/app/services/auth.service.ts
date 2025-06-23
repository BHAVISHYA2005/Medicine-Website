import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../environment/environment';
import { LoginRequest, RegisterRequest, LoginResponse } from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<any>;
  public currentUser: Observable<any>;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<any>(JSON.parse(localStorage.getItem('currentUser') || 'null'));
    this.currentUser = this.currentUserSubject.asObservable();
  }

  public get currentUserValue() {
    return this.currentUserSubject.value;
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, credentials)
      .pipe(map(response => {
        // Store user details and JWT token in local storage
        localStorage.setItem('currentUser', JSON.stringify(response));
        this.currentUserSubject.next(response);
        return response;
      }));
  }

  register(userData: RegisterRequest): Observable<any> {
    return this.http.post(`${environment.apiUrl}/auth/register`, userData);
  }

  logout(): void {
    // Remove user from local storage and set current user to null
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  isAuthenticated(): boolean {
    const user = this.currentUserValue;
    return user && user.token && !this.isTokenExpired(user.token);
  }

  getToken(): string | null {
    const user = this.currentUserValue;
    return user ? user.token : null;
  }

  refreshToken(): Observable<any> {
    return this.http.post(`${environment.apiUrl}/auth/refresh-token`, {
      token: this.getToken()
    });
  }

  forgotPassword(email: string): Observable<any> {
    return this.http.post(`${environment.apiUrl}/auth/forgot-password`, { email });
  }

  resetPassword(token: string, password: string): Observable<any> {
    return this.http.post(`${environment.apiUrl}/auth/reset-password`, { token, password });
  }

  confirmEmail(token: string): Observable<any> {
    return this.http.post(`${environment.apiUrl}/auth/confirm-email`, { token });
  }

  private isTokenExpired(token: string): boolean {
    try {
      const decoded = JSON.parse(atob(token.split('.')[1]));
      return decoded.exp < Date.now() / 1000;
    } catch {
      return true;
    }
  }
}
