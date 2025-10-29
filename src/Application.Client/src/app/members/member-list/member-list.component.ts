import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../../core/services/members.service';
import { MemberCardComponent } from '../member-card/member-card.component';

@Component({
  selector: 'app-member-list',
  imports: [MemberCardComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent implements OnInit {
  memberService = inject(MembersService);
  async ngOnInit(): Promise<void> {
    if (this.memberService.members().length === 0) await this.loadMembers();
  }

  async loadMembers() {
    await this.memberService.getAll();
  }
}
