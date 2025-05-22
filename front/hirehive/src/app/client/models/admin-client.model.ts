import { JobType } from "@shared/constants/job-types";

export interface AssessmentDataPayload {
    criteriaWeights: Array<number>,
    jobDescription: string,
    jobType: JobType,
    cursor?: number
}

export interface AssessResponse {
    matchPercentages: Array<string>,
    pageSize: number,
    pageNumber: number
}