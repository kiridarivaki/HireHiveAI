export interface StoredAuth{
    accessToken: string;
    refreshToken: string;
    expiresIn: number;
}

export enum UserRole{
    candidate = 'Candidate',
    admin = 'Admin'
}