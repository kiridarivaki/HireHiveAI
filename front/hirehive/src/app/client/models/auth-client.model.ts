export interface RegisterFormParameters{
    email: string,
    firstName: string,
    lastName: string,
    employmentStatus: string,
    password: string;
    confirmPassword: string;
}

export interface LoginFormParameters{
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