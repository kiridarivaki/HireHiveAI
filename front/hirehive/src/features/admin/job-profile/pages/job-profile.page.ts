import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators, FormArray, FormsModule } from '@angular/forms';
import { AppSelectComponent } from '@shared/components/select/select.component';
import { AppButtonComponent } from '@shared/components/button/button.component';
import { CriteriaSliderComponent } from '../components/criteria-slider/criteria-slider.component';
import { JobType } from '@shared/constants/job-types';
import { AssessmentDataPayload } from 'src/app/client/models/admin-client.model';
import { Router } from '@angular/router';
import { JobStateService } from '@shared/services/job-state.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-job-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    AppSelectComponent,
    AppButtonComponent,
    CriteriaSliderComponent
  ],
  templateUrl: './job-profile.page.html',
  styleUrls: ['./job-profile.page.scss']
})
export class JobProfileComponent {
  jobForm = new FormGroup({
    jobDescription: new FormControl('', Validators.required),
    jobType: new FormControl('', Validators.required),
    criteria: new FormArray<FormGroup>([])
  });

  newCriterionLabel = '';

  jobTypes = Object.entries(JobType).map(([key, label], index) => ({
    value: index.toString(),
    label: label as string
  }));

  constructor(
    private stateService: JobStateService,
    private router: Router
  ) {}

  get criteriaArray() {
    return this.jobForm.get('criteria') as FormArray<FormGroup>;
  }

  createCriterion(label: string, weight = 50): FormGroup {
    return new FormGroup({
      label: new FormControl(label, Validators.required),
      value: new FormControl(weight, Validators.required)
    });
  }

  addCriterion() {
    const label = this.newCriterionLabel.trim();
    if (!label) return;
    this.criteriaArray.push(this.createCriterion(label));
    this.newCriterionLabel = '';
  }

  removeCriterion(index: number) {
    this.criteriaArray.removeAt(index);
  }

  onSubmit() {
    if (this.jobForm.invalid) {
      this.jobForm.markAllAsTouched();
      return;
    }

    const jobForm = this.jobForm.value;

    const criteria = this.criteriaArray.controls.map(c => {
      const label = c.get('label')?.value;
      return typeof label === 'string' ? label : '';
    });

    const criteriaWeights = this.criteriaArray.controls.map(c => {
      const value = c.get('value')?.value;
      return typeof value === 'number' ? value : 0;
    });

    const jobTypeValue = Number(jobForm.jobType) as unknown as JobType;

    const assessmentData: AssessmentDataPayload = {
      criteria,
      criteriaWeights,
      jobDescription: jobForm.jobDescription!,
      jobType: jobTypeValue,
      cursor: 0
    };

    this.stateService.setAssessmentData(assessmentData);
    this.router.navigate(['/admin/results']);
  }
}