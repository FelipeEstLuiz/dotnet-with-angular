import { inject, Injectable, signal } from '@angular/core';
import { Member } from '../../types/member';
import { MemberUpdate } from '../../types/member-update';
import { Photo } from '../../types/photo';
import { ApiResponse } from './../../types/api-response';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private httpService = inject(HttpService);
  editMode = signal(false);
  member = signal<Member | null>(null);

  enableEditMode() {
    this.setEditMode(true);
  }

  disableEditMode() {
    this.setEditMode(false);
  }

  setEditMode(value: boolean) {
    this.editMode.set(value);
  }

  async getAll(pageNumber = 1, pageSize = 5): Promise<ApiResponse<Member[]>> {
    return await this.httpService.getApiResult<Member[]>(
      'user',
      pageNumber,
      pageSize
    );
  }

  async getByName(username: string): Promise<Member> {
    const member = await this.httpService.get<Member>('user/' + username);
    this.member.set(member);
    return member;
  }

  async getById(id: number): Promise<Member> {
    const member = await this.httpService.get<Member>('user/' + id);
    this.member.set(member);
    return member;
  }

  async updateById(id: number, member: MemberUpdate) {
    await this.httpService.put('user/' + id, member);
  }

  async getMemberPhotoById(id: number): Promise<Photo[]> {
    return await this.httpService.get<Photo[]>('user/' + id + '/photos');
  }

  async uploadFile(file: File) {
    const formData = new FormData();
    formData.append('file', file);
    return await this.httpService.post<Photo>('user/add-photo', formData);
  }

  async setMainPhoto(photo: Photo) {
    return this.httpService.put('user/set-main-photo/' + photo.id, {});
  }

  async deletePhoto(photoId: number) {
    return this.httpService.delete('user/photo/' + photoId);
  }
}
