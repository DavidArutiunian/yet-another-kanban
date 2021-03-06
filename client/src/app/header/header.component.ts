import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  loggedIn$: Observable<boolean>;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.loggedIn$ = this.authService.loggedIn$;
  }

  onLogout(): void {
    this.authService.logout();

    this.loggedIn$.subscribe((logged) => {
      if (!logged) {
        this.router.navigateByUrl('/login');
      }
    });
  }
}
