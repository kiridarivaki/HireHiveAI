import { EmploymentStatus } from "@shared/constants/employment-options";
import { JobType } from "@shared/constants/job-types";

export interface AssessmentDataPayload {
    jobDescription: string,
    jobType: JobType,
    criteriaWeights: Array<number>,
    cursor?: number
}

export interface AssessResponse {
    matchPercentage: string,
    userId: string,
    email: string,
    firstName: string,
    lastName: string,
    employmentStatus: EmploymentStatus
}