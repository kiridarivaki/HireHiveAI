import { Injectable } from "@angular/core";
import { BehaviorSubject, map, Observable } from "rxjs";
import { EmailConfirmationPayload, EmailConfirmationResendPayload, GetInfoResponse, LoginPayload, LoginResponse, RefreshTokenResponse, RegisterPayload } from "src/app/client/models/auth-client.model";
import { AuthClientService } from "src/app/client/services/auth-client.service";
import { StorageService } from "./storage.service";
import { Router } from "@angular/router";
import { User } from "@shared/models/user.model";
import { UserRole } from "@shared/models/auth.model";

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

    isAdmin$(): Observable<boolean> {
        return this.currentUser$.pipe(
            map(user => !!user?.roles?.includes(UserRole.admin))
        );
    }

    register(registerData: RegisterPayload): Observable<any>{
        return this.authClientService.register(registerData);
    }

    confirmEmail(confirmEmailData: EmailConfirmationPayload): Observable<any>{
        return this.authClientService.confirmEmail(confirmEmailData);
    }

    resendConfirmation(emailResendData: EmailConfirmationResendPayload): Observable<any>{
        return this.authClientService.resendConfirmation(emailResendData);
    }

    isTokenExpired(): boolean {
        const expiration = this.storageService.getAuth()?.expiresIn;
        if (!expiration) return true;

        const expiryTime = Number(expiration);
        const now = Date.now();
        
        return expiryTime < now;
    }

    refreshToken(): Observable<RefreshTokenResponse>{
        return this.authClientService.refreshToken();
    }

    logout(){
        this.storageService.removeAuth();
        this.storageService.removeUser();
        this.currentUserSubject.next(null);
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