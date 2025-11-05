import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom, lastValueFrom, map } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../../types/api-response';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrlV1;

  get<T>(url: string): Promise<T> {
    return firstValueFrom(
      this.http
        .get<ApiResponse<T>>(this.baseUrl + url)
        .pipe(map((response) => response.data!))
    );
  }

  async post<T>(url: string, body: any): Promise<T> {
    return await lastValueFrom(
      this.http
        .post<ApiResponse<T>>(this.baseUrl + url, body)
        .pipe(map((response) => response.data!))
    );
  }

  async put(url: string, body: any) {
    return await lastValueFrom(this.http.put<void>(this.baseUrl + url, body));
  }

  async delete(url: string) {
    return await lastValueFrom(this.http.delete<void>(this.baseUrl + url));
  }
}
