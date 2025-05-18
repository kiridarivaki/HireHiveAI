import { Component } from "@angular/core";

@Component({
  selector: 'app-candidates-match',
  standalone: false,
  templateUrl: './candidate-matches.page.html'
})
export class CandidatesMatchComponent {
  candidates = [
    { userId: '0196acea-5488-709c-8c8a-5d7d9b02fd40', name: 'John Doe' },
    { userId: 'ab12cd34-56ef-78gh-90ij-klmn1234opqr', name: 'Jane Smith' }
  ];
}
