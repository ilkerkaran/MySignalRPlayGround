import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from './../shared/auth.service';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit, OnDestroy {
  loginLogoutText: string;
  isAuthenticateSub: Subscription;
  isAuthenticated = false;

  constructor(private authService: AuthService, private router: Router) {
    this.isAuthenticateSub = authService.isAuthenticated.subscribe(res => {
      this.isAuthenticated = res;
      this.loginLogoutText = res ? 'Logout' : 'Login';
    });
  }

  ngOnInit() {
    this.loginLogoutText =  this.isAuthenticated ? 'Logout' : 'Login';
  }

  ngOnDestroy(): void {
    this.isAuthenticateSub.unsubscribe();
  }

  loginLogout() {
    if (this.isAuthenticated) {
      this.authService.logout();
      this.router.navigate(['/home']);
    } else {
      this.router.navigate(['/login']);
    }
  }
}
