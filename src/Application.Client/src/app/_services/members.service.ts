import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../_model/api-response';
import { Member } from '../_model/member';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  baseUrl = environment.apiUrl;

  getMembers() {
    return this.http.get<ApiResponse<Member[]>>(this.baseUrl + 'v1/user', this.getHttpOptions());
  }

  getMember(username: string) {
    return this.http.get<ApiResponse<Member>>(this.baseUrl + 'v1/user/' + username, this.getHttpOptions());
  }

  getHttpOptions() {
    const user = this.accountService.currentUser();
    if (user) {
      return { headers: new HttpHeaders({ 'Authorization': `Bearer ${user.token}` }) };
    }
    return {};
  }
}
