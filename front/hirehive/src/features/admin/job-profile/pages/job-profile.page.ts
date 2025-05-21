import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSliderModule } from '@angular/material/slider';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { AppSelectComponent } from '@shared/components/select/select.component';
import { AppButtonComponent } from '@shared/components/button/button.component';
import { CriteriaSliderComponent } from '../components/criteria-slider/criteria-slider.component';
import { JobType } from '@shared/constants/job-types';

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

  criteria = [
    { key: 'education', label: 'Education' },
    { key: 'skills', label: 'Skills' },
    { key: 'experience', label: 'Experience' },
  ];

  submitConfig() {
    console.log('Form value:', this.jobForm.value);
  }
}
