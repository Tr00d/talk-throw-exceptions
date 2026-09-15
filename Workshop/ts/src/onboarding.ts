import type {
  EmployeeRepository,
  HrSystem,
  ItProvisioning,
  Payroll,
} from "./externals/ports.js";
import type { AcceptedOffer, OnboardingResult } from "./models.js";
import {
  AccountProvisioningException,
  BusinessException,
  ContractGenerationException,
  EmployeeRegistrationException,
  PayrollEnrollmentException,
} from "./models.js";

export class Onboarding {
  constructor(
    private readonly employees: EmployeeRepository,
    private readonly hr: HrSystem,
    private readonly it: ItProvisioning,
    private readonly payroll: Payroll,
  ) {}

  onboardNewHire(offer: AcceptedOffer): OnboardingResult {
    try {
      const employee = this.employees.register(offer);
      const contract = this.hr.generateContract(employee);
      const account = this.it.provisionAccount(contract);
      const enrollment = this.payroll.enroll(account);
      return enrollment;
    } catch (e) {
      if (e instanceof EmployeeRegistrationException) {
        throw new BusinessException(e.message);
      }
      if (e instanceof ContractGenerationException) {
        throw new BusinessException(e.message);
      }
      if (e instanceof AccountProvisioningException) {
        throw new BusinessException(e.message);
      }
      if (e instanceof PayrollEnrollmentException) {
        throw new BusinessException(e.message);
      }
      throw e;
    }
  }
}
