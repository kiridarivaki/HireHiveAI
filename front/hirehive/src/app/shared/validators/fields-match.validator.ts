import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function fieldsMatchValidator(sourceField: string, targetField: string): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const source = group.get(sourceField);
    const target = group.get(targetField);

    if (!source || !target) return null;

    const error = source.value !== target.value;

    if (error) {
      target.setErrors({ ...target.errors, fieldsMismatch: true });
    } else {
      if (target.hasError('fieldsMismatch')) {
        const newErrors = { ...target.errors };
        delete newErrors['fieldsMismatch'];
        if (Object.keys(newErrors).length === 0) {
          target.setErrors(null);
        } else {
          target.setErrors(newErrors);
        }
      }
    }

    return null;
  };
}
