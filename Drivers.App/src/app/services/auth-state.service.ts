import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthStateService {
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor() {}

  private getAuthStateFromMemory(): boolean {
    const token = localStorage.getItem('jwtToken');
    return !!token;
  }

  setAuthState(isAuthenticated: boolean): void {
    this.isAuthenticatedSubject.next(isAuthenticated);
  }

  getAuthState(): boolean {
    return this.isAuthenticatedSubject.value;
  }

  setAuthTokenInMemory(token: string): void {
    localStorage.setItem('jwtToken', token);
    this.setAuthState(true);
  }

  getAuthTokenFromMemory(): string | null {
    return localStorage.getItem('jwtToken');
  }
}