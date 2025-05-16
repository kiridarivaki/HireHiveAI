import { Resume } from "@shared/models/resume.model";

export interface GetAllResumesResponse extends Array<Resume> {}

export interface GetResumeInfoPayload {
    fileName?: string,
    fileSize?: number,
    contentType?: string,
    updatedAt: Date, 
}

export interface UploadResumePayload {
    file: File
}

export interface UpdateResumePayload {
    file?: File | null;
}