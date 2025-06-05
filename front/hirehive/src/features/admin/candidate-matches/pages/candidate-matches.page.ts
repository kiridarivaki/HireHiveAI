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
import { trigger, state, style, transition, animate } from '@angular/animations';
import { EmploymentStatus } from '@shared/constants/employment-options';

@Component({
  selector: 'app-candidates-match',
  templateUrl: './candidate-matches.page.html',
  styleUrl: './candidate-matches.page.scss',
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
  sortField: string = 'None';
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
      this.router.navigate(['/admin/job-profile']);
      return;
    }

    this.loading$ = this.loaderService.loading$;
    this.usersList = [];
    this.stateService.setCursor(0);
    this.fetchNextBatch();
  }

  private updateUsersList(newUsers: AssessResponse[]) {
    this.usersList = [...this.usersList, ...newUsers];
    this.sortUsers();
  }

  sortUsers() {
    if (this.sortField === 'None') return;

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
        this.stateService.setCursor(cursor + users.length);
        this.updateUsersList(users);
      }
    });
  }
}
