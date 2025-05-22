import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay } from 'rxjs/operators';
import { GetUserInfoPayload } from 'src/app/client/models/user-client.model';
import { CandidateCardComponent } from '../components/candidate-card/candidate-card.component';
import { CommonModule } from '@angular/common';
import { JobStateService } from '@shared/services/state.service';
import { AdminClientService } from 'src/app/client/services/admin-client.service';
import { ErrorService } from '@shared/services/error.service';

@Component({
  selector: 'app-candidates-match',
  templateUrl: './candidate-matches.page.html',
  imports: [CandidateCardComponent, CommonModule]
})
export class CandidateMatchesComponent implements OnInit {
  candidateIds = ['1', '2', '3'];
  matchPercentages = ['92', '76', '88'];

  candidates: { userInfo: GetUserInfoPayload; matchPercentage: string }[] = [];

  constructor(
    private adminService: AdminClientService,
    private stateService: JobStateService, 
    private errorService: ErrorService
  ){}

  ngOnInit() {
    this.getUsersByIds(this.candidateIds).subscribe(users => {
      this.candidates = users.map((user, index) => ({
        userInfo: user,
        matchPercentage: this.matchPercentages[index]
      })); //change to user service 

      const assessmentData= this.stateService.getJobData();
      if (assessmentData){
        this.adminService.assess(assessmentData);
        //add loader
      }else{
        this.errorService.showError('No job description was given.')
      }
    });
  }

getUsersByIds(ids: string[]): Observable<GetUserInfoPayload[]> {
  const response: GetUserInfoPayload[] = [
    { firstName: 'Alice', lastName: 'Smith', email: 'alice@example.com' },
    { firstName: 'Bob', lastName: 'Johnson', email: 'bob@example.com' },
    { firstName: 'Charlie', lastName: 'Lee', email: 'charlie@example.com' }
  ];

  return of(response).pipe(delay(500));
}
}

