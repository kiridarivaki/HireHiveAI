import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UrlService } from "../helpers/url-service.service";
import { GetAllResumesResponse, GetResumeInfoPayload, UpdateFormParameters, UploadFormParameters } from "../models/resume-client.model";

@Injectable({
    providedIn : 'root'
})
export class ResumeClientService{   
    constructor (
        private http : HttpClient,
        private urlService : UrlService
    ){}

    getAll() : Observable<Array<GetAllResumesResponse>>{
        const getAllUrl = this.urlService.urlFor('resume', undefined, undefined)
        return this.http.get<Array<GetAllResumesResponse>>(getAllUrl);
    }

    getById(resumeId: string) : Observable<GetResumeInfoPayload>{
        const getResumeUrl = this.urlService.urlFor('resume', undefined, { id: resumeId });
        return this.http.get<GetResumeInfoPayload>(getResumeUrl);
    }

    upload(userId: string, uploadData: UploadFormParameters) : void{
        const uploadResumeUrl = this.urlService.urlFor('resume', 'upload/{id}', { id: userId });
        this.http.post<void>(uploadResumeUrl, uploadData);
    }

    update(userId: string, updateData: UpdateFormParameters) : void{
        const updateResumeUrl = this.urlService.urlFor('resume', 'update/{id}', { id: userId });
        this.http.patch<void>(updateResumeUrl, updateData);
    }

    delete(resumeId: string) : void{
        const deleteResumeUrl = this.urlService.urlFor('resume', 'delete/{id}', { id: resumeId });
        this.http.delete<void>(deleteResumeUrl);
    }
}