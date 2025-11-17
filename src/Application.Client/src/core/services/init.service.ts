import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { Observable, of } from 'rxjs';
import { LikesService } from './likes.service';

@Injectable({
  providedIn: 'root',
})
export class InitService {
  private accountService = inject(AccountService);
  private likeService = inject(LikesService);

  async init(): Promise<Observable<null>> {
    const userString = localStorage.getItem('user');
    if (!userString) return of(null);
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
    await this.likeService.getLikeIds();
    return of(null);
  }
}
