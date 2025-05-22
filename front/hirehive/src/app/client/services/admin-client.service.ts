import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UrlService } from "@shared/services/url.service";
import { Observable } from "rxjs";
import { AssessmentDataPayload, AssessResponse } from "../models/admin-client.model";

@Injectable({
    providedIn : 'root'
})
export class AdminClientService{   
    constructor (
        private http : HttpClient,
        private urlService : UrlService
    ){}

    assess(assessData: AssessmentDataPayload) : Observable<AssessResponse>{
        const uploadResumeUrl = this.urlService.urlFor('admin', 'assess', undefined);
        return this.http.post<AssessResponse>(uploadResumeUrl, assessData);
    }
}