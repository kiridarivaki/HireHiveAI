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
    expiresIn: Date
}

export interface LoginPayload{
    email: string,
    password: string;
}

export interface LoginResponse{
    accessToken: string,
    refreshToken: string,
    expiresIn: Date
}

export interface RefreshTokenResponse{
    acessToken: string,
    refreshToken: string,
    expiresIn: Date
}