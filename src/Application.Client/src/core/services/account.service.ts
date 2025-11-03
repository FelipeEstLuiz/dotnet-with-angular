import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../../types/api-response';
import { Login } from '../../types/login';
import { User } from '../../types/user';
import { UserRegister } from '../../types/user-register';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrlV1;
  currentUser = signal<User | null>(null);

  login(model: Login) {
    return this.http
      .post<ApiResponse<User>>(this.baseUrl + 'Account/Login', model)
      .pipe(
        tap((response) => {
          if (response && response.data) {
            this.setCurrentUser(response.data);
          }
        })
      );
  }

  register(model: UserRegister) {
    return this.http
      .post<ApiResponse<User>>(this.baseUrl + 'Account', model)
      .pipe(
        tap((response) => {
          if (response && response.data) {
            this.setCurrentUser(response.data);
          }
          return response?.data;
        })
      );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }

  setCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
  }
}
