import { Component, inject, OnInit, output, signal } from '@angular/core';
import { UserRegister } from '../../../types/user-register';
import { AccountService } from '../../../core/services/account.service';
import { AlertService } from '../../../core/services/alert.service';
import {
  AbstractControl,
  FormBuilder,
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
import { TextareaInputComponent } from '../../../shared/textarea-input/textarea-input.component';

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    JsonPipe,
    TextInputComponent,
    TextareaInputComponent,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  private accountService = inject(AccountService);
  private alertService = inject(AlertService);
  cancelRegister = output<boolean>();

  private fb = inject(FormBuilder);
  protected credentialsForm: FormGroup;
  protected profileForm: FormGroup;
  protected aboutForm: FormGroup;
  protected currentStep = signal(1);

  constructor() {
    this.credentialsForm = this.fb.group({
      name: [
        '',
        [
          Validators.required,
          Validators.maxLength(100),
          Validators.minLength(3),
        ],
      ],
      email: ['', [Validators.email, Validators.required]],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          uppercaseValidator,
          lowercaseValidator,
          leastOneNumberValidator,
          leastOneSpecialCharacterValidator,
          passwordStrengthValidator,
        ],
      ],
      passwordConfirmed: [
        '',
        [Validators.required, this.matchValues('password')],
      ],
    });
    this.credentialsForm.controls['password'].valueChanges.subscribe(() => {
      this.credentialsForm.controls[
        'passwordConfirmed'
      ].updateValueAndValidity();
    });

    this.profileForm = this.fb.group({
      gender: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: [
        '',
        [
          Validators.required,
          Validators.maxLength(100),
          Validators.minLength(3),
        ],
      ],
      country: [
        '',
        [
          Validators.required,
          Validators.maxLength(50),
          Validators.minLength(3),
        ],
      ],
    });

    this.aboutForm = this.fb.group({
      knowAs: [
        '',
        [
          Validators.required,
          Validators.maxLength(100),
          Validators.minLength(3),
        ],
      ],
      interests: ['', Validators.maxLength(1000)],
      lookingFor: ['', Validators.maxLength(1000)],
      introduction: ['', Validators.maxLength(2000)],
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

  nextStep() {
    if (this.credentialsForm.valid) {
      this.currentStep.update((prevStep) => prevStep + 1);
    }
  }

  prevStep() {
    this.currentStep.update((prevStep) => prevStep - 1);
  }

  getMaxDate() {
    const today = new Date();
    today.setFullYear(today.getFullYear() - 18);
    return today.toISOString().split('T')[0];
  }

  async register() {
    if (this.profileForm.valid && this.credentialsForm.valid) {
      const formData = {
        ...this.profileForm.value,
        ...this.credentialsForm.value,
        ...this.aboutForm.value,
      };

      console.log('formData', formData);
    }

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
