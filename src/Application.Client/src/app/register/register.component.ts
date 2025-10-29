import { Component, inject, input, output } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { getPasswordStrength } from '../validators/password-strength.validator';
import { CommonModule } from '@angular/common';
import { AlertService } from '../../core/services/alert.service';
import { AccountService } from '../../core/services/account.service';
import { UserRegister } from '../../types/user-register';

@Component({
  selector: 'app-register',
  imports: [FormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  private alertService = inject(AlertService);
  cancelRegister = output<boolean>();
  model: UserRegister = {
    name: '',
    email: '',
    password: '',
    passwordConfirmed: '',
  };

  passwordStrength: string = '';

  onPasswordChange(): void {
    this.passwordStrength = getPasswordStrength(this.model.password);
  }

  register(form: NgForm) {
    if (!form.valid || this.model.password !== this.model.passwordConfirmed) {
      form.control.markAllAsTouched();
      return;
    }

    if (this.passwordStrength === 'Weak') {
      this.alertService.error('The password is too weak!');
      return;
    }

    this.accountService.register(this.model).subscribe({
      next: (response) => {
        console.log(response);
        this.cancel();
      },
      error: (error) => this.alertService.error(error),
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
