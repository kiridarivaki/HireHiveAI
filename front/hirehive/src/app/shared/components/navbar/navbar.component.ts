import { Component } from "@angular/core";
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatButtonModule} from '@angular/material/button';
import { RouterModule } from "@angular/router";

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.component.html',
  styleUrl:'./navbar.component.scss',
  imports: [MatToolbarModule, MatButtonModule, RouterModule]
})
export class AppNavBarComponent{
    
}