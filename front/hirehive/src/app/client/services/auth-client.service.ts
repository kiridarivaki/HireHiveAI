import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UrlService } from "../helpers/url-service.service";
import { LoginPayload, LoginResponse, RefreshTokenResponse, RegisterPayload } from "../models/auth-client.model";
import { Observable } from "rxjs";

@Injectable({
    providedIn : 'root'
})
export class AuthClientService{
    constructor(private http: HttpClient, private urlService: UrlService){ }

    register(registerData: RegisterPayload){
        const registerUrl = this.urlService.urlFor("auth", "register", undefined);
        return this.http.post<void>(registerUrl, registerData)
    }

    login(loginData: LoginPayload): Observable<LoginResponse>{
        const loginUrl = this.urlService.urlFor("auth", "login", undefined)
        return this.http.post<LoginResponse>(loginUrl, loginData)
    }

    refreshToken(): Observable<RefreshTokenResponse>{
        const refreshTokenUrl = this.urlService.urlFor("auth", "refreshToken", undefined)
        return this.http.get<RefreshTokenResponse>(refreshTokenUrl)
    }
}
