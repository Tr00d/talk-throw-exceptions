namespace EmployeeOnboarding.Models;

public class AcceptedOffer
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Department { get; set; }
    public DateTime StartDate { get; set; }
}

public class Employee
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}

public class Contract
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
}

public class Account
{
    public int EmployeeId { get; set; }
    public required string Login { get; set; }
}

public class OnboardingResult
{
    public int EmployeeId { get; set; }
    public required string Login { get; set; }
    public DateTime EnrolledAt { get; set; }
}

public record Error(string Reason);
public record EmployeeRegistrationError(string Reason) : Error(Reason);
public record ContractGenerationError(string Reason) : Error(Reason);
public record AccountProvisioningError(string Reason) : Error(Reason);
public record PayrollEnrollmentError(string Reason) : Error(Reason);