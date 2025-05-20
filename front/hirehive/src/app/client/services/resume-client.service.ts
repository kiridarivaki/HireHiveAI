import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UrlService } from "../../shared/services/url.service";
import { GetAllResumesResponse, GetResumeInfoPayload, GetResumeUrlPayload, UpdateResumePayload, UploadResumePayload } from "../models/resume-client.model";

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

    getById(userId: string) : Observable<GetResumeInfoPayload>{
        const getResumeUrl = this.urlService.urlFor('resume', '{userId}', { userId });
        return this.http.get<GetResumeInfoPayload>(getResumeUrl);
    }

    getFileUrl(userId: string) : Observable<string>{
        const getFileUrlUrl = this.urlService.urlFor('resume', 'url/{userId}', { userId });
        return this.http.get<string>(getFileUrlUrl);
    }

    getFileStream(userId: string): Observable<Blob> { 
        const getFileUrlUrl = this.urlService.urlFor('resume', 'stream/{userId}', { userId });
        return this.http.get(getFileUrlUrl, { responseType: 'blob' });
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