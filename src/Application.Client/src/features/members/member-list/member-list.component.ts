import { Component, inject, OnInit, signal } from '@angular/core';
import { MemberService } from '../../../core/services/member.service';
import { MemberCardComponent } from '../member-card/member-card.component';
import { Member } from '../../../types/member';
import { ApiResponse } from '../../../types/api-response';
import { PaginatorComponent } from '../../../shared/paginator/paginator.component';

@Component({
  selector: 'app-member-list',
  imports: [MemberCardComponent, PaginatorComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent implements OnInit {
  protected memberService = inject(MemberService);
  paginatedMember = signal<ApiResponse<Member[]> | null>(null);
  pageNumber = 1;
  pageSize = 5;

  async ngOnInit(): Promise<void> {
    await this.loadMembers();
  }

  async loadMembers() {
    const member = await this.memberService.getAll(
      this.pageNumber,
      this.pageSize
    );

    if (member) this.paginatedMember.set(member);
  }

  async onPageChange(event: { pageNumber: number; pageSize: number }) {
    this.pageNumber = event.pageNumber;
    this.pageSize = event.pageSize;
    await this.loadMembers();
  }
}
