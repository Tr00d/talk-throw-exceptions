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
  AccountProvisioningException,
  BusinessException,
  ContractGenerationException,
  EmployeeRegistrationException,
  PayrollEnrollmentException,
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

describe("Onboarding", () => {
  let employees: EmployeeRepository;
  let hr: HrSystem;
  let it_: ItProvisioning;
  let payroll: Payroll;
  let onboarding: Onboarding;

  beforeEach(() => {
    employees = { register: vi.fn().mockReturnValue(RegisteredEmployee) };
    hr = { generateContract: vi.fn().mockReturnValue(GeneratedContract) };
    it_ = { provisionAccount: vi.fn().mockReturnValue(ProvisionedAccount) };
    payroll = { enroll: vi.fn().mockReturnValue(EnrollmentResult) };
    onboarding = new Onboarding(employees, hr, it_, payroll);
  });

  // ── Happy path ────────────────────────────────────────────────────────────

  it("should return enrollment when all steps succeed", () => {
    vi.mocked(employees.register).mockReturnValue(RegisteredEmployee);
    vi.mocked(hr.generateContract).mockReturnValue(GeneratedContract);
    vi.mocked(it_.provisionAccount).mockReturnValue(ProvisionedAccount);
    vi.mocked(payroll.enroll).mockReturnValue(EnrollmentResult);

    const result = onboarding.onboardNewHire(Offer);

    expect(result).toEqual(EnrollmentResult);
  });

  // ── Failure paths ─────────────────────────────────────────────────────────

  it("should throw business exception when employee registration fails", () => {
    vi.mocked(employees.register).mockImplementation(() => {
      throw new EmployeeRegistrationException("Duplicate employee record");
    });

    expect(() => onboarding.onboardNewHire(Offer)).toThrow(
      new BusinessException("Duplicate employee record"),
    );
  });

  it("should throw business exception when contract generation fails", () => {
    vi.mocked(employees.register).mockReturnValue(RegisteredEmployee);
    vi.mocked(hr.generateContract).mockImplementation(() => {
      throw new ContractGenerationException("Missing salary band");
    });

    expect(() => onboarding.onboardNewHire(Offer)).toThrow(
      new BusinessException("Missing salary band"),
    );
  });

  it("should throw business exception when account provisioning fails", () => {
    vi.mocked(employees.register).mockReturnValue(RegisteredEmployee);
    vi.mocked(hr.generateContract).mockReturnValue(GeneratedContract);
    vi.mocked(it_.provisionAccount).mockImplementation(() => {
      throw new AccountProvisioningException("Login already taken");
    });

    expect(() => onboarding.onboardNewHire(Offer)).toThrow(
      new BusinessException("Login already taken"),
    );
  });

  it("should throw business exception when payroll enrollment fails", () => {
    vi.mocked(employees.register).mockReturnValue(RegisteredEmployee);
    vi.mocked(hr.generateContract).mockReturnValue(GeneratedContract);
    vi.mocked(it_.provisionAccount).mockReturnValue(ProvisionedAccount);
    vi.mocked(payroll.enroll).mockImplementation(() => {
      throw new PayrollEnrollmentException("Payroll system unavailable");
    });

    expect(() => onboarding.onboardNewHire(Offer)).toThrow(
      new BusinessException("Payroll system unavailable"),
    );
  });
});
