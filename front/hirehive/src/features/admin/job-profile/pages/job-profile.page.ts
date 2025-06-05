import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSliderModule } from '@angular/material/slider';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { AppSelectComponent } from '@shared/components/select/select.component';
import { AppButtonComponent } from '@shared/components/button/button.component';
import { CriteriaSliderComponent } from '../components/criteria-slider/criteria-slider.component';
import { JobType } from '@shared/constants/job-types';
import { AssessmentCriteria } from '@shared/constants/assessment-criteria';
import { AssessmentDataPayload } from 'src/app/client/models/admin-client.model';
import { Router } from '@angular/router';
import { JobStateService } from '@shared/services/job-state.service';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-job-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatSliderModule, AppSelectComponent, AppButtonComponent, CriteriaSliderComponent, MatFormFieldModule],
  templateUrl: './job-profile.page.html',
  styleUrl: './job-profile.page.scss'
})
export class JobProfileComponent {
  jobForm = new FormGroup({
    jobDescription: new FormControl('', Validators.required),
    jobType: new FormControl('', Validators.required), 
    education: new FormControl(50),
    skills: new FormControl(50),
    experience: new FormControl(50),
  });

  jobTypes = Object.entries(JobType).map(([key, label], index) => ({
    value: index.toString(),  
    label: label as string
  }));

  criteria = Object.entries(AssessmentCriteria).map(([key, label], index) => ({
    key,
    label
  }));

  constructor(
    private stateService: JobStateService,
    private router: Router
  ){}

  onSubmit() {
    if (this.jobForm.invalid) {
      this.jobForm.markAllAsTouched();
      console.log(this.jobForm.value)
      return;
    }

    const jobForm = this.jobForm.value;
    const criteriaWeights = this.criteria.map(c => {
      const value = this.jobForm.get(c.key)?.value;
      return typeof value === 'number' ? value : 0;
    });

    const jobTypeValue = Number(jobForm.jobType) as unknown as JobType;

    const assessmentData: AssessmentDataPayload = {
      criteriaWeights,
      jobDescription: jobForm.jobDescription!,
      jobType: jobTypeValue, 
      cursor: 0
    };
    this.stateService.setAssessmentData(assessmentData);
    this.router.navigate(['/admin/results']);
  }
}
