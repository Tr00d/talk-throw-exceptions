import { beforeEach, describe, expect, it, vi } from "vitest";
import { Onboarding } from "../src/onboarding.js";
import type {
  CandidateRepository,
  HrSystem,
  ItProvisioning,
} from "../src/externals/ports.js";
import type {
  Account,
  Candidate,
  Contract,
  Department,
} from "../src/models.js";

// ── Test data ─────────────────────────────────────────────────────────────
const Engineering: Department = { name: "Engineering" };
const Marketing: Department = { name: "Marketing" };

const Alice: Candidate = { name: "Alice", email: "alice@corp.com" };
const Bob: Candidate = { name: "Bob", email: "bob@corp.com" };

const AliceContract: Contract = { candidateName: "Alice", email: "alice@corp.com" };
const BobContract: Contract = { candidateName: "Bob", email: "bob@corp.com" };

const AliceAccount: Account = { name: "Alice", login: "alice.corp" };
const BobAccount: Account = { name: "Bob", login: "bob.corp" };

describe("Onboarding", () => {
  let candidates: CandidateRepository;
  let hr: HrSystem;
  let it_: ItProvisioning;
  let onboarding: Onboarding;

  beforeEach(() => {
    candidates = { findApprovedCandidate: vi.fn().mockReturnValue(null) };
    hr = { generateContract: vi.fn().mockReturnValue(null) };
    it_ = { provisionAccount: vi.fn().mockReturnValue(null) };
    onboarding = new Onboarding(candidates, hr, it_);
  });

  // ── Happy path ────────────────────────────────────────────────────────────

  it("should onboard candidate when all steps succeed", () => {
    vi.mocked(candidates.findApprovedCandidate).mockReturnValue(Alice);
    vi.mocked(hr.generateContract).mockReturnValue(AliceContract);
    vi.mocked(it_.provisionAccount).mockReturnValue(AliceAccount);

    const result = onboarding.processNewHires(Engineering);

    expect(result).toEqual(["Onboarded: Alice is ready to start!"]);
  });

  it("should process multiple departments", () => {
    vi.mocked(candidates.findApprovedCandidate).mockImplementation((dept) =>
      dept === Engineering ? Alice : dept === Marketing ? Bob : null,
    );
    vi.mocked(hr.generateContract).mockImplementation((candidate) =>
      candidate === Alice ? AliceContract : candidate === Bob ? BobContract : null,
    );
    vi.mocked(it_.provisionAccount).mockImplementation((email) =>
      email === Alice.email ? AliceAccount : email === Bob.email ? BobAccount : null,
    );

    const result = onboarding.processNewHires(Engineering, Marketing);

    expect(result).toHaveLength(2);
    expect(result).toContain("Onboarded: Alice is ready to start!");
    expect(result).toContain("Onboarded: Bob is ready to start!");
  });

  // ── Failure paths — currently SILENT ─────────────────────────────────────
  // These tests document what the code does TODAY.
  // Your goal: make each failure case explicit in the output.

  it("should throw when no departments provided", () => {
    expect(() => onboarding.processNewHires()).toThrow("No departments to process!");
  });

  it("should silently skip when no approved candidate", () => {
    vi.mocked(candidates.findApprovedCandidate).mockReturnValue(null);

    const result = onboarding.processNewHires(Engineering);

    // ⚠️ Silent! The caller has no idea why Engineering was skipped.
    expect(result).toEqual([]);
  });

  it("should silently skip when contract generation fails", () => {
    vi.mocked(candidates.findApprovedCandidate).mockReturnValue(Alice);
    vi.mocked(hr.generateContract).mockReturnValue(null);

    const result = onboarding.processNewHires(Engineering);

    // ⚠️ Silent! Indistinguishable from the case above.
    expect(result).toEqual([]);
  });

  it("should silently skip when it provisioning fails", () => {
    vi.mocked(candidates.findApprovedCandidate).mockReturnValue(Alice);
    vi.mocked(hr.generateContract).mockReturnValue(AliceContract);
    vi.mocked(it_.provisionAccount).mockReturnValue(null);

    const result = onboarding.processNewHires(Engineering);

    // ⚠️ Silent! Three different failures, one indistinguishable outcome.
    expect(result).toEqual([]);
  });
});
