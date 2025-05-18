import { Component, Input } from '@angular/core';
import { ResumePreviewContentComponent } from './resume-preview-content.component';
import { DialogService } from '@shared/services/dialog.service';
import { FileService } from '@shared/services/file.service';
import { AppButtonComponent } from '@shared/components/button/button.component';

@Component({
  selector: 'app-resume-preview',
  templateUrl: './resume-preview.component.html',
  imports: [AppButtonComponent]
})
export class ResumePreviewComponent {
  @Input() userId: string = '0196acea-5488-709c-8c8a-5d7d9b02fd40';
  fileUrl: string | null = null;

  constructor(
    private dialogService: DialogService,
  ) {}

  openPreview() {
    this.dialogService.open(ResumePreviewContentComponent, {
      title: 'Resume Preview',
      width: '600px',
      height: '580px',
      data: { userId: this.userId }
    });
  }
}