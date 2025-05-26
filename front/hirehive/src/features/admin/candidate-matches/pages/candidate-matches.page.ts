import { Component, OnInit } from '@angular/core';
import { CandidateCardComponent } from '../components/candidate-card/candidate-card.component';
import { CommonModule } from '@angular/common';
import { JobStateService } from '@shared/services/job-state.service';
import { AdminClientService } from 'src/app/client/services/admin-client.service';
import { ErrorService } from '@shared/services/error.service';
import { AssessResponse } from 'src/app/client/models/admin-client.model';
import { Router } from '@angular/router';
import { finalize, Observable } from 'rxjs';
import { LoaderService } from '@shared/services/loader.service';
import { AppButtonComponent } from '@shared/components/button/button.component';

@Component({
  selector: 'app-candidates-match',
  templateUrl: './candidate-matches.page.html',
  imports: [CandidateCardComponent, CommonModule, AppButtonComponent]
})
export class CandidateMatchesComponent implements OnInit {
  usersList: Array<AssessResponse> = [];
  noMoreResults: boolean = false;
  loading$!: Observable<boolean>;

  constructor(
    private adminService: AdminClientService,
    private stateService: JobStateService, 
    private errorService: ErrorService,
    private loaderService: LoaderService,
    private router: Router
  ){}

  ngOnInit() {
    const assessmentData = this.stateService.getAssessmentData();
    if (!assessmentData) {
      this.errorService.showError('No information about the job was given.');
      return;
    }

    this.loading$ = this.loaderService.loading$;
    this.usersList = [];
    this.stateService.setCursor(0);
    this.fetchNextBatch();
  }

  fetchNextBatch() {
    const assessmentData = this.stateService.getAssessmentData();
    const cursor = this.stateService.getCursor();

    if (!assessmentData) return;

    this.loaderService.show();

    const requestPayload = {
      ...assessmentData,
      cursor
    };

    this.adminService.assess(requestPayload).pipe(
      finalize(() => {
        this.loaderService.hide();
      })
    ).subscribe({
      next: (users) => {
        const batchSize = users?.length ?? 0;

        if (batchSize === 0) {
          this.noMoreResults = true;
          
        } else {
          this.usersList = [...this.usersList, ...users];
          this.stateService.setCursor(cursor + users.length);
        }
      }
    });
  }
}

