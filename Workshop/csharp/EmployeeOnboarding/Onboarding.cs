using EmployeeOnboarding.Externals;
using EmployeeOnboarding.Models;

namespace EmployeeOnboarding;

public class Onboarding(IEmployeeRepository employees, IHrSystem hr, IItProvisioning it, IPayroll payroll)
{
    public OnboardingResult OnboardNewHire(AcceptedOffer offer)
    {
        try
        {
            var employee = employees.Register(offer);
            var contract = hr.GenerateContract(employee);
            var account = it.ProvisionAccount(contract);
            var enrollment = payroll.Enroll(account);
            return enrollment;
        }
        catch (EmployeeRegistrationException e)
        {
            throw new BusinessException(e.Message);
        }
        catch (ContractGenerationException e)
        {
            throw new BusinessException(e.Message);
        }
        catch (AccountProvisioningException e)
        {
            throw new BusinessException(e.Message);
        }
        catch (PayrollEnrollmentException e)
        {
            throw new BusinessException(e.Message);
        }
    }
}