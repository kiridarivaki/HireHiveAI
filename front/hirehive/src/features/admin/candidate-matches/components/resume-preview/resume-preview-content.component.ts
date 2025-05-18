import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ResumeClientService } from 'src/app/client/services/resume-client.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-resume-preview-content',
  templateUrl: './resume-preview-content.html',
  imports:[CommonModule]
})
export class ResumePreviewContentComponent implements OnInit, OnDestroy {
  sanitizedUrl?: SafeResourceUrl;
  private objectUrl?: string;
  errorMessage: string | null = null;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { userId: string },
    private resumeService: ResumeClientService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit() {
    this.resumeService.getFileStream(this.data.userId).subscribe({
      next: (blob) => {
        this.objectUrl = URL.createObjectURL(blob);
        this.sanitizedUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.objectUrl);
      },
      error: async (err) => {
        this.errorMessage = 'Unable to retrieve the resume file.';
      }
    });
  }

  ngOnDestroy() {
    if (this.objectUrl) {
      URL.revokeObjectURL(this.objectUrl);
    }
  }
}
