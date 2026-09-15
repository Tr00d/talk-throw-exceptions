import type {
  AcceptedOffer,
  Account,
  Contract,
  Employee,
  OnboardingResult,
} from "../models.js";

export interface EmployeeRepository {
  register(offer: AcceptedOffer): Employee;
}

export interface HrSystem {
  generateContract(employee: Employee): Contract;
}

export interface ItProvisioning {
  provisionAccount(contract: Contract): Account;
}

export interface Payroll {
  enroll(account: Account): OnboardingResult;
}
