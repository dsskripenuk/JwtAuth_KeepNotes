import { Component, OnInit } from '@angular/core';
import { AuthStateService } from './services/auth-state.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  constructor(public authStateService: AuthStateService, private router: Router) {}

  ngOnInit(): void {
    const token = this.authStateService.getAuthTokenFromMemory();
    if (token) {
      this.authStateService.setAuthState(true);
    }
  }

  logout() {
    localStorage.removeItem('jwtToken');
    this.authStateService.setAuthState(false);
    this.router.navigate(['/authentication']);
  }
}