import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module'; 
import { UserModule } from '../features/user/user.module'; 
import { AppComponent } from './app.component'; 
import { HeaderComponent } from './shared/components/header/header.component';
import { HomeComponent } from "../features/home/home.component";

@NgModule({
  declarations: [
    AppComponent, 
    HeaderComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    UserModule
],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
