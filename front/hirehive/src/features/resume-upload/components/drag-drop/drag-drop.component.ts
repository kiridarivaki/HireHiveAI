import { Component, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-drag-drop',
  standalone: false,
  templateUrl: './drag-drop.component.html',
  styleUrls: ['./drag-drop.component.css']
})
export class DragDropComponent {
  @Output() fileAdded = new EventEmitter<File>();
  file: File | null = null;
  acceptTypes: string = '.pdf';
  maxSize: number = 2 * 1024 * 1024;

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  
    const files = event.dataTransfer?.files;
  
    if (!files || files.length === 0) {
      return;
    }
  
    if (files.length > 1) {
      alert('Please drop only one file.');
      return;
    }
  
    this.fileAdded.emit(files[0]);
  } 

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input?.files?.[0];
  
    if (file) {
      this.fileAdded.emit(file);
    }
  }

  removeFile(): void {
    this.file = null;
  }
}