import { inject, Injectable } from '@angular/core';
import { Member } from '../_model/member';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  private httpService = inject(HttpService);

  getMembers() {
    return this.httpService.get<Member[]>('user');
  }

  getMember(username: string) {
    return this.httpService.get<Member>('user/' + username);
  }
}
