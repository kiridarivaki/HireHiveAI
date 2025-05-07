import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-file-info',
  templateUrl: './file-info.component.html',
  styleUrls: ['./file-info.component.css']
})
export class FileInfoComponent {
  @Input() file!: File;
}