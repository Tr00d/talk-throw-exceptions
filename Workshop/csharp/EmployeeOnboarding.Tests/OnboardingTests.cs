using EmployeeOnboarding.Externals;
using EmployeeOnboarding.Models;
using FluentAssertions;
using FluentAssertions.LanguageExt;
using NSubstitute;
using Xunit;

namespace EmployeeOnboarding.Tests;

public class OnboardingTests
{
    private static readonly AcceptedOffer Offer = new()
        { Name = "Alice", Email = "alice@corp.com", Department = "Engineering", StartDate = new DateTime(2024, 1, 15) };

    private static readonly Employee RegisteredEmployee = new() { Id = 1, Name = "Alice", Email = "alice@corp.com" };

    private static readonly Contract GeneratedContract = new()
        { Id = 100, EmployeeId = 1, StartDate = new DateTime(2024, 1, 15) };

    private static readonly Account ProvisionedAccount = new() { EmployeeId = 1, Login = "alice.corp" };

    private static readonly OnboardingResult EnrollmentResult = new()
        { EmployeeId = 1, Login = "alice.corp", EnrolledAt = new DateTime(2024, 1, 15) };

    private readonly AccountProvisioningError accountProvisioningError = new("Login already taken");
    private readonly ContractGenerationError contractGenerationError = new("Missing salary band");

    private readonly IEmployeeRepository employees = Substitute.For<IEmployeeRepository>();
    private readonly IHrSystem hr = Substitute.For<IHrSystem>();
    private readonly IItProvisioning it = Substitute.For<IItProvisioning>();
    private readonly Onboarding onboarding;
    private readonly IPayroll payroll = Substitute.For<IPayroll>();
    private readonly PayrollEnrollmentError payrollEnrollmentError = new("Payroll system unavailable");
    private readonly EmployeeRegistrationError registrationError = new("Duplicate employee record");

    public OnboardingTests()
    {
        onboarding = new Onboarding(employees, hr, it, payroll);
    }

    // ── Happy path ────────────────────────────────────────────────────────────

    [Fact]
    public void ShouldReturnOnboardingResult()
    {
        employees.Register(Offer).Returns(RegisteredEmployee);
        hr.GenerateContract(RegisteredEmployee).Returns(GeneratedContract);
        it.ProvisionAccount(GeneratedContract).Returns(ProvisionedAccount);
        payroll.Enroll(ProvisionedAccount).Returns(EnrollmentResult);
        var result = onboarding.OnboardNewHire(Offer);
        result.Should().BeRight(enrollment => enrollment.Should().Be(EnrollmentResult));
    }

    // ── Failure paths ─────────────────────────────────────────────────────────

    [Fact]
    public void ShouldReturnRegistrationError_WhenRegistrationFails()
    {
        employees.Register(Offer).Returns(registrationError);
        var result = onboarding.OnboardNewHire(Offer);
        result.Should().BeLeft(error => error.Should().Be(registrationError));
    }

    [Fact]
    public void ShouldReturnContractGenerationError_WhenContractGenerationFails()
    {
        employees.Register(Offer).Returns(RegisteredEmployee);
        hr.GenerateContract(RegisteredEmployee).Returns(contractGenerationError);
        var result = onboarding.OnboardNewHire(Offer);
        result.Should().BeLeft(error => error.Should().Be(contractGenerationError));
    }

    [Fact]
    public void ShouldReturnAccountProvisioningError_WhenAccountProvisioningFails()
    {
        employees.Register(Offer).Returns(RegisteredEmployee);
        hr.GenerateContract(RegisteredEmployee).Returns(GeneratedContract);
        it.ProvisionAccount(GeneratedContract).Returns(accountProvisioningError);
        var result = onboarding.OnboardNewHire(Offer);
        result.Should().BeLeft(error => error.Should().Be(accountProvisioningError));
    }

    [Fact]
    public void ShouldReturnPayrollEnrollmentError_WhenPayrollEnrollmentError()
    {
        employees.Register(Offer).Returns(RegisteredEmployee);
        hr.GenerateContract(RegisteredEmployee).Returns(GeneratedContract);
        it.ProvisionAccount(GeneratedContract).Returns(ProvisionedAccount);
        payroll.Enroll(ProvisionedAccount).Returns(payrollEnrollmentError);
        var result = onboarding.OnboardNewHire(Offer);
        result.Should().BeLeft(error => error.Should().Be(payrollEnrollmentError));
    }
}