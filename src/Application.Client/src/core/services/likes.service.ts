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
    const response = await this.httpService.post<string[]>(
      'like/' + targetUserId,
      {}
    );

    if (this.likeIds().includes(targetUserId)) {
      this.likeIds.update((ids) => ids.filter((x) => x !== targetUserId));
    } else {
      this.likeIds.update((ids) => [...ids, targetUserId]);
    }

    return response;
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
