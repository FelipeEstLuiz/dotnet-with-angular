import { Component, inject, OnInit, signal } from '@angular/core';
import { MemberService } from '../../../core/services/member.service';
import { ActivatedRoute } from '@angular/router';
import { Photo } from '../../../types/photo';
import { ImageUploadComponent } from '../../../shared/image-upload/image-upload.component';

@Component({
  selector: 'app-member-photos',
  imports: [ImageUploadComponent],
  templateUrl: './member-photos.component.html',
  styleUrl: './member-photos.component.css',
})
export class MemberPhotosComponent implements OnInit {
  protected memberService = inject(MemberService);
  private route = inject(ActivatedRoute);
  protected photos = signal<Photo[]>([]);
  protected loading = signal<boolean>(false);

  async ngOnInit() {
    const memberId = this.route.parent?.snapshot.paramMap.get('id');
    if (memberId)
      this.photos.set(
        await this.memberService.getMemberPhotoById(parseInt(memberId))
      );
  }

  async uploadImage(file: File) {
    try {
      this.loading.set(true);
      const photo = await this.memberService.uploadFile(file);
      this.photos.update((photos) => [...photos, photo]);
      this.memberService.disableEditMode();
    } finally {
      this.loading.set(false);
    }
  }
}
