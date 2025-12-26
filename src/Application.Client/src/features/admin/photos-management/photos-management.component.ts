import { Component, inject, OnInit, signal } from '@angular/core';
import { AdminService } from 'src/core/services/admin.service';
import { Photo } from 'src/types/photo';

@Component({
  selector: 'app-photos-management',
  imports: [],
  templateUrl: './photos-management.component.html',
  styleUrl: './photos-management.component.css',
})
export class PhotosManagementComponent implements OnInit {
  private adminService = inject(AdminService);
  photos = signal<Photo[]>([]);

  async ngOnInit() {
    await this.getPhotosForApproval();
  }

  async getPhotosForApproval() {
    const photos = await this.adminService.getPhotosForApproval();
    if (photos) this.photos.set(photos);
  }

  async approvePhoto(photoId: number) {
    await this.adminService.approvePhoto(photoId);
    this.photos.update((photos) => {
      return photos.filter((x) => x.id !== photoId);
    });
  }

  async rejectPhoto(photoId: number) {
    await this.adminService.rejectPhoto(photoId);
    this.photos.update((photos) => {
      return photos.filter((x) => x.id !== photoId);
    });
  }
}
