import { Component, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { SharedModule } from '@shared/shared.module';

@Component({
  selector: 'app-select',
  standalone: true,
  templateUrl: './select.component.html',
  providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: AppSelectComponent, multi: true }],
  imports: [SharedModule]
})
export class AppSelectComponent implements ControlValueAccessor {
  @Input() selectId = '';
  @Input() options: { value: string, label: string }[] = [];
  @Input() class: string = '';
  @Input() disabled = false;
  @Input() label = '';

  value = '';
  onChange = (_: any) => {};
  onTouched = () => {};

  writeValue(value: any): void {
    if (value !== undefined) {
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

  onValueChange(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    this.value = value;
    this.onChange(value);
    this.onTouched();
  }
}