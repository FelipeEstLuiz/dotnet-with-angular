import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../../types/api-response';

@Component({
  selector: 'app-test-errors',
  imports: [],
  templateUrl: './test-errors.component.html',
  styleUrl: './test-errors.component.css',
})
export class TestErrorsComponent {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrlV1;
  validationErrors = signal<string[]>([]);

  get400Error() {
    this.getBuggyApiError('bad-request');
  }

  get401Error() {
    this.getBuggyApiError('auth');
  }

  get403Error() {
    this.getBuggyApiError('forbidden');
  }

  get404Error() {
    this.getBuggyApiError('not-found');
  }

  get500Error() {
    this.getBuggyApiError('server-error');
  }

  getBuggyApiError(
    action: 'not-found' | 'server-error' | 'forbidden' | 'auth' | 'bad-request'
  ) {
    this.http
      .get<ApiResponse<string>>(this.baseUrl + 'buggy/' + action)
      .subscribe({
        next: (response) => console.log(response),
        error: (error) => {
          console.log(error);
          this.validationErrors.set(error);
        },
      });
  }
}
