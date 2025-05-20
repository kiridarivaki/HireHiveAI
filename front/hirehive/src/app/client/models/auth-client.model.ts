import { UserRole } from "@shared/models/auth.model";

export interface RegisterPayload{
    email: string,
    firstName: string,
    lastName: string,
    employmentStatus: string,
    password: string;
    confirmPassword: string;
}

export interface EmailConfirmationResponse{
    accessToken: string,
    refreshToken: string,
    expiresIn: number
}

export interface LoginPayload{
    email: string,
    password: string;
}

export interface LoginResponse{
    userId: string,
    tokenType: string,
    accessToken: string,
    refreshToken: string,
    expiresIn: number
}

export interface RefreshTokenResponse{
    acessToken: string,
    refreshToken: string,
    expiresIn: number
}

export interface GetInfoResponse{
    userId: string,
    roles: Array<UserRole>,
    email: string,
    firstName: string,
    lastName: string
}