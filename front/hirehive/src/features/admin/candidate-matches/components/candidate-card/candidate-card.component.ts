import { Component, Input } from "@angular/core";
import { ResumePreviewComponent } from "../resume-preview/resume-preview.component";

@Component({
  selector: 'app-candidate-card',
  imports: [ResumePreviewComponent],
  template: `
    <div>
      <p>{{ candidate.name }}</p>
      <app-resume-preview [userId]="candidate.userId"></app-resume-preview>
    </div>
  `
})
export class CandidateCardComponent {
  @Input() candidate!: { userId: string; name: string; };
}
