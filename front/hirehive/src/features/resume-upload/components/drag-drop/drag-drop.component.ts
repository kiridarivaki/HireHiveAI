import { Component, Output, EventEmitter, Input, ElementRef, ViewChild } from '@angular/core';
import { GetResumeInfoPayload, GetResumeUrlPayload } from 'src/app/client/models/resume-client.model';

@Component({
  selector: 'app-drag-drop',
  standalone: false,
  templateUrl: './drag-drop.component.html',
  styleUrls: ['./drag-drop.component.css']
})
export class DragDropComponent {
  @Input() file: File | null = null;
  @Input() fileUrl: GetResumeUrlPayload | null = null;
  @Input() fileMetadata: GetResumeInfoPayload | null = null;
  @Output() fileAdded = new EventEmitter<File>();
  @ViewChild('fileInput') fileInputRef!: ElementRef<HTMLInputElement>;

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
  }

  onFileDrop(event: DragEvent): void {
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

  onFileSelect(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input?.files?.[0];
  
    if (file) {
      this.fileAdded.emit(file);
    }
  }

  clearFileInput(): void {
    if (this.fileInputRef) {
      this.fileInputRef.nativeElement.value = '';
    }
  }

  downloadFile(): void {
    if (this.file) {
      const url = URL.createObjectURL(this.file);
      const a = document.createElement('a');
      a.href = url;
      a.download = this.file.name;
      a.click();
      URL.revokeObjectURL(url);
    }
    else if (this.fileUrl){
      const a = document.createElement('a');
      a.href = this.fileUrl.sasUrl ?? '';
      a.download = this.fileUrl.fileName ?? '';
      a.click();
    }
  }
}