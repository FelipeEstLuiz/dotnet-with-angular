import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AccountService } from '../../core/services/account.service';
import { Login } from '../../types/login';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css',
})
export class NavComponent {
  protected accountService = inject(AccountService);
  private router = inject(Router);
  protected creds: Login = { email: '', password: '' };

  login() {
    this.accountService.login(this.creds).subscribe({
      next: (_) => {
        this.router.navigateByUrl('/members');
        this.creds = { email: '', password: '' };
      },
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
