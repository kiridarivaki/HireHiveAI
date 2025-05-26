import { Component, Input } from "@angular/core";
import { ResumePreviewComponent } from "../resume-preview/resume-preview.component";
import { MatCardModule } from "@angular/material/card";
import { AppButtonComponent } from "@shared/components/button/button.component";
import { AssessResponse } from "src/app/client/models/admin-client.model";
import { FileService } from "@shared/services/file.service";
import { LoaderService } from "@shared/services/loader.service";
import { finalize } from "rxjs";

@Component({
  selector: 'app-candidate-card',
  imports: [ResumePreviewComponent, MatCardModule, AppButtonComponent],
  templateUrl: './candidate-card.component.html',
  styleUrl: './candidate-card.component.css'
})
export class CandidateCardComponent {
  @Input() assessedUserInfo!: AssessResponse;
  fileUrl: string | null = null;
  
  constructor(
    private fileService: FileService,
    private loaderService: LoaderService
  ){}

  fetchFileUrl(){
    if (this.assessedUserInfo.userId){
      this.loaderService.show();
      this.fileService.getUrl(this.assessedUserInfo.userId)
      .pipe(finalize(() => this.loaderService.hide()))
      .subscribe({
        next: (fileUrl: string) => {
          this.fileUrl = fileUrl;
        }
      });
    }
  }

  onDownload(): void {
    this.fetchFileUrl();
    if (this.fileUrl) {
      this.fileService.download(undefined, this.fileUrl);
    }
  }
}