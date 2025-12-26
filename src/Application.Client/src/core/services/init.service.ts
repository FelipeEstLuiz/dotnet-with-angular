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
    const user = await this.accountService.refreshToken();

    if (user) {
      this.accountService.setCurrentUser(user);
      await this.likeService.getLikeIds();
      await this.accountService.startTokenRefreshInterval();
    }
    return of(null);
  }
}
