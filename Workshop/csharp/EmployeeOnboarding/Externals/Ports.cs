using EmployeeOnboarding.Models;

namespace EmployeeOnboarding.Externals;

public interface ICandidateRepository
{
    Candidate? FindApprovedCandidate(Department dept);
}

public interface IHrSystem
{
    Contract? GenerateContract(Candidate candidate);
}

public interface IItProvisioning
{
    Account? ProvisionAccount(string email);
}
