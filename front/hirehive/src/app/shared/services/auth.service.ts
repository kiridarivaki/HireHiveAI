import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LoginPayload, LoginResponse, RefreshTokenResponse, RegisterPayload } from "src/app/client/models/auth-client.model";
import { AuthClientService } from "src/app/client/services/auth-client.service";
import { StorageService } from "./storage.service";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
    constructor(
        private authClientService: AuthClientService,
        private storageService: StorageService,
        private router : Router 
    ){}

    login(loginData: LoginPayload): Observable<LoginResponse>{
        return this.authClientService.login(loginData);
    }

    register(registerData: RegisterPayload): void{
        this.authClientService.login(registerData);
    }

    isTokenExpired(): boolean{
        const expiration = this.storageService.getAuth()?.expiresIn;
        if (!expiration)
            return true;
        const expiryDate = new Date(expiration).getTime();
        const now = Date.now();
        return expiryDate < now;
    }

    refreshToken(): Observable<RefreshTokenResponse>{
        return this.authClientService.refreshToken();
    }

    logout(){
        this.storageService.removeAuth();
        this.storageService.removeUser();
        this.router.navigate(['/home']);
    }
    
    isLoggedIn(): boolean{
        const authUser = this.storageService.getAuth();
        return authUser?.accessToken !== undefined;
    }
}