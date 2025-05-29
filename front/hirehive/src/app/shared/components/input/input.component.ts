import { Component, Input, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-input',
  standalone: true,
  templateUrl: './input.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AppInputComponent),
      multi: true
    }
  ],
  imports: [
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    CommonModule
  ]
})
export class AppInputComponent implements ControlValueAccessor {
  @Input() inputId = '';
  @Input() type: string = 'text';
  @Input() placeholder: string = '';
  @Input() disabled = false;
  @Input() label: string = '';
  @Input() value: any = '';
  @Input() inputIcon: string = '';

  onChange = (_: any) => {};
  onTouched: () => void = () => {};

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
