using EmployeeOnboarding.Models;
using LanguageExt;

namespace EmployeeOnboarding.Externals;

public interface IEmployeeRepository
{
    Either<Error, Employee> Register(AcceptedOffer offer);
}

public interface IHrSystem
{
    Either<Error, Contract> GenerateContract(Employee employee);
}

public interface IItProvisioning
{
    Either<Error, Account> ProvisionAccount(Contract contract);
}

public interface IPayroll
{
    Either<Error, OnboardingResult> Enroll(Account account);
}