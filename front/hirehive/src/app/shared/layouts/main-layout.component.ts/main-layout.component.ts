import { Component } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { RouterModule } from '@angular/router';
import { AppFooterComponent } from '@shared/components/footer/footer.component';
import { AppNavBarComponent } from '@shared/components/navbar/navbar.component';

@Component({
  selector: 'app-main-layout',
  imports: [FlexLayoutModule, RouterModule, AppNavBarComponent, AppFooterComponent ],
  templateUrl: './main-layout.component.html',
  styleUrl: './main-layout.component.css'
})
export class MainLayoutComponent {

}
