import type {
  CandidateRepository,
  HrSystem,
  ItProvisioning,
} from "./externals/ports.js";
import type { Department } from "./models.js";

export class Onboarding {
  constructor(
    private readonly candidates: CandidateRepository,
    private readonly hr: HrSystem,
    private readonly it: ItProvisioning,
  ) {}

  processNewHires(...departments: Department[]): string[] {
    if (departments.length === 0) {
      throw new Error("No departments to process!");
    }

    const results: string[] = [];

    for (const dept of departments) {
      const candidate = this.candidates.findApprovedCandidate(dept);
      if (candidate !== null) {
        const contract = this.hr.generateContract(candidate);
        if (contract !== null) {
          const account = this.it.provisionAccount(candidate.email);
          if (account !== null) {
            results.push(`Onboarded: ${account.name} is ready to start!`);
          }
        }
      }
    }

    return results;
  }
}
