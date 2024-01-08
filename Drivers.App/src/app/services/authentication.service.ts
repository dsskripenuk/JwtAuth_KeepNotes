import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Login } from '../models/login';
import { Register } from '../models/register';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { JwtAuth } from '../models/jwtAuth';
import { AuthStateService } from './auth-state.service';
@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  registerUrl = "AuthManagment/Register"
  loginUrl = "AuthManagment/Login"

  constructor(private http: HttpClient, private authStateService: AuthStateService) { 
    const token = this.authStateService.getAuthTokenFromMemory();
    if (token) {
      this.authStateService.setAuthState(true);
    }
  }

  public register(user: Register): Observable<JwtAuth>{
    return this.http.post<JwtAuth>(`${environment.apiUrl}/${this.registerUrl}`, user);
  }

  public login(user: Login): Observable<JwtAuth>{
    return this.http.post<JwtAuth>(`${environment.apiUrl}/${this.loginUrl}`, user)
  }

  public logout(): void {
    localStorage.removeItem('jwtToken');
    this.authStateService.setAuthState(false);
  }
}
