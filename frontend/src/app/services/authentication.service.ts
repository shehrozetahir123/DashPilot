import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { LoginRequest } from '../Models/Authentication/login-request';
import { LoginResponse } from '../Models/Authentication/login-response';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  loggedInUser: LoginResponse;
  public user = signal<LoginResponse | null>(null);
  public userCopy = new BehaviorSubject<LoginResponse>(null);
  private apiUrl = environment.apiUrl;
  isLoggedIn: boolean = false;
  constructor(private httpClient: HttpClient, private router: Router, private toastr: ToastrService) {

  }


  Login(loginRequest: LoginRequest) {
    this.httpClient.post<LoginResponse>(`${this.apiUrl}/UserAccount/authenticate`, loginRequest)
      .subscribe({
        next: (res: LoginResponse) => {
          console.log(res);
          this.loggedInUser = res;
          localStorage.setItem("loggedInUser", JSON.stringify(this.loggedInUser));
          this.isLoggedIn = true;
          this.user.set(this.loggedInUser);
          this.router.navigate(['dashboard']);
          this.toastr.success('Login Successful!', 'Success');
        },
        error: (error) => {
          console.error('Login failed', error.error);
          if (error.status === 400) {
            this.toastr.error(error.error, 'Login Failed');
          } else {
            this.toastr.error('An unexpected error occurred. Please try again.', 'Error');
          }
        }
      });
  }
  Logout() {
    localStorage.clear();
    this.isLoggedIn = false;
    this.user.set(null);
    this.router.navigate(['login']);
  }
  AutoLogin() {
    const user = localStorage.getItem('loggedInUser');
    if (!user) {
      return;
    }
    const loggedInUser = JSON.parse(user) as LoginResponse;
    this.user.set(loggedInUser);
    this.isLoggedIn = true;
  }

  IsUserLoggedIn(): boolean {
    const user = localStorage.getItem('loggedInUser');
    return !!user;
  }

}
