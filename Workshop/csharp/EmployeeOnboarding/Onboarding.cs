using EmployeeOnboarding.Externals;
using EmployeeOnboarding.Models;

namespace EmployeeOnboarding;

public class Onboarding(
    ICandidateRepository candidates,
    IHrSystem hr,
    IItProvisioning it)
{
    public List<string> ProcessNewHires(params Department[] departments)
    {
        if (!departments.Any())
            throw new ArgumentException("No departments to process!");

        var results = new List<string>();

        foreach (var dept in departments)
        {
            var candidate = candidates.FindApprovedCandidate(dept);
            if (candidate is not null)
            {
                var contract = hr.GenerateContract(candidate);
                if (contract is not null)
                {
                    var account = it.ProvisionAccount(candidate.Email);
                    if (account is not null)
                        results.Add($"Onboarded: {account.Name} is ready to start!");
                }
            }
        }

        return results;
    }
}
