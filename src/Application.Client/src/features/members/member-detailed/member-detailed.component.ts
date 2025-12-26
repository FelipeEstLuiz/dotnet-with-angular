import { Location } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import {
  ActivatedRoute,
  NavigationEnd,
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet,
} from '@angular/router';
import { filter } from 'rxjs';
import { AgePipe } from '../../../core/pipes/age.pipe';
import { AccountService } from '../../../core/services/account.service';
import { MemberService } from '../../../core/services/member.service';
import { PresenceService } from 'src/core/services/presence.service';
import { LikesService } from 'src/core/services/likes.service';

@Component({
  selector: 'app-member-detailed',
  imports: [RouterLink, RouterLinkActive, RouterOutlet, AgePipe],
  templateUrl: './member-detailed.component.html',
  styleUrl: './member-detailed.component.css',
})
export class MemberDetailedComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private location = inject(Location);
  private accountService = inject(AccountService);
  protected memberService = inject(MemberService);
  protected presenceService = inject(PresenceService);
  protected likeService = inject(LikesService);

  private routeId = signal<string | null>(null);
  protected title = signal<string | undefined>('Profile');
  protected isCurrentUser = computed(() => {
    return `${this.accountService.currentUser()?.id}` === this.routeId();
  });
  protected hasLiked = computed(() =>
    this.likeService.likeIds().includes(this.routeId()!)
  );

  constructor() {
    this.route.paramMap.subscribe((params) => {
      this.routeId.set(params.get('id'));
    });
  }

  async ngOnInit() {
    this.title.set(this.route.firstChild?.snapshot?.title);

    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe({
        next: () => {
          this.title.set(this.route.firstChild?.snapshot?.title);
        },
      });
  }

  goBack() {
    this.location.back();
  }
}
