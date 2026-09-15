import type { Account, Candidate, Contract, Department } from "../models.js";

export interface CandidateRepository {
  findApprovedCandidate(dept: Department): Candidate | null;
}

export interface HrSystem {
  generateContract(candidate: Candidate): Contract | null;
}

export interface ItProvisioning {
  provisionAccount(email: string): Account | null;
}
