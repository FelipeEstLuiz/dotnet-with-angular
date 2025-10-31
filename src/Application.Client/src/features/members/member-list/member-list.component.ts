import { Component, inject, OnInit } from '@angular/core';
import { MemberService } from '../../../core/services/member.service';
import { MemberCardComponent } from '../member-card/member-card.component';
import { Member } from '../../../types/member';

@Component({
  selector: 'app-member-list',
  imports: [MemberCardComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent implements OnInit {
  protected memberService = inject(MemberService);
  members?: Member[];

  async ngOnInit(): Promise<void> {
    await this.loadMembers();
  }

  async loadMembers() {
    this.members = await this.memberService.getAll();
  }
}
