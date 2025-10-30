import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../../core/services/members.service';
import { Member } from '../../../types/member';
import { MemberCardComponent } from '../member-card/member-card.component';

@Component({
  selector: 'app-member-list',
  imports: [MemberCardComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent implements OnInit {
  private memberService = inject(MembersService);
  members: Member[] = [];

  async ngOnInit(): Promise<void> {
    //if (this.memberService.members().length === 0) await this.loadMembers();
    this.members = await this.loadMembers();
  }

  async loadMembers() {
    return await this.memberService.getAll();
  }
}
