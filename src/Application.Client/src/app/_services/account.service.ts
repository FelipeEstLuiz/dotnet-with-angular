import { Injectable, inject, signal } from '@angular/core';
import { User } from '../_model/user';
import { map } from 'rxjs';
import { UserRegister } from '../_model/user-register';
import { environment } from '../../environments/environment';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private httpService = inject(HttpService);
  baseUrl = environment.apiUrlV1;
  currentUser = signal<User | null>(null);

  login(model: any) {
    return this.httpService
      .post<User>(this.baseUrl + 'Account/Login', model)
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
    return this.httpService
      .post<User>(this.baseUrl + 'Account', model)
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
