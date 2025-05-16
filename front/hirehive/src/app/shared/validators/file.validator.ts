import { AbstractControl, ValidationErrors } from '@angular/forms';

export function fileValidator(control: AbstractControl): ValidationErrors | null {
  const file = control.value as File | null;
  const maxSize = 5 * 1024 * 1024;
  const validTypes = ['application/pdf'];

  if (!file) return { required: true };

  const isValidType = validTypes.includes(file.type);
  const isValidSize = file.size <= maxSize;

  if (!isValidType) return { invalidType: true };
  if (!isValidSize) return { maxSizeExceeded: true };

  return null;
}
