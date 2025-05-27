import { Component, OnInit } from '@angular/core';
import { CandidateCardComponent } from '../components/candidate-card/candidate-card.component';
import { CommonModule } from '@angular/common';
import { JobStateService } from '@shared/services/job-state.service';
import { AdminClientService } from 'src/app/client/services/admin-client.service';
import { ErrorService } from '@shared/services/error.service';
import { AssessResponse, SortDataPayload } from 'src/app/client/models/admin-client.model';
import { finalize, Observable } from 'rxjs';
import { LoaderService } from '@shared/services/loader.service';
import { AppButtonComponent } from '@shared/components/button/button.component';
import { SortParams } from '@shared/constants/sort-parameters';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { AppSelectComponent } from '@shared/components/select/select.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-candidates-match',
  templateUrl: './candidate-matches.page.html',
  imports: [CandidateCardComponent, CommonModule, AppButtonComponent, MatIconModule, FormsModule, MatSelectModule, AppSelectComponent]
})
export class CandidateMatchesComponent implements OnInit {
  usersList: Array<AssessResponse> = [];
  noMoreResults: boolean = false;
  loading$!: Observable<boolean>;
  
  sortOptions = Object.entries(SortParams).map(([key, label]) => ({
    value: key,
    label: label
  }));
  sortField: string | null = null;
  sortOrder: 'asc' | 'desc' = 'asc';

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
      this.router.navigate(['/admin/job-profile'])
      return;
    }

    this.loading$ = this.loaderService.loading$;
    this.usersList = [];
    this.stateService.setCursor(0);
    this.fetchNextBatch();
  }

  sortUsers() {
    if (!this.sortField || !this.sortOrder) return;

    const sortDataPayload: SortDataPayload = {
      assessmentData: this.usersList,
      orderByField: this.sortField,
      sortOrder: this.sortOrder
    };

    this.adminService.sort(sortDataPayload).subscribe({
      next: (sorted) => {
        this.usersList = sorted;
      }
    });
  }

  onSortFieldChange() {
    if (this.sortField) {
      this.sortUsers();
    }
  }

  toggleSortOrder() {
    this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    if (this.sortField) {
      this.sortUsers();
    }
  }

  fetchNextBatchMock() {
    const assessmentData = this.stateService.getAssessmentData();
    const cursor = this.stateService.getCursor() || 0;

    if (!assessmentData) return;

    this.loaderService.show();

      const batchSize = 5;

      const users: AssessResponse[] = [];

    for (let i = 0; i < batchSize; i++) {
      const id = cursor + i + 1;
      users.push({
        id,
        firstName: `First${id}`,
        lastName: `Last${id}`,
        matchPercentage: Math.floor(Math.random() * 101),
      } as unknown as AssessResponse);

      if (users.length === 0) {
        this.noMoreResults = true;
      } else {
        this.usersList = [...this.usersList, ...users];
        this.stateService.setCursor(cursor + batchSize);
      }

      this.loaderService.hide();
    }
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
      finalize(() => this.loaderService.hide())
      ).subscribe({
        next: (users) => {
          if (!users || users.length === 0) {
            this.noMoreResults = true;
            return;
          }

          this.usersList = [...this.usersList, ...users];
          this.stateService.setCursor(cursor + users.length);

          this.sortUsers();
        }
    });
  }
}

