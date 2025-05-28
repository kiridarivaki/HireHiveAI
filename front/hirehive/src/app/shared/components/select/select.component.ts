import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-select',
  standalone: true,
  templateUrl: './select.component.html',
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: AppSelectComponent, multi: true }],
  imports: [MatSelectModule, MatFormFieldModule, CommonModule]
})
export class AppSelectComponent implements ControlValueAccessor, OnInit {
  @Input() selectId = '';
  @Input() options: { value: string, label: string }[] = [];
  @Input() class: string = '';
  @Input() disabled = false;
  @Input() label = '';

  value: string = '';
  private _hasValueSet = false;

  onChange = (_: any) => {};
  onTouched = () => {};

    writeValue(value: any): void {
      if (value != null) {
        this.value = value.toString();
        this._hasValueSet = true;
      } else {
        this.value = '';
        this._hasValueSet = false;
      }
    }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  ngOnInit(): void {
    if (this._hasValueSet && this.options.length > 0) {
      const match = this.options.find(opt => opt.value === this.value);
    }
  }

  onValueChange(value: any): void {
    this.value = value;
    this.onChange(value);
  }
}
