import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { EnvironmentUrlService } from './environment-url.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl: string;

  public isAuthenticated = new Subject<boolean>();

  constructor(
    private http: HttpClient,
    private environmentUrlService: EnvironmentUrlService,
    private router: Router
  ) {
    this.apiUrl = `${this.environmentUrlService.urlAddress}/api/auth`;
  }

  login(username: string, password: string): Observable<boolean> {
    return this.http
      .post<any>(this.apiUrl, {
        UserName: username,
        Password: password
      })
      .pipe(
        map(result => {
          localStorage.setItem('access_token', JSON.parse(result).access_token);
          localStorage.setItem('auth', JSON.stringify(result));
          this.isAuthenticated.next(true);
          this.router.navigate(['/news']);
          return true;
        })
      );
  }

  logout() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('auth');
    this.isAuthenticated.next(false);
  }
}
