import { Component } from '@angular/core';
import { Login } from '../models/login';
import { Register } from '../models/register';
import { JwtAuth } from '../models/jwtAuth';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';
import { AuthStateService } from '../services/auth-state.service';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrl: './authentication.component.css'
})
export class AuthenticationComponent {
  title = 'Drivers.App';
  loginDto = new Login();
  registerDto = new Register();
  jwtDto = new JwtAuth();
  showLoginForm: boolean = true;
  showPassword: boolean = false;

  constructor(
    private authService: AuthenticationService,
    public authStateService: AuthStateService,
    private router: Router,
  ){}

  toggleForm() {
    this.showLoginForm = !this.showLoginForm;
  }

  register(registerDto: Register){
    this.authService.register(registerDto).subscribe((jwtDto) => {
      localStorage.setItem('jwtToken', jwtDto.token);
      this.authStateService.setAuthState(true);
      this.router.navigate(['/notes']);
    });
    }

  login(loginDto: Login){
    this.authService.login(loginDto).subscribe((jwtDto) => {
      localStorage.setItem('jwtToken', jwtDto.token);
      this.authStateService.setAuthState(true);
      this.router.navigate(['/notes']);
    });
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }
}
