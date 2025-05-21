import { NgModule } from "@angular/core";
import { EmailConfirmationComponent } from "./email-confirmation/email-confirmation.component";
import { CoreRoutingModule } from "./core-routing.module";

@NgModule({
  imports: [CoreRoutingModule, EmailConfirmationComponent] 
})
export class CoreModule {}