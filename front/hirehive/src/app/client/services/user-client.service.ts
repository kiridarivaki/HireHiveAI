import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UrlService } from "../helpers/url-service.service";
import { GetAllUsersResponse, GetUserInfoPayload, UpdateUserPayload } from "../models/user-client.model";

@Injectable({
    providedIn : 'root'
})
export class UserClientService{   
    constructor (
        private http : HttpClient,
        private urlService : UrlService
    ){}

    getAll() : Observable<Array<GetAllUsersResponse>>{
        const getAllUrl = this.urlService.urlFor('user', undefined, undefined)
        return this.http.get<Array<GetAllUsersResponse>>(getAllUrl);
    }

    getById(userId: string) : Observable<GetUserInfoPayload>{
        const getUserUrl = this.urlService.urlFor('user', '{id}', { id: userId });
        return this.http.get<GetUserInfoPayload>(getUserUrl);
    }

    update(userId: string, updateData: UpdateUserPayload) : Observable<any>{
        const updateUserUrl = this.urlService.urlFor('user', 'update/{id}', { id: userId });
        return this.http.patch<void>(updateUserUrl, updateData);
    }

    delete(userId: string) : Observable<any>{
        const deleteUserUrl = this.urlService.urlFor('user', 'delete/{id}', { id: userId });
        return this.http.delete<void>(deleteUserUrl);
    }
}