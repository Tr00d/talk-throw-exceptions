using EmployeeOnboarding.Externals;
using EmployeeOnboarding.Models;
using LanguageExt;

namespace EmployeeOnboarding;

public class Onboarding(IEmployeeRepository employees, IHrSystem hr, IItProvisioning it, IPayroll payroll)
{
    public Either<Error, OnboardingResult> OnboardNewHire(AcceptedOffer offer) =>
        RegisterEmployee(offer)
            .Bind(GenerateContract())
            .Bind(ProvisionAccount())
            .Bind(EnrollAccount());

    private Func<Account, Either<Error, OnboardingResult>> EnrollAccount() => payroll.Enroll;

    private Func<Contract, Either<Error, Account>> ProvisionAccount() => it.ProvisionAccount;

    private Func<Employee, Either<Error, Contract>> GenerateContract() => hr.GenerateContract;

    private Either<Error, Employee> RegisterEmployee(AcceptedOffer offer) => employees.Register(offer);
}