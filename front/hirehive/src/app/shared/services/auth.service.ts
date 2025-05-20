import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { GetInfoResponse, LoginPayload, LoginResponse, RefreshTokenResponse, RegisterPayload } from "src/app/client/models/auth-client.model";
import { AuthClientService } from "src/app/client/services/auth-client.service";
import { StorageService } from "./storage.service";
import { Router } from "@angular/router";
import { User } from "@shared/models/user.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
    currentUser$!: Observable<User | null>;
    private currentUserSubject!: BehaviorSubject<User | null>;

    constructor(
        private authClientService: AuthClientService,
        private storageService: StorageService,
        private router : Router 
    ){
        this.initializeUser()
    }

    login(loginData: LoginPayload): Observable<LoginResponse>{
        return this.authClientService.login(loginData);
    }

    setUser(user: User): void {
        this.currentUserSubject.next(user);
    }

    getCurrentUser(): User | null{
        return this.currentUserSubject.value;
    }

    getUserInfo(userId: string) : Observable<GetInfoResponse>{
        return this.authClientService.getUserInfo(userId);
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

    private initializeUser(): void {
        const user = this.storageService.getUser();
        this.currentUserSubject = new BehaviorSubject<User | null>(user);
        this.currentUser$ = this.currentUserSubject.asObservable();
    }
}