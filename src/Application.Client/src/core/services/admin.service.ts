import { inject, Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { User } from '../../types/user';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private httpService = inject(HttpService);

  async getUserWithRoles() {
    return await this.httpService.get<User[]>('admin/users-with-roles');
  }

  async updateUserRoles(userId: string, roles: string[]) {
    return await this.httpService.post<string[]>(
      'admin/edit-roles/' + userId + '?roles=' + roles,
      {}
    );
  }
}
