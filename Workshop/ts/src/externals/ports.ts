import type { Either } from "fp-ts/Either";
import type {
  AcceptedOffer,
  Account,
  Contract,
  Employee,
  OnboardingError,
  OnboardingResult,
} from "../models.js";

export interface EmployeeRepository {
  register(offer: AcceptedOffer): Either<OnboardingError, Employee>;
}

export interface HrSystem {
  generateContract(employee: Employee): Either<OnboardingError, Contract>;
}

export interface ItProvisioning {
  provisionAccount(contract: Contract): Either<OnboardingError, Account>;
}

export interface Payroll {
  enroll(account: Account): Either<OnboardingError, OnboardingResult>;
}
