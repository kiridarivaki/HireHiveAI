import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UrlService } from "../../shared/services/url.service";
import { EmailConfirmationPayload, GetInfoResponse, LoginPayload, LoginResponse, RefreshTokenResponse, RegisterPayload } from "../models/auth-client.model";
import { Observable } from "rxjs";

@Injectable({
    providedIn : 'root'
})
export class AuthClientService{
    constructor(private http: HttpClient, private urlService: UrlService){ }

    register(registerData: RegisterPayload){
        const registerUrl = this.urlService.urlFor('auth', 'register', undefined);
        return this.http.post<void>(registerUrl, registerData)
    }

    login(loginData: LoginPayload): Observable<LoginResponse>{
        const loginUrl = this.urlService.urlFor('auth', 'login', undefined)
        return this.http.post<LoginResponse>(loginUrl, loginData)
    }

    refreshToken(): Observable<RefreshTokenResponse>{
        const refreshTokenUrl = this.urlService.urlFor('auth', 'refresh-token', undefined)
        return this.http.get<RefreshTokenResponse>(refreshTokenUrl)
    }

    getUserInfo(userId: string) : Observable<GetInfoResponse>{
        const getInfoUrl = this.urlService.urlFor('auth', 'get-info/{id}', {id: userId})
        return this.http.get<GetInfoResponse>(getInfoUrl);
    }

    confirmEmail(confirmEmailData: EmailConfirmationPayload){
        const confirmEmailUrl = this.urlService.urlFor('auth', 'confirm-email', undefined)
        return this.http.post<void>(confirmEmailUrl, confirmEmailData);
    }
}
