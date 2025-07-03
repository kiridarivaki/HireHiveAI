import { EmploymentStatus } from "@shared/constants/employment-options";
import { JobType } from "@shared/constants/job-types";
import { UserRole } from "@shared/models/auth.model";

export interface RegisterPayload{
    email: string,
    firstName: string,
    lastName: string,
    employmentStatus: EmploymentStatus,
    jobTypes: Array<JobType>,
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

export interface RegisterResponse{
    userId: string,
    tokenType: string,
    accessToken: string,
    refreshToken: string,
    expiresIn: number
}

export interface RefreshTokenResponse{
    accessToken: string,
    refreshToken: string,
    expiresIn: number
}

export interface RefreshTokenPayload{
    accessToken?: string,
    refreshToken?: string
}

export interface GetInfoResponse{
    roles: Array<UserRole>,
    email: string,
    firstName: string,
    lastName: string,
    emailConfirmed: boolean
}

export interface EmailConfirmationPayload{
    confirmationToken: string,
    email: string
}

export interface EmailConfirmationResendPayload{
    email: string
}