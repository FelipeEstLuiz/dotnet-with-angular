import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../../core/services/members.service';
import { ActivatedRoute, RouterLink, RouterLinkActive } from '@angular/router';
import { Member } from '../../../types/member';
import { GalleryItem, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-member-detailed',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './member-detailed.component.html',
  styleUrl: './member-detailed.component.css',
})
export class MemberDetailedComponent implements OnInit {
  private memberService = inject(MembersService);
  private route = inject(ActivatedRoute);
  protected member?: Member;
  protected images: GalleryItem[] = [];

  async ngOnInit() {
    await this.loadMember();
  }

  async loadMember() {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.member = await this.memberService.getById(parseInt(id));

    this.member?.photo?.map((p) => {
      this.images.push(new ImageItem({ src: p.url, thumb: p.url }));
    });
  }

  get age(): number | null {
    if (!this.member) return null;
    const birth = new Date(this.member.dateOfBirth);
    const diff = Date.now() - birth.getTime();
    const ageDate = new Date(diff);
    return Math.abs(ageDate.getUTCFullYear() - 1970);
  }
}
