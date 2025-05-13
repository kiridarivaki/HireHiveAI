import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module'; 
import { UserModule } from '../features/user/user.module'; 
import { AppComponent } from './app.component'; 
import { HomeModule } from 'src/features/home/home.module';
import { HomeRoutingModule } from 'src/features/home/home-routing.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor } from './client/interceptors/error.interceptor';

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
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor , multi: true }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
