import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module'; 
import { UserModule } from '../features/user/user.module'; 
import { AppComponent } from './app.component'; 
import { HomeModule } from 'src/features/home/home.module';
import { HomeRoutingModule } from 'src/features/home/home-routing.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    UserModule,
    HomeModule,
    HomeRoutingModule
],
  providers: [
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
