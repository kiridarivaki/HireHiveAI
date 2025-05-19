import { Component, Input } from "@angular/core";
import { ResumePreviewComponent } from "../resume-preview/resume-preview.component";
import { MatCardModule } from "@angular/material/card";
import { GetUserInfoPayload } from "src/app/client/models/user-client.model";
import { AppButtonComponent } from "@shared/components/button/button.component";

@Component({
  selector: 'app-candidate-card',
  imports: [ResumePreviewComponent, MatCardModule, AppButtonComponent],
  templateUrl: './candidate-card.component.html',
  styleUrl: './candidate-card.component.css'
})
export class CandidateCardComponent {
  @Input() userInfo!: GetUserInfoPayload;
  @Input() matchPercentage!: string;
}
