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
        const a = document.createElement('a');

        if (file) {
            const url = URL.createObjectURL(file);
            a.href = url;
            a.download = file.name;
            a.click();
            URL.revokeObjectURL(url);
        } 
        else if (fileUrl) {
            a.href = fileUrl;
            a.target = '_blank';
            a.rel = 'noopener noreferrer';

            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }
    }
}