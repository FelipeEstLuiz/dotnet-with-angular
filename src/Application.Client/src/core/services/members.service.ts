import { inject, Injectable, signal } from '@angular/core';
import { HttpService } from './http.service';
import { Photo } from '../../types/photo';
import { Member } from '../../types/member';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private httpService = inject(HttpService);

  async getAll(): Promise<Member[]> {
    return await this.httpService.get<Member[]>('user');
  }

  async getByName(username: string): Promise<Member> {
    return await this.httpService.get<Member>('user/' + username);
  }

  async getById(id: number): Promise<Member> {
    return await this.httpService.get<Member>('user/' + id);
  }

  async updateById(id: number, member: Partial<Member>) {
    await this.httpService.put('user/' + id, member);
  }

  async getMemberPhotoById(id: number): Promise<Photo[]> {
    return await this.httpService.get<Photo[]>('user/' + id + '/photos');
  }
}
