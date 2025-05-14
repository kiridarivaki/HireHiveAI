export interface Resume {
    id: string;
    file: File;
    fileName?: string;
    fileSize?: number;
    contentType?: string;
    updatedAt: Date; 
    userId: string; 
}