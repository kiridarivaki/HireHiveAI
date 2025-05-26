import { EmploymentStatus } from "@shared/constants/employment-options";
import { JobType } from "@shared/constants/job-types";
import { User } from "@shared/models/user.model";

export interface GetAllUsersResponse extends Array<User> {}

export interface GetUserInfoPayload {
    email: string,
    firstName: string,
    lastName: string,
    employmentStatus?: EmploymentStatus,
    jobTypes: Array<JobType>,
    resumeId?: string | null
}

export interface UpdateUserPayload {
    firstName?: string | null,
    lastName?: string | null,
    employmentStatus?: EmploymentStatus | null
    jobTypes?: Array<JobType> | null
}
