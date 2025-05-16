import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UrlService } from "../helpers/url-service.service";
import { GetAllResumesResponse, GetResumeInfoPayload, UpdateResumePayload, UploadResumePayload } from "../models/resume-client.model";

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

    upload(userId: string, uploadData: UploadResumePayload) : Observable<any>{
        const uploadResumeUrl = this.urlService.urlFor('resume', 'upload/{userId}', { userId });
        return this.http.post<void>(uploadResumeUrl, uploadData);
    }

    update(userId: string, updateData: UpdateResumePayload) : Observable<any>{
        const updateResumeUrl = this.urlService.urlFor('resume', 'update/{userId}', { userId });
        return this.http.patch<void>(updateResumeUrl, updateData);
    }

    delete(resumeId: string) : Observable<any>{
        const deleteResumeUrl = this.urlService.urlFor('resume', 'delete/{resumeId}', { resumeId });
        return this.http.delete<void>(deleteResumeUrl);
    }
}