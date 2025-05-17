import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { CandidateMatchesComponent } from "./candidate-matches/pages/candidate-matches.page";
import { AdminRoutingModule } from "./admin-routing.module";
import { ResumePreviewComponent } from "./candidate-matches/components/resume-preview/resume-preview.component";

@NgModule({
  declarations: [CandidateMatchesComponent],
  imports: [
    CommonModule,
    AdminRoutingModule,
    ResumePreviewComponent
  ]
})
export class AdminModule {}
