import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { User } from "../models/user.model";
import { environment } from "src/environments/environment";
import { UrlService } from "../helpers/url-service.service";

@Injectable({
    providedIn : 'root'
})
export class UserClientService{   
    constructor (
        private http : HttpClient,
        private urlService : UrlService
    ){}

    getAll() : Observable<Array<User>>{
        const getAllUrl = this.urlService.urlFor('user', undefined, undefined)
        return this.http.get<Array<User>>(getAllUrl);
    }

    getById(userId: string) : Observable<User>{
        const getUserUrl = this.urlService.urlFor('user', undefined, { id: userId });
        return this.http.get<User>(getUserUrl);
    }

    update(userId: string, updateData: FormData) : void{
        const updateUserUrl = this.urlService.urlFor('user', 'update/{id}', { id: userId });
        this.http.patch<User>(updateUserUrl, User);
    }

    delete(userId: string) : void{
        const deleteUserUrl = this.urlService.urlFor('user', 'delete/{id}', { id: userId });
        this.http.request(deleteUserUrl, userId);
    }
}