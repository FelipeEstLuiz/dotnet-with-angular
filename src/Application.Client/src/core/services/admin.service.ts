import { inject, Injectable } from '@angular/core';
import { HttpService } from './http.service';
import { User } from '../../types/user';
import { Photo } from 'src/types/photo';

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

  async getPhotosForApproval() {
    return await this.httpService.get<Photo[]>('admin/photos-to-moderate');
  }

  async approvePhoto(photoId: number) {
    return await this.httpService.post('admin/approve-photo/' + photoId, {});
  }

  async rejectPhoto(photoId: number) {
    return await this.httpService.post('admin/reject-photo/' + photoId, {});
  }
}
