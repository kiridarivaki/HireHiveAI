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

    assess(assessData: AssessmentDataPayload) : Observable<Array<AssessResponse>>{
        const assessUrl = this.urlService.urlFor('admin', 'assess', undefined);
        console.log(assessData)
        return this.http.post<Array<AssessResponse>>(assessUrl, assessData);
    }
}