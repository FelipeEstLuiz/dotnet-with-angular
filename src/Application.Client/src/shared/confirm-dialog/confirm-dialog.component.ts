import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { ConfirmDialogService } from 'src/core/services/confirm-dialog.service';

@Component({
  selector: 'app-confirm-dialog',
  imports: [],
  templateUrl: './confirm-dialog.component.html',
  styleUrl: './confirm-dialog.component.css',
})
export class ConfirmDialogComponent {
  @ViewChild('dialogRef') dialogRef!: ElementRef<HTMLDialogElement>;
  message = '';
  btnOkText = '';
  btnCancelText = '';

  private resolver: ((result: boolean) => void) | null = null;

  constructor() {
    inject(ConfirmDialogService).register(this);
  }

  open(
    message: string,
    btnOkText = 'Ok',
    btnCancelText = 'Cancel'
  ): Promise<boolean> {
    this.message = message;
    this.btnOkText = btnOkText;
    this.btnCancelText = btnCancelText;
    this.dialogRef.nativeElement.showModal();
    return new Promise((resolve) => (this.resolver = resolve));
  }

  confirm() {
    this.dialogRef.nativeElement.close();
    this.resolver?.(true);
    this.resolver = null;
  }

  cancel() {
    this.dialogRef.nativeElement.close();
    this.resolver?.(false);
    this.resolver = null;
  }
}
