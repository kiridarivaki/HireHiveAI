import { JobType } from "@shared/constants/job-types";

export interface AssessmentDataPayload {
    criteria: Array<string>,
    criteriaWeights: Array<number>,
    jobDescription: string,
    jobType: JobType,
    cursor?: number
}

export interface AssessResponse {
    matchPercentage: string,
    userId: string,
    email: string,
    firstName: string,
    lastName: string,
    explanation: string
}

export interface SortDataPayload {
    sortOrder: string,
    orderByField: string,
    assessmentData: Array<AssessResponse>
}

export interface SortResponse {
    matchPercentage: string,
    userId: string,
    email: string,
    firstName: string,
    lastName: string,
    explanation: string
}