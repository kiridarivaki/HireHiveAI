import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { User } from "../models/user.model";

@Injectable({
    providedIn : 'root'
})
export class AuthClientService{
    
    private baseUrl = `${environment.apiBaseUrl}/User`;
    constructor(private http: HttpClient){ }

    register(userObj:User){
        return this.http.post<User>('${this.baseUrl}/register', userObj)
    }

    login(userObj:User){
        return this.http.post<User>('${this.baseUrl}/login', userObj)
    }
}
