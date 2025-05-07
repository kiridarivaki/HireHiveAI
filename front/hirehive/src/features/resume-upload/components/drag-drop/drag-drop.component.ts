import { Component, Output, EventEmitter } from '@angular/core';
import { serialize } from 'object-to-formdata';


@Component({
  selector: 'app-drag-drop',
  standalone: false,
  templateUrl: './drag-drop.component.html',
  styleUrls: ['./drag-drop.component.css']
})
export class DragDropComponent {
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
  
    this.handleFile(files[0]);
  }
  

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input?.files?.[0];
  
    if (file) {
      this.handleFile(file);
    }
  }

  private handleFile(file: File): void {
    const validTypes = ['application/pdf'];
    const isValidType = validTypes.includes(file.type);
    const isValidSize = file.size <= this.maxSize;
  
    if (isValidType && isValidSize) {
      this.file = file;
      console.log('File accepted:', file);
    } else {
      alert('Invalid file type or file size exceeded!');
    }
  }

  removeFile(): void {
    this.file = null;
  }
  
  handleUploadFile(){
    const formData = serialize({
      document: this.file
    });
  }
}