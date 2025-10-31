import { DatePipe, NgClass } from '@angular/common';
import {
  Component,
  HostListener,
  inject,
  OnDestroy,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MemberService } from '../../../core/services/member.service';
import { ToastService } from '../../../core/services/toast.service';
import { Member } from '../../../types/member';
import { MemberUpdate } from '../../../types/member-update';

@Component({
  selector: 'app-members-profile',
  imports: [DatePipe, FormsModule, NgClass],
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
  private route = inject(ActivatedRoute);
  protected memberService = inject(MemberService);
  private toastService = inject(ToastService);

  protected member = signal<Member | undefined>(undefined);
  protected editableMember: MemberUpdate = {
    name: '',
    city: '',
    country: '',
    interests: '',
    introduction: '',
    lookingFor: '',
  };

  ngOnInit(): void {
    this.route.parent?.data.subscribe({
      next: (data) => this.member.set(data['member']),
    });

    this.editableMember = {
      name: this.member()?.name || '',
      city: this.member()?.city || '',
      country: this.member()?.country || '',
      interests: this.member()?.interests,
      introduction: this.member()?.introduction,
      lookingFor: this.member()?.lookingFor,
    };
  }

  resetForm(form: NgForm) {
    if (!this.member()) return;
    form.resetForm({
      name: this.member()?.name || '',
      city: this.member()?.city || '',
      country: this.member()?.country || '',
      interests: this.member()?.interests || '',
      introduction: this.member()?.introduction || '',
      lookingFor: this.member()?.lookingFor || '',
    });
  }

  async updateProfile() {
    if (!this.member() || !this.editForm?.valid) {
      this.toastService.error('Verifique os campos obrigatórios.');
      return;
    }

    const updateMember = { ...this.member(), ...this.editableMember };
    await this.memberService.updateById(updateMember.id!, updateMember);
    this.toastService.success('Profile updated successfully');
    this.memberService.editMode.set(false);
  }

  ngOnDestroy() {
    if (this.memberService.editMode()) this.memberService.editMode.set(false);
  }
}
