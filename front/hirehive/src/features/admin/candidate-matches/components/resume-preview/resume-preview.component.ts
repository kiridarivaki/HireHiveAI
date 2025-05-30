import { Component, Input } from '@angular/core';
import { ResumePreviewContentComponent } from './resume-preview-content.component';
import { DialogService } from '@shared/services/dialog.service';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-resume-preview',
  templateUrl: './resume-preview.component.html',
  imports: [MatButtonModule, MatIconModule, CommonModule]
})
export class ResumePreviewComponent {
  @Input() userId: string = '';
  fileUrl: string | null = null;
  hover: boolean = false;

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