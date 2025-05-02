import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { ApiResponse } from '../_model/api-response';
import { User } from '../_model/user';
import { map } from 'rxjs';
import { UserRegister } from '../_model/user-register';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null);

  login(model: any) {
    return this.http
      .post<ApiResponse<User>>(this.baseUrl + 'v1/Account/Login', model)
      .pipe(
        map((response) => {
          if (response && response.data) {
            localStorage.setItem('user', JSON.stringify(response.data));
            this.currentUser.set(response.data);
          }
        })
      );
  }

  register(model: UserRegister) {
    return this.http
      .post<ApiResponse<User>>(this.baseUrl + 'v1/Account', model)
      .pipe(
        map((response) => {
          if (response && response.data) {
            localStorage.setItem('user', JSON.stringify(response.data));
            this.currentUser.set(response.data);
          }
          return response?.data;
        })
      );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
