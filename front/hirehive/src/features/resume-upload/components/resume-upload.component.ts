import { Component, ElementRef, Input, OnInit, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { fileValidator } from "@shared/validators/file.validator";
import { GetResumeInfoPayload } from "src/app/client/models/resume-client.model";
import { ResumeClientService } from "src/app/client/services/resume-client.service";
import { DragDropComponent } from "./drag-drop/drag-drop.component";
import { FileService } from "@shared/services/file.service";

@Component({
    selector: 'app-resume-upload',
    standalone: false,
    templateUrl: './resume-upload.component.html'
  })
  
export class ResumeUploadComponent implements OnInit{
  @Input() userId?: string | null;
  fileMetadata: GetResumeInfoPayload | null = null;
  fileUrl: string | null = null;
  @ViewChild(DragDropComponent) dragDropComponent!: DragDropComponent;

  constructor(
    private resumeService: ResumeClientService,
    private fileService: FileService
  ){}

  ngOnInit(): void {
    this.fetchFileUrl();
    this.fetchFileMetadata();
  }

  fetchFileMetadata(){
      if (this.userId){
        this.resumeService.getById(this.userId).subscribe({
          next: (fileInfo: GetResumeInfoPayload) => {
            console.log('File fetched successfully.');
            this.fileMetadata = fileInfo;
          },
          error: (err) => {
            console.error('Fetch failed:', err);
          }
      });
    }
  }

  fetchFileUrl(){
    if (this.userId){
      this.fileService.getUrl(this.userId).subscribe({
        next: (fileUrl: string) => {
          console.log('File fetched successfully.');
          this.fileUrl = fileUrl;
        },
        error: (err) => {
          console.error('Fetch failed:', err);
        }
      });
    }
  }

  uploadForm = new FormGroup({
    selectedFile: new FormControl<File | null>(null, [Validators.required, fileValidator])
  });

  onFileReceived(file: File): void {
    this.uploadForm.patchValue({ selectedFile: file });
    this.uploadForm.markAsDirty();
  }

  onFileRemoved(): void {
    this.uploadForm.reset();

    if (this.dragDropComponent)
      this.dragDropComponent.clearFileInput();

    if (this.fileMetadata && this.userId) {
      this.resumeService.delete(this.userId).subscribe({
        next: () => {
          this.fileMetadata = null;
          this.fileUrl = null;
          console.log('File removed successfully.');
        },
        error: (err) => {
          console.error('File removal failed:', err);
        }
      });
    }
  }

  onSubmit(): void {
    if (!this.userId) return;
    const uploadData = new FormData();
    const file: File | null | undefined = this.uploadForm.get('selectedFile')?.value;
    if (file) {
      uploadData.append('file', file);
      this.resumeService.upload(this.userId, uploadData).subscribe({
        next: () => {
          console.log('File uploaded successfully.');
          this.uploadForm.get('selectedFile')?.setValue(file);
        },
        error: (err) => {
          console.error('Upload failed:', err);
        }
      })
    };
  }

  onDownload(): void {
    console.log(this.fileUrl)
    if (this.selectedFile) {
      this.fileService.download(this.selectedFile, undefined);
    } else if (this.fileUrl) {
      console.log('check')
      this.fileService.download(undefined, this.fileUrl);
    }
  }

  get selectedFile(): File | null {
    return this.uploadForm.get('selectedFile')?.value ?? null;
  }
}
