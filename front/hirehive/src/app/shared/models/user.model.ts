import { EmploymentStatus } from "@shared/constants/employment-options";
import { UserRole } from "./auth.model";
import { JobType } from "@shared/constants/job-types";

export interface User{
    roles: Array<UserRole>,
    emailConfirmed: boolean,
    email: string;
    id: string;
    firstName: string;
    lastName: string;
    employmentStatus?: EmploymentStatus;
    jobTypes?: Array<JobType>;
    resumeId?: string;
}