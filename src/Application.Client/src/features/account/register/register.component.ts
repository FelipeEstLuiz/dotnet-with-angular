import { Component, inject, output } from '@angular/core';
import { UserRegister } from '../../../types/user-register';
import { AccountService } from '../../../core/services/account.service';
import { AlertService } from '../../../core/services/alert.service';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  imports: [FormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  protected model: UserRegister = {
    name: '',
    email: '',
    password: '',
    passwordConfirmed: '',
    knowAs: '',
    city: '',
    country: '',
    gender: '',
  };
  private accountService = inject(AccountService);
  private alertService = inject(AlertService);
  cancelRegister = output<boolean>();
  passwordStrength: string = '';

  register(form: NgForm) {
    if (!form.valid || this.model.password !== this.model.passwordConfirmed) {
      form.control.markAllAsTouched();
      return;
    }

    if (this.model.password !== this.model.passwordConfirmed) {
      this.alertService.warning('Password and confirmation must match!');
      return;
    }

    if (this.passwordStrength === 'Weak') {
      this.alertService.error('The password is too weak!');
      return;
    }

    this.accountService.register(this.model).subscribe({
      next: (_) => this.cancel(),
      error: (error) => this.alertService.error(error),
    });
  }

  onPasswordChange(): void {
    this.passwordStrength = this.getPasswordStrength(this.model.password);
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

  getPasswordStrength(password: string): 'Weak' | 'Average' | 'Strong' {
    const hasLower = /[a-z]/.test(password);
    const hasUpper = /[A-Z]/.test(password);
    const hasNumber = /[0-9]/.test(password);
    const hasSymbol = /[\W_]/.test(password);

    const strengthCount = [hasLower, hasUpper, hasNumber, hasSymbol].filter(
      Boolean
    ).length;

    if (password.length < 6 || strengthCount < 2) return 'Weak';
    if (password.length >= 6 && strengthCount === 3) return 'Average';
    if (password.length >= 8 && strengthCount === 4) return 'Strong';

    return 'Weak';
  }
}
