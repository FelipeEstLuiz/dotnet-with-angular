import { Component, ElementRef, output, ViewChild } from '@angular/core';
import { MemberParams } from '../../../types/member-params';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filter-modal',
  imports: [FormsModule],
  templateUrl: './filter-modal.component.html',
  styleUrl: './filter-modal.component.css',
})
export class FilterModalComponent {
  @ViewChild('filterModel') modalRef!: ElementRef<HTMLDialogElement>;
  closeModal = output();
  submitData = output<MemberParams>();
  memberParams = new MemberParams();

  open() {
    this.modalRef.nativeElement.showModal();
  }

  close() {
    this.modalRef.nativeElement.close();
    this.closeModal.emit();
  }

  submit() {
    this.submitData.emit(this.memberParams);
    this.close();
  }

  onMinAgeChange() {
    if (this.memberParams.minAge < 18) this.memberParams.minAge = 18;
  }

  onMaxAgeChange() {
    if (this.memberParams.maxAge < this.memberParams.minAge)
      this.memberParams.maxAge = this.memberParams.minAge;
  }
}
