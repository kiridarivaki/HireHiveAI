import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Resume } from "../models/resume.model";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn : 'root'
})
export class ResumeClientService{
    
    private url = `${environment.apiBaseUrl}/user`;

    constructor (
        private http : HttpClient
    ){}

    getAll() : Observable<Array<Resume>>{
        return this.http.get<Array<Resume>>(this.url);
    }

    get() : Observable<Resume>{
        return this.http.get<Resume>(this.url);
    }

    upload(resume: Resume) : void{
        this.http.post<Resume>(this.url, resume);
    }

    delete(resume: Resume) : void{
        this.http.post<Resume>(this.url, resume);
    }
}