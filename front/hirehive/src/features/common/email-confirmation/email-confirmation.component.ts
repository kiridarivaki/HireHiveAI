import { Component, OnInit } from '@angular/core';
import { AuthService } from '@shared/services/auth.service';
import { EmailConfirmationPayload } from 'src/app/client/models/auth-client.model';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './email-confirmation.component.html',
  // styleUrl: './email-confirmation.component.css'
})
export class EmailConfirmationComponent implements OnInit{
    constructor(private authService: AuthService){}

    ngOnInit(): void {
        const emailConfirmationData: EmailConfirmationPayload ={
            confirmationToken: '',
            email: ''
        } 
        
        this.authService.confirmEmail(emailConfirmationData)
    }

}