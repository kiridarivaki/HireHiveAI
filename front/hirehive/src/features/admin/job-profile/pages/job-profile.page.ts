import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSliderModule } from '@angular/material/slider';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { AppSelectComponent } from '@shared/components/select/select.component';
import { AppButtonComponent } from '@shared/components/button/button.component';
import { CriteriaSliderComponent } from '../components/criteria-slider/criteria-slider.component';
import { JobType } from '@shared/constants/job-types';
import { AssessmentCriteria } from '@shared/constants/assessment-criteria';
import { AssessmentDataPayload } from 'src/app/client/models/admin-client.model';
import { AdminClientService } from 'src/app/client/services/admin-client.service';
import { Router } from '@angular/router';
import { JobStateService } from '@shared/services/state.service';

@Component({
  selector: 'app-job-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatSliderModule, AppSelectComponent, AppButtonComponent, CriteriaSliderComponent],
  templateUrl: './job-profile.page.html',
  styleUrl: './job-profile.page.css'
})
export class JobProfileComponent {
  jobForm = new FormGroup({
    jobDescription: new FormControl(''),
    jobType: new FormControl(''),
    education: new FormControl(50),
    skills: new FormControl(50),
    experience: new FormControl(50),
  });

  jobTypes = Object.entries(JobType).map(([key, label]) => ({ value: key, label }));
  criteria = Object.entries(AssessmentCriteria).map(([key, label]) => ({ key, label }));

  constructor(
    private adminService: AdminClientService, 
    private stateService: JobStateService,
    private router: Router
  ){}

  onSubmit() {
      if (this.jobForm.valid) {
        const jobForm = this.jobForm.value;
        const criteriaWeights = this.criteria.map(c => {
          const value = this.jobForm.get(c.key)?.value;
          return typeof value === 'number' ? value : 0;
        });

        const assessmentData: AssessmentDataPayload = {
          criteriaWeights: criteriaWeights,
          jobDescription: jobForm.jobDescription!,
          jobType: jobForm.jobType as JobType
        }
        this.stateService.setJobData(assessmentData);
        this.router.navigate(['/admin/results']);
      }
  }
}
