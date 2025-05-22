import { Injectable } from "@angular/core";
import { AssessmentDataPayload } from "src/app/client/models/admin-client.model";

@Injectable({ providedIn: 'root' })
export class JobStateService {
  private assessmentData: AssessmentDataPayload | null = null;

  setJobData(data: AssessmentDataPayload) {
    this.assessmentData = data;
  }

  getJobData(): AssessmentDataPayload | null {
    return this.assessmentData;
  }

  clearJobData() {
    this.assessmentData = null;
  }
}
