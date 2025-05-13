import { CommonModule } from "@angular/common";
import { Component, Input, Output, EventEmitter } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";

@Component({
    selector: 'app-button',
    standalone: true,
    imports : [
      CommonModule, 
      MatButtonModule,
      MatIconModule
    ],
    templateUrl: './button.component.html'
  })
  export class AppButtonComponent{
    @Input() btnId: string = '';
    @Input() btnClass: string = '';
    @Input() type: string = 'button'
    @Input() text: string = '';
    @Input() icon: string = '';
    @Input() disabled: boolean = false;
    @Output() onClick = new EventEmitter<string>();

    emitEvent(){
        this.onClick.emit();
    }
  }