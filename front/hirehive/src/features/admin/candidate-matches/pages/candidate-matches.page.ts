import { Component, OnInit } from '@angular/core';
import { CandidateCardComponent } from '../components/candidate-card/candidate-card.component';
import { CommonModule } from '@angular/common';
import { JobStateService } from '@shared/services/job-state.service';
import { AdminClientService } from 'src/app/client/services/admin-client.service';
import { ErrorService } from '@shared/services/error.service';
import { AssessResponse } from 'src/app/client/models/admin-client.model';

@Component({
  selector: 'app-candidates-match',
  templateUrl: './candidate-matches.page.html',
  imports: [CandidateCardComponent, CommonModule]
})
export class CandidateMatchesComponent implements OnInit {
  usersList?: Array<AssessResponse> = [];

  constructor(
    private adminService: AdminClientService,
    private stateService: JobStateService, 
    private errorService: ErrorService
  ){}

  ngOnInit() {
    const assessmentData = this.stateService.getAssessmentData();

    if (assessmentData) {
      this.fetchNextBatch();
    } else {
      this.errorService.showError('No information about the job was given.');
    }
  }

  fetchNextBatch() {
    const assessmentData = this.stateService.getAssessmentData();
    const cursor = this.stateService.getCursor();

    if (!assessmentData) return;

    const requestPayload = {
      ...assessmentData,
      cursor
    };

    this.adminService.assess(requestPayload).subscribe({
      next: (users) => {
        const count = users?.length ?? 0;
        this.stateService.incrementCursor(count);
        this.usersList = users;
      },
      error: (err) => {
        console.log(err)
        this.errorService.showError('No candidates matched this job position.');
      }
    });
  }

}

