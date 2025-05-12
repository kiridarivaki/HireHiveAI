import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { User } from "src/app/client/models/user.model";
import { UserClientService } from "src/app/client/services/user-client.service";

@Injectable({
    providedIn : 'root'
})
export class UserService{ 
    constructor (
        private userClientService : UserClientService
    ){}

        getAll() : Observable<Array<User>>{
            return this.userClientService.getAll();
        }
    
        getById(userId: string) : Observable<User>{
            return this.userClientService.getById(userId);
        }
    
        update(userId: string, updateData: FormData) : void{
            this.userClientService.update(userId, updateData);
        }
    
        delete(userId: string) : void{
            this.userClientService.delete(userId);
        }
}