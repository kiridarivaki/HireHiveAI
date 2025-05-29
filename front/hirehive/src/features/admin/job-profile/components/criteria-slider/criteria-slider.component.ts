import { Component, Input, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSliderModule } from '@angular/material/slider';
import { ReactiveFormsModule, ControlValueAccessor, NG_VALUE_ACCESSOR, FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-criteria-slider',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatSliderModule, FormsModule, MatIconModule],
  templateUrl:'./criteria-slider.component.html',
  styleUrl: './criteria-slider.component.css',
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => CriteriaSliderComponent),
    multi: true,
  }]
})
export class CriteriaSliderComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() icon = '';

  selectedValue = 50;
  disabled = false;

  private onChange = (value: any) => {};
  private onTouched = () => {};

  writeValue(value: any): void {
    if (value !== undefined && value !== null) {
      this.selectedValue = value;
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  onInput(event: any) {
    this.selectedValue = Number((event.target as HTMLInputElement).value);
    this.onChange(this.selectedValue);
  }

  onBlur() {
    this.onTouched();
  }
}