using EmployeeOnboarding.Models;

namespace EmployeeOnboarding.Externals;

public interface IEmployeeRepository
{
    Employee Register(AcceptedOffer offer);
}

public interface IHrSystem
{
    Contract GenerateContract(Employee employee);
}

public interface IItProvisioning
{
    Account ProvisionAccount(Contract contract);
}

public interface IPayroll
{
    OnboardingResult Enroll(Account account);
}