import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { GetResumeUrlPayload } from "src/app/client/models/resume-client.model";
import { ResumeClientService } from "src/app/client/services/resume-client.service";
import { DialogService } from "./dialog.service";
import { ResumePreviewComponent } from "src/features/admin/candidate-matches/components/resume-preview/resume-preview.component";

@Injectable({
  providedIn: 'root'
})
export class FileService {
    constructor(
        private resumeService: ResumeClientService,
        private dialogService: DialogService
    ){}
    
    getUrl(userId: string): Observable<GetResumeUrlPayload> {
        return this.resumeService.getFileUrl(userId);
    }

    downloadFile(file: GetResumeUrlPayload): void {
        const anchor = document.createElement('a');
        anchor.href = file.sasUrl ?? '';
        anchor.download = file.fileName ?? '';
        anchor.click();
        anchor.remove();
    }

    openPreview(fileUrl: string) {
        this.dialogService.open(ResumePreviewComponent, {
        title: 'Resume Preview',
        data: { fileUrl },
        width: '800px'
        });
    }
}