import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Resume } from "../models/resume.model";
import { environment } from "src/environments/environment";
import { UrlService } from "../helpers/url-service.service";

@Injectable({
    providedIn : 'root'
})
export class ResumeClientService{   
    constructor (
        private http : HttpClient,
        private urlService : UrlService
    ){}

    getAll() : Observable<Array<Resume>>{
        const getAllUrl = this.urlService.urlFor('Resume', undefined, undefined)
        return this.http.get<Array<Resume>>(getAllUrl);
    }

    getById(resumeId: string) : Observable<Resume>{
        const getResumeUrl = this.urlService.urlFor('resume', undefined, { id: resumeId });
        return this.http.get<Resume>(getResumeUrl);
    }

    upload(userId: string, uploadData: FormData) : void{
        const uploadResumeUrl = this.urlService.urlFor('resume', 'upload/{id}', { id: userId });
        this.http.patch<Resume>(uploadResumeUrl, uploadData);
    }

    update(userId: string, updateData: FormData) : void{
        const updateResumeUrl = this.urlService.urlFor('resume', 'update/{id}', { id: userId });
        this.http.patch<Resume>(updateResumeUrl, updateData);
    }

    delete(resumeId: string) : void{
        const deleteResumeUrl = this.urlService.urlFor('resume', 'delete/{id}', { id: resumeId });
        this.http.request(deleteResumeUrl, resumeId);
    }
}