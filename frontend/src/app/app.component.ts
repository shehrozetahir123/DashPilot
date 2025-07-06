import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './services/authentication.service';
import { LoginResponse } from './Models/Authentication/login-response';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  isCollapsed = false;
  isLoggedIn = false;
  loggedInUser: boolean = false;
  constructor(public authService: AuthenticationService, private router: Router) { }
  ngOnInit() {
  }

  Logout() {
    this.authService.Logout();
  }

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
  }

}
