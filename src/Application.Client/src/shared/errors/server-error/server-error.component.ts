import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ApiResponse } from '../../../types/api-response';
import { Location } from '@angular/common';

@Component({
  selector: 'app-server-error',
  imports: [],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.css',
})
export class ServerErrorComponent {
  protected error: ApiResponse<any>;
  protected errors: string[];
  private router = inject(Router);
  private location = inject(Location);

  protected showDetails = false;

  constructor() {
    const navigation = this.router.currentNavigation();
    this.error = navigation?.extras?.state?.['error'];
    this.errors = this.error?.errors ?? [];
  }

  detailsToggle() {
    this.showDetails = !this.showDetails;
  }

  goBack() {
    this.location.back();
  }
}
