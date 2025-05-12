import { Component } from "@angular/core";
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatButtonModule} from '@angular/material/button';

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.component.html',
  styleUrl:'./navbar.component.css',
  imports: [MatToolbarModule, MatButtonModule]
})
export class AppNavBarComponent{
    
}