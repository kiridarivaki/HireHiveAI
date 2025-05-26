import { Component, Output, EventEmitter, Input, ElementRef, ViewChild } from '@angular/core';
import { GetResumeInfoPayload } from 'src/app/client/models/resume-client.model';

@Component({
  selector: 'app-drag-drop',
  standalone: false,
  templateUrl: './drag-drop.component.html',
  styleUrls: ['./drag-drop.component.css']
})
export class DragDropComponent {
  @Input() file: File | null = null;
  @Input() fileMetadata: GetResumeInfoPayload | null = null;
  @Input() disabled: boolean = false;
  @Output() fileAdded = new EventEmitter<File>();
  @Output() downloadRequested = new EventEmitter<void>();
  @ViewChild('fileInput') fileInputRef!: ElementRef<HTMLInputElement>;

  onDragOver(event: DragEvent): void {
    if (this.disabled) {
      event.preventDefault();
      event.stopPropagation();
      return;
    }
    event.preventDefault();
    event.stopPropagation();
  }

  onDragLeave(event: DragEvent): void {
    if (this.disabled) {
      event.preventDefault();
      event.stopPropagation();
      return;
    }
    event.preventDefault();
    event.stopPropagation();
  }

  onFileDrop(event: DragEvent): void {
    if (this.disabled) {
      event.preventDefault();
      event.stopPropagation();
      return;
    }
  
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
    if (this.disabled) {
      const input = event.target as HTMLInputElement;
      if (input) input.value = '';  // reset input so user can try again later
      return;
    }

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

  onDownloadClick() {
    this.downloadRequested.emit();
  }
}
