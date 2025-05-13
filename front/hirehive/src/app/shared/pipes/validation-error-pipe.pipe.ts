import { Pipe, PipeTransform } from '@angular/core';
import { ValidationErrors } from "@angular/forms";
import { ERROR_MESSAGES } from '@shared/constants/error-messages';

@Pipe({
  name: 'validationError',
  standalone: true
})
export class ValidationErrorPipe implements PipeTransform {

  transform(errors: ValidationErrors | null | undefined): string {
    return errors ?
      Object.entries(errors)
        .map(([key, value]) =>
          typeof value === 'string' && value.length > 0 ?
            value :
            value === true && ERROR_MESSAGES[key] ?
              ERROR_MESSAGES[key] :
              ERROR_MESSAGES['unknown']
        )
        .join('. ') :
      '';
  }

}