import { Component, inject, OnInit, signal } from '@angular/core';
import { MembersService } from '../../../core/services/members.service';
import {
  ActivatedRoute,
  NavigationEnd,
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet,
} from '@angular/router';
import { Member } from '../../../types/member';
import { GalleryItem, ImageItem } from 'ng-gallery';
import { filter } from 'rxjs';

@Component({
  selector: 'app-member-detailed',
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './member-detailed.component.html',
  styleUrl: './member-detailed.component.css',
})
export class MemberDetailedComponent implements OnInit {
  private memberService = inject(MembersService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  protected title = signal<string | undefined>('Profile');
  protected member = signal<Member | undefined>(undefined);
  protected images: GalleryItem[] = [];

  async ngOnInit() {
    this.route.data.subscribe({
      next: (data) => this.member.set(data['member']),
    });

    this.member()?.photo?.map((p) => {
      this.images.push(new ImageItem({ src: p.url, thumb: p.url }));
    });

    this.title.set(this.route.firstChild?.snapshot?.title);

    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe({
        next: () => {
          this.title.set(this.route.firstChild?.snapshot?.title);
        },
      });
  }

  get age(): number | null {
    if (!this.member()) return null;
    const birth = new Date(this.member()?.dateOfBirth ?? 0);
    const diff = Date.now() - birth.getTime();
    const ageDate = new Date(diff);
    return Math.abs(ageDate.getUTCFullYear() - 1970);
  }
}
