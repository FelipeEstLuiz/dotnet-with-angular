import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../../core/services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Photo } from '../../../types/photo';

@Component({
  selector: 'app-member-photos',
  imports: [],
  templateUrl: './member-photos.component.html',
  styleUrl: './member-photos.component.css',
})
export class MemberPhotosComponent implements OnInit {
  private memberService = inject(MembersService);
  private route = inject(ActivatedRoute);
  protected photos?: Photo[];

  async ngOnInit() {
    const memberId = this.route.parent?.snapshot.paramMap.get('id');
    if (memberId)
      this.photos = await this.memberService.getMemberPhotoById(
        parseInt(memberId)
      );
  }
}
