import { inject, Injectable, signal } from '@angular/core';
import { Login } from '../../types/login';
import { User } from '../../types/user';
import { UserRegister } from '../../types/user-register';
import { HttpService } from './http.service';
import { LikesService } from './likes.service';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private httpService = inject(HttpService);
  private likeService = inject(LikesService);
  private presenceService = inject(PresenceService);

  currentUser = signal<User | null>(null);

  async login(model: Login) {
    const user = await this.httpService.postWithCredentials<User>(
      'Account/Login',
      model
    );

    if (user) {
      await this.setCurrentUser(user);
      await this.startTokenRefreshInterval();
    }

    return user;
  }

  async register(model: UserRegister) {
    const user = await this.httpService.postWithCredentials<User>(
      'Account',
      model
    );

    if (user) {
      await this.setCurrentUser(user);
      await this.startTokenRefreshInterval();
    }

    return user;
  }

  async refreshToken() {
    return await this.httpService.postWithCredentials<User>(
      'Account/refresh-token',
      {}
    );
  }

  async startTokenRefreshInterval() {
    setInterval(async () => {
      try {
        const user = await this.refreshToken();
        if (user) {
          await this.setCurrentUser(user);
        }
      } catch (error) {
        console.error('Error refreshing token:', error);
        await this.logout();
      }
    }, 5 * 60 * 1000); //5 minutes
  }

  async logout() {
    await this.httpService.postWithCredentials('Account/logout', {});
    localStorage.removeItem('filters');
    this.likeService.clearLikeIds();
    this.currentUser.set(null);
    this.presenceService.stopHubConnection();
  }

  async setCurrentUser(user: User) {
    user.roles = this.getRolesFromToken(user);
    this.currentUser.set(user);
    await this.likeService.getLikeIds();

    if (!this.presenceService.serviceConnected)
      this.presenceService.createHubConnection(user);
  }

  private getRolesFromToken(user: User): string[] {
    const payload = user.token.split('.')[1];
    const decode = atob(payload);
    const jasonPayload = JSON.parse(decode);
    return Array.isArray(jasonPayload.role)
      ? jasonPayload.role
      : [jasonPayload.role];
  }

  isAdmin(): boolean {
    return this.isRole('Admin');
  }

  isModerator(): boolean {
    return this.isRole('Moderator');
  }

  private isRole(role: string): boolean {
    return this.currentUser()?.roles?.includes(role) ?? false;
  }
}
