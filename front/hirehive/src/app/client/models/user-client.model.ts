import { User } from "@shared/models/user.model";

export interface GetAllUsersResponse extends Array<User> {}

export interface GetUserInfoPayload {
    email: string,
    firstName: string,
    lastName: string,
    employmentStatus?: string,
    resumeId?: string | null
}

export interface GetMatchPercentagePayload {
    matchPercentage: string,
}

export interface UpdateUserPayload {
    firstName?: string | null,
    lastName?: string | null,
    employmentStatus?: string | null
}
