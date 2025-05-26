import { Component, ElementRef, Input, OnInit, ViewChild } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { fileValidator } from "@shared/validators/file.validator";
import { GetResumeInfoPayload } from "src/app/client/models/resume-client.model";
import { ResumeClientService } from "src/app/client/services/resume-client.service";
import { DragDropComponent } from "./drag-drop/drag-drop.component";
import { FileService } from "@shared/services/file.service";
import { DialogService } from "@shared/services/dialog.service";
import { ConfirmDialogComponent } from "@shared/components/dialog/confirm-dialog/confirm-dialog.component";
import { finalize } from "rxjs";
import { LoaderService } from "@shared/services/loader.service";
import { NotificationService } from "@shared/services/notification.service";

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
    private fileService: FileService,
    private loaderService: LoaderService,
    private dialogService: DialogService,
    private notificationService: NotificationService
  ){}

  ngOnInit(): void {
    this.fetchFileUrl();
    this.fetchFileMetadata();
  }

  fetchFileMetadata(){
      this.loaderService.show();
      if (this.userId){
        this.resumeService.getById(this.userId)
        .pipe(finalize(() => this.loaderService.hide()))
        .subscribe({
          next: (fileInfo: GetResumeInfoPayload) => {
            this.fileMetadata = fileInfo;
          }
      });
    }
  }

  fetchFileUrl(){
    if (this.userId){
      this.loaderService.show();
      this.fileService.getUrl(this.userId)
      .pipe(finalize(() => this.loaderService.hide()))
      .subscribe({
        next: (fileUrl: string) => {
          this.fileUrl = fileUrl;
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
    this.dialogService.open(ConfirmDialogComponent, {
      title: 'Confirm Removal',
      data: {
        message: 'Are you sure you want to remove the current resume file?',
        confirmText: 'Remove',
        cancelText: 'Cancel'
      },
      width: '400px'
    })
    .afterClosed().subscribe(confirmed => {
      if (!confirmed) {
        return;
      }

      this.uploadForm.reset();

      if (this.dragDropComponent) {
        this.dragDropComponent.clearFileInput();
      }

      if (this.fileMetadata && this.userId) {
        this.resumeService.delete(this.userId).subscribe({
          next: () => {
            this.fileMetadata = null;
            this.fileUrl = null;
            this.notificationService.showNotification('File removed successfully!');
          }
        });
      }
    });
  }

  onSubmit(): void {
    if (!this.userId) return;
    const uploadData = new FormData();
    const file: File | null | undefined = this.uploadForm.get('selectedFile')?.value;
    if (file) {
      uploadData.append('file', file);
      this.resumeService.upload(this.userId, uploadData).subscribe({
        next: () => {
          this.uploadForm.get('selectedFile')?.setValue(file);
          this.notificationService.showNotification('File uploaded successfully!');
        }
      })
    };
  }

  onDownload(): void {
    if (this.selectedFile) {
      this.fileService.download(this.selectedFile, undefined);
    } else if (this.fileUrl) {
      this.fileService.download(undefined, this.fileUrl);
    }
  }

  get selectedFile(): File | null {
    return this.uploadForm.get('selectedFile')?.value ?? null;
  }
}
