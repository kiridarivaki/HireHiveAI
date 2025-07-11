import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module'; 
import { UserModule } from '../features/user/user.module'; 
import { AppComponent } from './app.component'; 
import { HomeModule } from 'src/features/core/home/home.module';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { ErrorInterceptor } from './shared/interceptors/error.interceptor';
import { TokenInterceptor } from './shared/interceptors/token.interceptor';
import { ErrorComponent } from '@shared/components/error/error.component';
import { LoaderComponent } from '@shared/components/loader/loader.component';
import { AppNotificationComponent } from '@shared/components/notification/notification.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    UserModule,
    ErrorComponent, 
    AppNotificationComponent,
    LoaderComponent
],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true,
    },
    { 
      provide: HTTP_INTERCEPTORS, 
      useClass: ErrorInterceptor , 
      multi: true 
    }
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
