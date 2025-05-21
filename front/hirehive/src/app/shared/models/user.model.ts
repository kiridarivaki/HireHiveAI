import { EmploymentStatus } from "@shared/constants/employment-options";
import { UserRole } from "./auth.model";

export interface User{
    roles: Array<UserRole>,
    emailConfirmed: boolean,
    email: string;
    id: string;
    firstName: string;
    lastName: string;
    employmentStatus?: EmploymentStatus;
    resumeId?: string;
}