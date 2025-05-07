// src/main.ts
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';  // Import your root AppModule

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
