export interface StoredAuth{
    accessToken: string;
    refreshToken: string;
    expiresIn: string;
}

export enum UserRole{
    candidate = 'Candidate',
    admin = 'Admin'
}