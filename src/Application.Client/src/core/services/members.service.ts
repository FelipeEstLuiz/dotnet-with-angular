import { inject, Injectable, signal } from '@angular/core';
import { Member } from '../../app/_model/member';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private httpService = inject(HttpService);
  members = signal<Member[]>([]);

  async getAll(): Promise<Member[]> {
    const membersResult = await this.httpService.get<Member[]>('user');
    this.members.set(membersResult);
    return membersResult;
  }

  async getByName(username: string): Promise<Member> {
    const member = this.members().find((x) => x.name === username);
    if (member !== undefined) return member;
    return await this.httpService.get<Member>('user/' + username);
  }

  async getById(id: number): Promise<Member> {
    const member = this.members().find((x) => x.id === id);
    if (member !== undefined) return member;
    return await this.httpService.get<Member>('user/' + id);
  }

  async updateById(id: number, member: Partial<Member>) {
    await this.httpService.put('user/' + id, member);
    this.members.update((members) =>
      members.map((m) => (m.id === id ? { ...m, ...member } : m))
    );
  }
}
