import { inject, Injectable, signal } from '@angular/core';
import { HttpService } from './http.service';
import { Photo } from '../../types/photo';
import { Member } from '../../types/member';
import { MemberUpdate } from '../../types/member-update';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  private httpService = inject(HttpService);
  editMode = signal(false);

  async getAll(): Promise<Member[]> {
    return await this.httpService.get<Member[]>('user');
  }

  async getByName(username: string): Promise<Member> {
    return await this.httpService.get<Member>('user/' + username);
  }

  async getById(id: number): Promise<Member> {
    return await this.httpService.get<Member>('user/' + id);
  }

  async updateById(id: number, member: MemberUpdate) {
    await this.httpService.put('user/' + id, member);
  }

  async getMemberPhotoById(id: number): Promise<Photo[]> {
    return await this.httpService.get<Photo[]>('user/' + id + '/photos');
  }
}
