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
    jwtToken: string,
    refreshToken: string,
    expiresIn: Date
}