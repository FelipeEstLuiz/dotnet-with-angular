import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingService } from '../../_services/loading.service';

@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.css'],
})
export class AppLoadingComponent {
  loading$ = inject(LoadingService).loading$;

  constructor() {
    this.loading$.subscribe();
  }
}
