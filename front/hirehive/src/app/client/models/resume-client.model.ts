import { JobType } from "@shared/constants/job-types";
import { Resume } from "@shared/models/resume.model";

export interface GetAllResumesResponse extends Array<Resume> {}

export interface GetResumeInfoPayload {
    fileName?: string,
    fileSize?: number,
    contentType?: string,
    updatedAt: Date 
}

export interface GetResumeUrlPayload {
    fileName?: string,
    sasUrl?: string 
}

export interface UpdateResumePayload {
    file?: File | null
}