import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';

import { AuthService } from '../shared/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {


  public username: string;
  public password: string;
  public error: string;

  constructor(private auth: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  public submit() {
    this.auth.login(this.username, this.password)
      .pipe(first())
      .subscribe(
        result => this.router.navigate(['news']),
        err => this.error = 'Could not authenticate'
      );
  }


}
