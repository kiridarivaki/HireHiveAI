import { NgModule } from "@angular/core";
import { CommonRoutingModule } from "./common-routing.module";
import { EmailConfirmationComponent } from "./email-confirmation/email-confirmation.component";

@NgModule({
  declarations: [EmailConfirmationComponent], 
  imports: [CommonRoutingModule] 
})
export class CommonModuleModule {}