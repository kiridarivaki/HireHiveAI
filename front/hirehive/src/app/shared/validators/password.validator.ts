import { AbstractControl, ValidationErrors } from '@angular/forms';

export function passwordValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value || '';

  const hasUppercase = /[A-Z]/.test(value);
  const hasLowercase = /[a-z]/.test(value);
  const hasDigit = /\d/.test(value);
  const isLongEnough = value.length >= 8;

  const errors: ValidationErrors = {};

  if (!hasUppercase) {
    errors['uppercase'] = true;
  }
  if (!hasLowercase) {
    errors['lowercase'] = true;
  }
  if (!hasDigit) {
    errors['digit'] = true;
  }
  if (!isLongEnough) {
    errors['minlength'] = { requiredLength: 8, actualLength: value.length };
  }

  return Object.keys(errors).length ? errors : null;
}