import { Injectable } from '@angular/core';
import { StoredAuth } from '@shared/models/auth.model';
import { User } from '@shared/models/user.model';

@Injectable({
  providedIn: 'root'
})
export class StorageService {
    storeAuth(auth: StoredAuth): void {
    const expiryTimestamp = Date.now() + auth.expiresIn * 1000;

    const updatedAuth: StoredAuth = {   
        accessToken: auth.accessToken,
        refreshToken: auth.refreshToken,
        expiresIn: expiryTimestamp
    };

    localStorage.setItem("auth", JSON.stringify(updatedAuth));
    }

    getAuth(): StoredAuth | null {
        const storedAuth = localStorage.getItem("auth");
        return storedAuth ? (JSON.parse(storedAuth) as StoredAuth) : null;
    }

    removeAuth(): void{
        localStorage.removeItem("auth");
    }

    setUser(user: User): void{
        localStorage.setItem("user", JSON.stringify(user));
    }

    getUser(): User | null{
        const user = localStorage.getItem("user");
        return user ? (JSON.parse(user)as User) : null;
    }

    removeUser(): void{
        localStorage.removeItem("user");
    }
}