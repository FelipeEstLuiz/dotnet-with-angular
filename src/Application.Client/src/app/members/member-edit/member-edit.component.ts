import {
  Component,
  HostListener,
  inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { MembersService } from '../../../core/services/members.service';
import { Member } from './../../_model/member';
import { FormsModule, NgForm } from '@angular/forms';
import { AlertService } from '../../../core/services/alert.service';
import { AccountService } from '../../../core/services/account.service';

@Component({
  selector: 'app-members-edit',
  imports: [TabsModule, FormsModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css',
})
export class MembersEditComponent implements OnInit {
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  member?: Member;

  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  private alertService = inject(AlertService);

  async ngOnInit(): Promise<void> {
    await this.loadMember();
  }

  async loadMember(): Promise<void> {
    const user = this.accountService.currentUser();

    if (!user) return;

    this.member = await this.memberService.getById(user.id);
  }

  async updateMember() {
    if (this.member) {
      await this.memberService.updateById(
        this.member?.id,
        this.editForm?.value
      );
      this.alertService.success('Profile updated successfully');
      this.editForm?.reset(this.member);
    }
  }
}
