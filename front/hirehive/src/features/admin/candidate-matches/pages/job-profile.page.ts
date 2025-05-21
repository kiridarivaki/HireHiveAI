import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import {MatSliderModule} from '@angular/material/slider';
import { AppSelectComponent } from '@shared/components/select/select.component';

type CriteriaKey = 'education' | 'skills' | 'experience';

@Component({
  selector: 'app-job-config',
  templateUrl: './job-profile.page.html',
  imports: [MatFormFieldModule, 
    MatSliderModule,
    AppSelectComponent,
    FormsModule,
    CommonModule]
})
export class JobProfileComponent {
  jobDescription: string = '';
  jobType: string = '';
    educationWeight: number = 50;
  skillsWeight: number = 50;
  experienceWeight: number = 50;

  criteriaKeys: CriteriaKey[] = ['education', 'skills', 'experience'];

  jobTypes = ['Back End Developer', 'Front End Developer', 'Data Engineer', 'IT Project Manager'];

  submitConfig() {
    const config = {
      jobDescription: this.jobDescription,
      jobType: this.jobType,
    };
    console.log('Submitted Job Config:', config);
  }
}