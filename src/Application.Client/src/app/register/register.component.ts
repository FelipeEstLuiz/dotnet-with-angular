import { Component, inject, input, output } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { UserRegister } from '../_model/user-register';
import { getPasswordStrength } from '../validators/password-strength.validator';
import { CommonModule } from '@angular/common';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  imports: [FormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  cancelRegister = output<boolean>();

  model: UserRegister = {
    nome: '',
    email: '',
    senha: '',
    senhaconfirmacao: '',
  };

  passwordStrength: string = '';

  onPasswordChange(): void {
    this.passwordStrength = getPasswordStrength(this.model.senha);
  }

  register(form: NgForm) {
    if (!form.valid || this.model.senha !== this.model.senhaconfirmacao) {
      return;
    }

    if (this.passwordStrength === 'Weak') {
      alert('The password is too weak!');
      return;
    }

    this.accountService.register(this.model).subscribe({
      next: response =>{
        console.log(response);
        this.cancel();
      },
      error: error => console.log(error)
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
