import { EmploymentStatus } from "@shared/constants/employment-options";
import { UserRole } from "./auth.model";

export interface User{
    role: UserRole
    email: string;
    id: string;
    firstName: string;
    lastName: string;
    employmentStatus: EmploymentStatus;
    resumeId: string;
}