import { Component, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { AppInputComponent } from './input.component';

@Component({
  selector: 'app-password-input',
  standalone: true,
  templateUrl: './password-input.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AppPasswordInputComponent),
      multi: true,
    },
  ],
  imports: [AppInputComponent]
})
export class AppPasswordInputComponent implements ControlValueAccessor {
  inputId = 'password';
  placeholder = 'Enter your password';
  label = 'Password';
  disabled = false;
  value: string = '';
  isVisible = false;

  onChange: (value: any) => void = () => {};
  onTouched: () => void = () => {};

  toggleVisibility() {
    this.isVisible = !this.isVisible;
  }

  writeValue(value: any): void {
    this.value = value;
  }

  registerOnChange(fn: (value: any) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  onInputChange(event: Event): void {
    this.value = (event.target as HTMLInputElement).value;
    this.onChange(this.value);
  }
}
