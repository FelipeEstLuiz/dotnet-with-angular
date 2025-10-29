import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { MembersService } from '../../../core/services/members.service';
import { Member } from './../../_model/member';

@Component({
  selector: 'app-member-detail',
  imports: [TabsModule, GalleryModule],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css',
})
export class MemberDetailComponent implements OnInit {
  private memberService = inject(MembersService);
  private route = inject(ActivatedRoute);

  member?: Member;
  images: GalleryItem[] = [];

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
}
