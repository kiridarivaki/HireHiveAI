import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UrlService } from "../helpers/url-service.service";
import { LoginFormParameters, LoginResponse, RegisterFormParameters } from "../models/auth-client.model";
import { Observable } from "rxjs";

@Injectable({
    providedIn : 'root'
})
export class AuthClientService{
    constructor(private http: HttpClient, private urlService: UrlService){ }

    register(registerData: RegisterFormParameters){
        const registerUrl = this.urlService.urlFor("user", "register", undefined);
        return this.http.post<void>(registerUrl, registerData)
    }

    login(loginData: LoginFormParameters): Observable<LoginResponse>{
        const loginUrl = this.urlService.urlFor("user", "login", undefined)
        return this.http.post<LoginResponse>(loginUrl, loginData)
    }
}
