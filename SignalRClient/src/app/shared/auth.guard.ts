import { AuthService } from './auth.service';
import { Injectable, OnInit, OnDestroy } from '@angular/core';
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from '@angular/router';
import { Subscription } from 'rxjs';

@Injectable()
export class AuthGuard implements CanActivate, OnInit, OnDestroy {
  isAuthSub: Subscription;
  isAuth: boolean;
  ngOnDestroy(): void {
    this.isAuthSub.unsubscribe();
  }
  ngOnInit(): void {

  }
  constructor(private router: Router, private authService: AuthService) {
    this.isAuthSub = this.authService.isAuthenticated.subscribe(
      res => (this.isAuth = res)
    );
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.isAuth) {
      return true;
    }

    this.router.navigate(['login']);
    return false;
  }
}
