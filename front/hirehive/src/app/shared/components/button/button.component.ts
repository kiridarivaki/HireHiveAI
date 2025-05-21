import { CommonModule } from "@angular/common";
import { Component, Input, Output, EventEmitter } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { RouterModule } from "@angular/router";

@Component({
    selector: 'app-button',
    standalone: true,
    imports : [
      CommonModule, 
      MatButtonModule,
      MatIconModule,
      RouterModule
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
    @Input() routerLink?: string | any[];
    @Output() onClick = new EventEmitter<string>();

    emitEvent(){
        this.onClick.emit();
    }
  }