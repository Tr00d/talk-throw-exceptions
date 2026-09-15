import * as E from "fp-ts/Either";
import { pipe } from "fp-ts/function";
import type {
  EmployeeRepository,
  HrSystem,
  ItProvisioning,
  Payroll,
} from "./externals/ports.js";
import type {
  AcceptedOffer,
  OnboardingError,
  OnboardingResult,
} from "./models.js";

export class Onboarding {
  constructor(
    private readonly employees: EmployeeRepository,
    private readonly hr: HrSystem,
    private readonly it: ItProvisioning,
    private readonly payroll: Payroll,
  ) {}

  onboardNewHire(offer: AcceptedOffer): E.Either<OnboardingError, OnboardingResult> {
    return pipe(
      this.employees.register(offer),
      E.flatMap((employee) => this.hr.generateContract(employee)),
      E.flatMap((contract) => this.it.provisionAccount(contract)),
      E.flatMap((account) => this.payroll.enroll(account)),
    );
  }
}
