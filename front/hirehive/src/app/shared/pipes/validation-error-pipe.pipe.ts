import { Pipe, PipeTransform } from '@angular/core';
import { ValidationErrors } from "@angular/forms";
import { ERROR_MESSAGES } from '@shared/constants/error-messages';

@Pipe({
  name: 'validationError',
  standalone: true
})
export class ValidationErrorPipe implements PipeTransform {
  transform(errors: ValidationErrors | null | undefined): string[] {
    if (!errors) return [];

    return Object.entries(errors).map(([key, value]) => {
      if (typeof value === 'string' && value.length > 0) {
        return value;
      } else if (value === true && ERROR_MESSAGES[key]) {
        return ERROR_MESSAGES[key];
      } else {
        return ERROR_MESSAGES['unknown'];
      }
    });
  }

}