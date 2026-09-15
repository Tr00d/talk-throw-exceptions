import * as E from "fp-ts/Either";
import { beforeEach, describe, expect, it, vi } from "vitest";
import { Onboarding } from "../src/onboarding.js";
import type {
  EmployeeRepository,
  HrSystem,
  ItProvisioning,
  Payroll,
} from "../src/externals/ports.js";
import type {
  AcceptedOffer,
  Account,
  Contract,
  Employee,
  OnboardingResult,
} from "../src/models.js";
import {
  accountProvisioningError,
  contractGenerationError,
  employeeRegistrationError,
  payrollEnrollmentError,
} from "../src/models.js";

// ── Test data ─────────────────────────────────────────────────────────────
const Offer: AcceptedOffer = {
  name: "Alice",
  email: "alice@corp.com",
  department: "Engineering",
  startDate: new Date(2024, 0, 15),
};

const RegisteredEmployee: Employee = { id: 1, name: "Alice", email: "alice@corp.com" };

const GeneratedContract: Contract = { id: 100, employeeId: 1, startDate: new Date(2024, 0, 15) };

const ProvisionedAccount: Account = { employeeId: 1, login: "alice.corp" };

const EnrollmentResult: OnboardingResult = {
  employeeId: 1,
  login: "alice.corp",
  enrolledAt: new Date(2024, 0, 15),
};

const registrationError = employeeRegistrationError("Duplicate employee record");
const contractError = contractGenerationError("Missing salary band");
const provisioningError = accountProvisioningError("Login already taken");
const enrollmentError = payrollEnrollmentError("Payroll system unavailable");

describe("Onboarding", () => {
  let employees: EmployeeRepository;
  let hr: HrSystem;
  let itProvisioning: ItProvisioning;
  let payroll: Payroll;
  let onboarding: Onboarding;

  beforeEach(() => {
    employees = { register: vi.fn().mockReturnValue(E.right(RegisteredEmployee)) };
    hr = { generateContract: vi.fn().mockReturnValue(E.right(GeneratedContract)) };
    itProvisioning = { provisionAccount: vi.fn().mockReturnValue(E.right(ProvisionedAccount)) };
    payroll = { enroll: vi.fn().mockReturnValue(E.right(EnrollmentResult)) };
    onboarding = new Onboarding(employees, hr, itProvisioning, payroll);
  });

  // ── Happy path ────────────────────────────────────────────────────────────

  it("should return onboarding result when all steps succeed", () => {
    const result = onboarding.onboardNewHire(Offer);

    expect(result).toEqual(E.right(EnrollmentResult));
  });

  // ── Failure paths ─────────────────────────────────────────────────────────

  it("should return registration error when registration fails", () => {
    vi.mocked(employees.register).mockReturnValue(E.left(registrationError));

    const result = onboarding.onboardNewHire(Offer);

    expect(result).toEqual(E.left(registrationError));
  });

  it("should return contract generation error when contract generation fails", () => {
    vi.mocked(hr.generateContract).mockReturnValue(E.left(contractError));

    const result = onboarding.onboardNewHire(Offer);

    expect(result).toEqual(E.left(contractError));
  });

  it("should return account provisioning error when account provisioning fails", () => {
    vi.mocked(itProvisioning.provisionAccount).mockReturnValue(E.left(provisioningError));

    const result = onboarding.onboardNewHire(Offer);

    expect(result).toEqual(E.left(provisioningError));
  });

  it("should return payroll enrollment error when payroll enrollment fails", () => {
    vi.mocked(payroll.enroll).mockReturnValue(E.left(enrollmentError));

    const result = onboarding.onboardNewHire(Offer);

    expect(result).toEqual(E.left(enrollmentError));
  });
});
