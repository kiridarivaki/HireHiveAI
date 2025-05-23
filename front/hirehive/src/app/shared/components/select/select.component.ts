import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-select',
  standalone: true,
  templateUrl: './select.component.html',
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: AppSelectComponent, multi: true }],
  imports: [
    MatSelectModule, 
    MatFormFieldModule,
    CommonModule
  ]
})
export class AppSelectComponent implements ControlValueAccessor {
  @Input() selectId = '';
  @Input() options: { value: string, label: string }[] = [];
  @Input() class: string = '';
  @Input() disabled = false;
  @Input() label = '';
  value: any = '';

  onChange = (_: any) => {};
  onTouched = () => {};

  writeValue(value: any): void {
      this.value = value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  onValueChange(value: string | number): void {
    const numericValue = typeof value === 'string' ? parseInt(value, 10) : value;
    this.value = numericValue;
    this.onChange(numericValue);
    this.onTouched();
  }
}