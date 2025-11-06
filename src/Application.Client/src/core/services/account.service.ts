import { inject, Injectable, signal } from '@angular/core';
import { Login } from '../../types/login';
import { User } from '../../types/user';
import { UserRegister } from '../../types/user-register';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private httpService = inject(HttpService);
  currentUser = signal<User | null>(null);

  async login(model: Login) {
    const user = await this.httpService.post<User>('Account/Login', model);

    if (user) {
      this.setCurrentUser(user);
    }

    return user;
  }

  async register(model: UserRegister) {
    const user = await this.httpService.post<User>('Account', model);

    if (user) {
      this.setCurrentUser(user);
    }

    return user;
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
