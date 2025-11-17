import { Component, inject, OnInit, signal } from '@angular/core';
import { LikesService } from '../../core/services/likes.service';
import { Member } from '../../types/member';
import { RouterLink } from '@angular/router';
import { MemberCardComponent } from '../members/member-card/member-card.component';
import { ApiResponse } from '../../types/api-response';
import { PaginatorComponent } from '../../shared/paginator/paginator.component';

@Component({
  selector: 'app-lists',
  imports: [MemberCardComponent, PaginatorComponent],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css',
})
export class ListsComponent implements OnInit {
  private likeService = inject(LikesService);
  protected paginatedMember = signal<ApiResponse<Member[]> | null>(null);
  protected predicate = 'liked';
  protected pageNumber = 1;
  protected pageSize = 5;

  tabs = [
    { label: 'Liked', value: 'liked' },
    { label: 'Liked me', value: 'likedBy' },
    { label: 'Mutual', value: 'mutual' },
  ];

  async ngOnInit() {
    await this.loadLikes();
  }

  async loadLikes() {
    const members = await this.likeService.getLikes(
      this.predicate,
      this.pageNumber,
      this.pageSize
    );
    if (members) this.paginatedMember.set(members);
  }

  async setPredicate(predicate: string) {
    if (this.predicate != predicate) {
      this.predicate = predicate;
      this.pageNumber = 1;
      await this.loadLikes();
    }
  }

  async onPageChange(event: { pageNumber: number; pageSize: number }) {
    this.pageNumber = event.pageNumber;
    this.pageSize = event.pageSize;
    await this.loadLikes();
  }
}
