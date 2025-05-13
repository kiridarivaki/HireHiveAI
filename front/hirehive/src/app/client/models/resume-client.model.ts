import { Resume } from "@shared/models/resume";

export interface GetAllResumesResponse extends Array<Resume> {}

export interface GetResumeInfoPayload {
    fileName?: string,
    fileSize?: number,
    contentType?: string,
    updatedAt: Date, 
}

export interface UploadFormParameters {
    file: File
    userId: string 
}

export interface UpdateFormParameters {
    file?: File | null;
}