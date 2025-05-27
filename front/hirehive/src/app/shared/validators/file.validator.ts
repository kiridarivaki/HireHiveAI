import { AbstractControl, ValidationErrors } from "@angular/forms";

export function fileValidator(control: AbstractControl): ValidationErrors | null {
  const file = control.value as File | null;

  if (!file) {
    console.log('No file provided');
    return { required: true };
  }

  const maxSize = 5 * 1024 * 1024;
  const validTypes = ['application/pdf'];

  const isValidType = validTypes.includes(file.type);
  const isValidSize = file.size <= maxSize;

  if (!isValidType) {
    return { invalidType: true };
  }

  if (!isValidSize) {
    return { maxSizeExceeded: true };
  }

  return null;
}
