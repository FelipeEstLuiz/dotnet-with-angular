import { DatePipe, NgClass } from '@angular/common';
import {
  Component,
  HostListener,
  inject,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { AccountService } from '../../../core/services/account.service';
import { MemberService } from '../../../core/services/member.service';
import { ToastService } from '../../../core/services/toast.service';
import { Member } from '../../../types/member';
import { MemberUpdate } from '../../../types/member-update';
import { TimeAgoPipe } from '../../../core/pipes/time-ago.pipe';

@Component({
  selector: 'app-members-profile',
  imports: [DatePipe, FormsModule, NgClass, TimeAgoPipe],
  templateUrl: './members-profile.component.html',
  styleUrl: './members-profile.component.css',
})
export class MembersProfileComponent implements OnInit, OnDestroy {
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify(
    $event: BeforeUnloadEvent
  ) {
    if (this.editForm?.dirty) $event.preventDefault();
  }
  protected memberService = inject(MemberService);
  private toastService = inject(ToastService);
  private accountService = inject(AccountService);

  protected editableMember: MemberUpdate = {
    name: '',
    city: '',
    country: '',
    interests: '',
    introduction: '',
    lookingFor: '',
  };

  ngOnInit(): void {
    this.editableMember = {
      name: this.memberService.member()?.name || '',
      city: this.memberService.member()?.city || '',
      country: this.memberService.member()?.country || '',
      interests: this.memberService.member()?.interests,
      introduction: this.memberService.member()?.introduction,
      lookingFor: this.memberService.member()?.lookingFor,
    };
  }

  resetForm(form: NgForm) {
    if (!this.memberService.member()) return;
    form.resetForm({
      name: this.memberService.member()?.name || '',
      city: this.memberService.member()?.city || '',
      country: this.memberService.member()?.country || '',
      interests: this.memberService.member()?.interests || '',
      introduction: this.memberService.member()?.introduction || '',
      lookingFor: this.memberService.member()?.lookingFor || '',
    });
  }

  async updateProfile() {
    if (!this.memberService.member() || !this.editForm?.valid) {
      this.toastService.error('Verifique os campos obrigatórios.');
      return;
    }

    const updateMember = {
      ...this.memberService.member(),
      ...this.editableMember,
    };
    await this.memberService.updateById(updateMember.id!, this.editableMember);
    const currentUser = this.accountService.currentUser();

    if (currentUser && updateMember.name !== currentUser?.name) {
      currentUser.name = updateMember.name;
      this.accountService.setCurrentUser(currentUser);
    }

    this.toastService.success('Profile updated successfully');
    this.memberService.disableEditMode();
    this.memberService.member.set(updateMember as Member);
    this.editForm.resetForm(updateMember);
  }

  ngOnDestroy() {
    if (this.memberService.editMode()) this.memberService.disableEditMode();
  }
}
