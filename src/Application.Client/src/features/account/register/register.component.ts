import { Component, inject, OnInit, output } from '@angular/core';
import { UserRegister } from '../../../types/user-register';
import { AccountService } from '../../../core/services/account.service';
import { AlertService } from '../../../core/services/alert.service';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  FormsModule,
  NgForm,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { CommonModule, JsonPipe } from '@angular/common';
import { TextInputComponent } from '../../../shared/text-input/text-input.component';
import {
  leastOneNumberValidator,
  leastOneSpecialCharacterValidator,
  lowercaseValidator,
  passwordStrengthValidator,
  uppercaseValidator,
} from '../../../core/validators/validator-input.validator';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, CommonModule, JsonPipe, TextInputComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
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

  protected registerForm: FormGroup = new FormGroup({});

  ngOnInit(): void {
    this.InitializaForm();
  }

  InitializaForm() {
    this.registerForm = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.maxLength(100),
        Validators.minLength(3),
      ]),
      email: new FormControl('', [Validators.email, Validators.required]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(8),
        uppercaseValidator,
        lowercaseValidator,
        leastOneNumberValidator,
        leastOneSpecialCharacterValidator,
        passwordStrengthValidator,
      ]),
      passwordConfirmed: new FormControl('', [
        Validators.required,
        this.matchValues('password'),
      ]),
      city: new FormControl('', [
        Validators.required,
        Validators.maxLength(100),
        Validators.minLength(3),
      ]),
      country: new FormControl('', [
        Validators.required,
        Validators.maxLength(50),
        Validators.minLength(3),
      ]),
    });
    this.registerForm.controls['password'].valueChanges.subscribe(() => {
      this.registerForm.controls['passwordConfirmed'].updateValueAndValidity();
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const parent = control.parent;

      if (!parent) return null;

      const matchValue = parent.get(matchTo)?.value;
      return control.value === matchValue ? null : { passworMismatch: true };
    };
  }

  async register() {
    console.log(this.registerForm.value);
    // if (!form.valid || this.model.password !== this.model.passwordConfirmed) {
    //   form.control.markAllAsTouched();
    //   return;
    // }
    // if (this.model.password !== this.model.passwordConfirmed) {
    //   this.alertService.warning('Password and confirmation must match!');
    //   return;
    // }
    // if (this.passwordStrength === 'Weak') {
    //   this.alertService.error('The password is too weak!');
    //   return;
    // }
    // await this.accountService.register(this.model);
    // this.cancel();
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
