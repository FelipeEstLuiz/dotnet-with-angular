import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../_model/api-response';
import { AccountService } from './account.service';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  private baseUrl = environment.apiUrlV1;

  get<T>(url: string) {
    return this.http.get<ApiResponse<T>>(
      this.baseUrl + url,
      this.getHttpOptions()
    );
  }

  post<T>(url: string, body: any): Observable<ApiResponse<T>> {
    return this.http.post<ApiResponse<T>>(
      this.baseUrl + url,
      body,
      this.getHttpOptions()
    );
  }

  private getHttpOptions() {
    const user = this.accountService.currentUser();
    if (user) {
      return {
        headers: new HttpHeaders({ Authorization: `Bearer ${user.token}` }),
      };
    }
    return {};
  }
}
