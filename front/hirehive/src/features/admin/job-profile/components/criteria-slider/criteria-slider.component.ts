import { Component, Input, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSliderModule } from '@angular/material/slider';
import { ReactiveFormsModule, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-criteria-slider',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatSliderModule],
  templateUrl:'./criteria-slider.component.html',
  styles: [`
    :host {
      display: block;
      margin-bottom: 1rem;
    }
    label {
      font-weight: 600;
      margin-bottom: 0.25rem;
      display: block;
    }
  `],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => CriteriaSliderComponent),
    multi: true,
  }]
})
export class CriteriaSliderComponent implements ControlValueAccessor {
  @Input() label = '';

  value = 50;
  disabled = false;

  private onChange = (value: any) => {};
  private onTouched = () => {};

  writeValue(value: any): void {
    if (value !== undefined && value !== null) {
      this.value = value;
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  onBlur() {
    this.onTouched();
  }
}