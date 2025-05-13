import { User } from '@shared/models/user'

export interface GetAllUsersResponse extends Array<User> {}

export interface GetUserInfoPayload {
    email: string,
    firstName: string,
    lastName: string,
    employmentStatus: string,
    resumeId?: string | null;
}

export interface UpdateFormParameters {
    firstName?: string | null,
    lastName?: string | null,
    employmentStatus?: string | null,
}
