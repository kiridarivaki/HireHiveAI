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
    
    getUrl(userId: string): Observable<string> {
        return this.resumeService.getFileUrl(userId);
    }

    download(file?: File, fileUrl?: string): void {
        if (file) {
            const url = URL.createObjectURL(file);
            const a = document.createElement('a');
            a.href = url;
            a.download = file.name;
            a.click();
            URL.revokeObjectURL(url);
        } 
        else if (fileUrl) {
            const anchor = document.createElement('a');
            anchor.href = fileUrl;
            anchor.target = '_blank';
            anchor.rel = 'noopener noreferrer';

            document.body.appendChild(anchor);
            anchor.click();
            document.body.removeChild(anchor);
        }
    }
}