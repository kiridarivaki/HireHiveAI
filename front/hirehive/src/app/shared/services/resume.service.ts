import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Resume } from "src/app/client/models/resume.model";
import { ResumeClientService } from "src/app/client/services/resume-client.service";

@Injectable({
    providedIn : 'root'
})
export class ResumeService{ 
    constructor (
        private resumeClientService : ResumeClientService
    ){}

        getAll() : Observable<Array<Resume>>{
            return this.resumeClientService.getAll();
        }
    
        getById(resumeId: string) : Observable<Resume>{
            return this.resumeClientService.getById(resumeId);
        }
    
        upload(userId: string, uploadData: FormData) : void{
            this.resumeClientService.upload(userId, uploadData);
        }
    
        update(userId: string, updateData: FormData) : void{
            this.resumeClientService.update(userId, updateData);
        }
    
        delete(resumeId: string) : void{
            this.resumeClientService.delete(resumeId);
        }
}