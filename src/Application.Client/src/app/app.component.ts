import { Component, inject } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { NavComponent } from '../layout/nav/nav.component';
import { AppLoadingComponent } from '../shared/loading/loading.component';
import { ConfirmDialogComponent } from 'src/shared/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    NavComponent,
    AppLoadingComponent,
    ConfirmDialogComponent,
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  protected router = inject(Router);

  isHomeRoute(): boolean {
    return this.router.url === '/';
  }
}
