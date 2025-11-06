import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const uppercaseValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const value = control.value;
  if (!value) return null;
  return !/[A-Z]/.test(value) ? { uppercase: true } : null;
};

export const lowercaseValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const value = control.value;
  if (!value) return null;
  return !/[a-z]/.test(value) ? { lowercase: true } : null;
};

export const leastOneNumberValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const value = control.value;
  if (!value) return null;
  return !/\d/.test(value) ? { leastOneNumber: true } : null;
};

export const leastOneSpecialCharacterValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const value = control.value;
  if (!value) return null;
  return !/[@#$%^&+=!]/.test(value) ? { leastOneSpecialCharacter: true } : null;
};

export const passwordStrengthValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const password = control.value;
  if (!password) return null;
  const hasLower = /[a-z]/.test(password);
  const hasUpper = /[A-Z]/.test(password);
  const hasNumber = /[0-9]/.test(password);
  const hasSymbol = /[\W_]/.test(password);

  const strengthCount = [hasLower, hasUpper, hasNumber, hasSymbol].filter(
    Boolean
  ).length;

  if (password.length < 6 || strengthCount < 2) return { weak: true };
  if (password.length >= 6 && strengthCount === 3) return { average: true };
  if (password.length >= 8 && strengthCount === 4) return null;

  return { weak: true };
};
