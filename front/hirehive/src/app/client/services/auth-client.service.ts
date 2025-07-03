import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { UrlService } from "../../shared/services/url.service";
import { EmailConfirmationPayload, EmailConfirmationResendPayload, GetInfoResponse, LoginPayload, LoginResponse, RefreshTokenPayload, RefreshTokenResponse, RegisterPayload, RegisterResponse } from "../models/auth-client.model";
import { Observable } from "rxjs";

@Injectable({
    providedIn : 'root'
})
export class AuthClientService{
    constructor(private http: HttpClient, private urlService: UrlService){ }

    register(registerData: RegisterPayload): Observable<RegisterResponse>{
        const registerUrl = this.urlService.urlFor('auth', 'register', undefined);
        return this.http.post<RegisterResponse>(registerUrl, registerData)
    }

    login(loginData: LoginPayload): Observable<LoginResponse>{
        const loginUrl = this.urlService.urlFor('auth', 'login', undefined)
        return this.http.post<LoginResponse>(loginUrl, loginData)
    }

    refreshToken(refreshTokenData: RefreshTokenPayload): Observable<RefreshTokenResponse>{
        const refreshTokenUrl = this.urlService.urlFor('auth', 'refresh', undefined)
        return this.http.post<RefreshTokenResponse>(refreshTokenUrl, refreshTokenData)
    }

    revokeRefreshToken(): Observable<void>{
        const revokeRefreshTokenUrl = this.urlService.urlFor('auth', 'revoke', undefined)
        return this.http.post<void>(revokeRefreshTokenUrl, {})
    }

    getUserInfo(userId: string) : Observable<GetInfoResponse>{
        const getInfoUrl = this.urlService.urlFor('auth', 'get-info/{id}', {id: userId})
        return this.http.get<GetInfoResponse>(getInfoUrl);
    }

    confirmEmail(confirmEmailData: EmailConfirmationPayload): Observable<any> {
        const confirmEmailUrl = this.urlService.urlFor('auth', 'confirm-email', undefined);
        return this.http.post<void>(confirmEmailUrl, confirmEmailData);
    }
    
    resendConfirmation(emailResendData: EmailConfirmationResendPayload): Observable<void> {
        const resendConfirmationUrl = this.urlService.urlFor('auth', 'resend-confirmation', undefined);
        return this.http.post<void>(resendConfirmationUrl, emailResendData);
    }
}
