import { Component, inject, OnInit, signal } from '@angular/core';
import { MemberService } from '../../../core/services/member.service';
import { ActivatedRoute } from '@angular/router';
import { Photo } from '../../../types/photo';
import { ImageUploadComponent } from '../../../shared/image-upload/image-upload.component';
import { AccountService } from '../../../core/services/account.service';
import { Member } from '../../../types/member';
import { StarButtonComponent } from '../../../shared/star-button/star-button.component';

@Component({
  selector: 'app-member-photos',
  imports: [ImageUploadComponent, StarButtonComponent],
  templateUrl: './member-photos.component.html',
  styleUrl: './member-photos.component.css',
})
export class MemberPhotosComponent implements OnInit {
  protected memberService = inject(MemberService);
  private accountService = inject(AccountService);
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

  async setMainPhoto(photo: Photo) {
    await this.memberService.setMainPhoto(photo);
    const currentUser = this.accountService.currentUser();

    if (currentUser) {
      currentUser.imageUrl = photo.url;
      this.accountService.setCurrentUser(currentUser);
      this.memberService.member.update(
        (member) =>
          ({
            ...member,
            photoUrl: photo.url,
          } as Member)
      );
    }
  }
}
