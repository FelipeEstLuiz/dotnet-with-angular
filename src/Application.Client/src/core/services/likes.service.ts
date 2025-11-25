import { inject, Injectable, signal } from '@angular/core';
import { HttpService } from './http.service';
import { Member } from '../../types/member';
import { HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class LikesService {
  private httpService = inject(HttpService);
  likeIds = signal<string[]>([]);

  async toggleLike(targetUserId: string) {
    return await this.httpService.post<string[]>('like/' + targetUserId, {});
  }

  async getLikes(predicate: string, pageNumber: number, pageSize: number) {
    let params = new HttpParams()
      .append('pageNumber', pageNumber)
      .append('pageSize', pageSize)
      .append('predicate', predicate);

    return await this.httpService.getApiResult<Member[]>('like', params);
  }

  async getLikeIds() {
    const ids = await this.httpService.get<string[]>('like/list');
    this.likeIds.set(ids);
    return ids;
  }

  clearLikeIds() {
    this.likeIds.set([]);
  }
}
