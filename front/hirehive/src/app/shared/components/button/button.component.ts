import { CommonModule } from "@angular/common";
import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: 'app-button',
    standalone: true,
    imports : [CommonModule],
    templateUrl: './button.component.html'
  })
  export class AppButtonComponent{
    @Input() btnId : string = '';
    @Input() btnClass : string = '';
    @Input() type : string = 'button'
    @Input() text : string = '';
    @Input() disabled : boolean = false;
    @Output() onClick = new EventEmitter<string>();

    emitEvent(){
        this.onClick.emit();
    }
  }