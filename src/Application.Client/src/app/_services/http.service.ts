import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom, lastValueFrom, map } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../_model/api-response';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  private baseUrl = environment.apiUrlV1;

  get<T>(url: string): Promise<T> {
    return firstValueFrom(
      this.http
        .get<ApiResponse<T>>(this.baseUrl + url, this.getHttpOptions())
        .pipe(map((response) => response.data!))
    );
  }

  async post<T>(url: string, body: any): Promise<T> {
    return await lastValueFrom(
      this.http
        .post<ApiResponse<T>>(this.baseUrl + url, body, this.getHttpOptions())
        .pipe(map((response) => response.data!))
    );
  }

  async put(url: string, body: any) {
    return await lastValueFrom(
      this.http.put<void>(this.baseUrl + url, body, this.getHttpOptions())
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
