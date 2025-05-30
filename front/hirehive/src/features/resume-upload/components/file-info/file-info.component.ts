import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { GetResumeInfoPayload } from 'src/app/client/models/resume-client.model';

@Component({
  selector: 'app-file-info',
  templateUrl: './file-info.component.html',
  styleUrl: './file-info.component.scss',
  imports: [CommonModule]
})
export class FileInfoComponent {
  @Input() file!: File;
  @Input() fileMetadata: GetResumeInfoPayload | null = null;
}