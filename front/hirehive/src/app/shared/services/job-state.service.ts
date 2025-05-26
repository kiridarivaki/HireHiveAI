import { Injectable } from "@angular/core";
import { AssessmentDataPayload } from "src/app/client/models/admin-client.model";

@Injectable({ providedIn: 'root' })
export class JobStateService {
  private cursor: number = 0;

  setAssessmentData(data: AssessmentDataPayload): void {
    localStorage.setItem('job-data', JSON.stringify(data));
  }

  getAssessmentData(): AssessmentDataPayload | null {
    const raw = localStorage.getItem('job-data');
    return raw ? JSON.parse(raw) : null;
  }

  clearAssessmentData(): void {
    localStorage.removeItem('job-data');
  }

  setCursor(cursor: number): void {
    localStorage.setItem('cursor', cursor.toString());
  }

  getCursor(): number {
    const raw = localStorage.getItem('cursor');
    return raw ? parseInt(raw, 10) : 0;
  }

  clearCursor(): void {
    localStorage.removeItem('cursor');
  }

  incrementCursor(by: number): void {
    const current = this.getCursor();
    const newCursor = current + by;
    this.setCursor(newCursor);
  }
}
