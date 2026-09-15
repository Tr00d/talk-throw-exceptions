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

export class BusinessException extends Error {}

export class EmployeeRegistrationException extends Error {}

export class ContractGenerationException extends Error {}

export class AccountProvisioningException extends Error {}

export class PayrollEnrollmentException extends Error {}
