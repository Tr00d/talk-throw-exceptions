export interface AcceptedOffer {
  readonly name: string;
  readonly email: string;
  readonly department: string;
  readonly startDate: Date;
}

export interface Employee {
  readonly id: number;
  readonly name: string;
  readonly email: string;
}

export interface Contract {
  readonly id: number;
  readonly employeeId: number;
  readonly startDate: Date;
}

export interface Account {
  readonly employeeId: number;
  readonly login: string;
}

export interface OnboardingResult {
  readonly employeeId: number;
  readonly login: string;
  readonly enrolledAt: Date;
}

export interface EmployeeRegistrationError {
  readonly _tag: "EmployeeRegistrationError";
  readonly reason: string;
}

export interface ContractGenerationError {
  readonly _tag: "ContractGenerationError";
  readonly reason: string;
}

export interface AccountProvisioningError {
  readonly _tag: "AccountProvisioningError";
  readonly reason: string;
}

export interface PayrollEnrollmentError {
  readonly _tag: "PayrollEnrollmentError";
  readonly reason: string;
}

export type OnboardingError =
  | EmployeeRegistrationError
  | ContractGenerationError
  | AccountProvisioningError
  | PayrollEnrollmentError;

export const employeeRegistrationError = (reason: string): EmployeeRegistrationError => ({
  _tag: "EmployeeRegistrationError",
  reason,
});

export const contractGenerationError = (reason: string): ContractGenerationError => ({
  _tag: "ContractGenerationError",
  reason,
});

export const accountProvisioningError = (reason: string): AccountProvisioningError => ({
  _tag: "AccountProvisioningError",
  reason,
});

export const payrollEnrollmentError = (reason: string): PayrollEnrollmentError => ({
  _tag: "PayrollEnrollmentError",
  reason,
});
