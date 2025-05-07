import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function fieldsMatchValidator(sourceField : string, targetField: string): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const c1 = group.get(sourceField);
    const c2 = group.get(targetField);

    if (c1?.value !== c2?.value) {
      return { fieldsMismatch: true };
    }

    return null;
  };
}