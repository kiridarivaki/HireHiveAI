import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { User } from "../models/user.model";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn : 'root'
})
export class UserClientService{
    
    private url = `${environment.apiBaseUrl}/User`;

    constructor (
        private http : HttpClient
    ){}

    getAll() : Observable<Array<User>>{
        return this.http.get<Array<User>>(this.url);
    }

    get() : Observable<User>{
        return this.http.get<User>(this.url);
    }

    upload(User: User) : void{
        this.http.post<User>(this.url, User);
    }

    delete(User: User) : void{
        this.http.post<User>(this.url, User);
    }
}