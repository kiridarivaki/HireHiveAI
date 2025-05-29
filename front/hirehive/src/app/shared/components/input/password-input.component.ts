import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, FormControl } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-password-input',
  standalone: true,
  templateUrl: './password-input.component.html',
  styleUrl: './password-input.component.scss',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AppPasswordInputComponent),
      multi: true
    }
  ],
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
  ]
})
export class AppPasswordInputComponent implements ControlValueAccessor {
  @Input() inputId = 'password';
  @Input() type: string = 'password';
  @Input() placeholder: string = 'Enter the password';
  @Input() disabled = false;
  @Input() label: string = 'Password';
  @Input() value: string = '';
  @Input() hint: string = '';
  @Input() withToggle: boolean = false;
  @Input() isVisible: boolean = false;

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
