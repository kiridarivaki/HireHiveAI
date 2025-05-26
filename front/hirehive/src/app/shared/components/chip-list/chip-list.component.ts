import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
  OnInit,
  forwardRef,
  inject,
} from '@angular/core';
import {
  FormControl,
  ControlValueAccessor,
  NG_VALUE_ACCESSOR,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  MatAutocompleteSelectedEvent,
  MatAutocompleteModule,
} from '@angular/material/autocomplete';
import {
  MatChipsModule,
  MatChipInputEvent,
} from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { LiveAnnouncer } from '@angular/cdk/a11y';

@Component({
  selector: 'app-chip-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatChipsModule,
    MatIconModule,
    MatAutocompleteModule,
  ],
  templateUrl: './chip-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AppChipListComponent),
      multi: true,
    },
  ],
})
export class AppChipListComponent implements ControlValueAccessor, OnInit {
  @Input() label = 'Select Items';
  @Input() disabled = false;
  @Input() options: { value: string; label: string }[] = [];

  @Output() selectionChange = new EventEmitter<string[]>();

  readonly separatorKeysCodes = [ENTER, COMMA];
  inputControl = new FormControl('');

  private announcer = inject(LiveAnnouncer);
  private _value: string[] = [];
  private hasInitialValue = false;

  private onChange = (_: string[]) => {};
  private onTouched = () => {};

  ngOnInit() {
    // Only emit initial value if one was provided by parent
    if (this.hasInitialValue && this._value.length > 0) {
      const validValues = this._value.filter(val =>
        this.options.some(opt => opt.value === val)
      );
      this._value = validValues;
      this.emitSelection();
    }
  }

writeValue(value: (string | number)[] | null): void {
  this._value = (value ?? []).map((v) => v.toString());
  this.emitSelection();
}


  registerOnChange(fn: (value: string[]) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
    if (isDisabled) {
      this.inputControl.disable();
    } else {
      this.inputControl.enable();
    }
  }

  get value() {
    return this._value;
  }
  set value(val: string[]) {
    this._value = val;
    this.onChange(val);
    this.emitSelection();
  }

  filteredOptions() {
    const filterValue = this.inputControl.value?.toLowerCase() || '';
    return this.options.filter(
      (option) =>
        option.label.toLowerCase().includes(filterValue) &&
        !this._value.includes(option.value)
    );
  }

  getLabel(value: string): string {
    const found = this.options.find((o) => o.value === value);
    return found ? found.label : value;
  }

  add(event: MatChipInputEvent) {
    const inputValue = (event.value || '').trim();
    if (!inputValue) {
      event.chipInput!.clear();
      this.inputControl.setValue('');
      return;
    }

    const matchingOption = this.options.find(
      (option) => option.label.toLowerCase() === inputValue.toLowerCase()
    );

    if (matchingOption && !this._value.includes(matchingOption.value)) {
      this.value = [...this._value, matchingOption.value];
    }

    event.chipInput!.clear();
    this.inputControl.setValue('');
  }

  remove(value: string) {
    if (!this._value.includes(value)) return;
    this.value = this._value.filter((v) => v !== value);
    this.announcer.announce(`Removed ${this.getLabel(value)}`);
  }

  selected(event: MatAutocompleteSelectedEvent, input: HTMLInputElement) {
    const selectedValue = event.option.value;
    if (!this._value.includes(selectedValue)) {
      this.value = [...this._value, selectedValue];
    }
    this.inputControl.setValue('');
    input.value = '';
  }

  private emitSelection() {
    this.selectionChange.emit(this._value);
  }
}
