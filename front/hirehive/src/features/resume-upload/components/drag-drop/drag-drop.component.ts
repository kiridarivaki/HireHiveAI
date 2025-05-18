import { Component, Output, EventEmitter, Input, ElementRef, ViewChild, SimpleChanges } from '@angular/core';
import { FileService } from '@shared/services/file.service';
import { GetResumeInfoPayload, GetResumeUrlPayload } from 'src/app/client/models/resume-client.model';

@Component({
  selector: 'app-drag-drop',
  standalone: false,
  templateUrl: './drag-drop.component.html',
  styleUrls: ['./drag-drop.component.css']
})
export class DragDropComponent {
  @Input() file: File | null = null;
  @Input() fileMetadata: GetResumeInfoPayload | null = null;
  @Output() fileAdded = new EventEmitter<File>();
  @Output() downloadRequested = new EventEmitter<void>();
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

  onDownloadClick() {
    this.downloadRequested.emit();
  }
}