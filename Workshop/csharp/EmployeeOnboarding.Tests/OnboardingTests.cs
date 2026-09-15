using EmployeeOnboarding.Externals;
using EmployeeOnboarding.Models;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace EmployeeOnboarding.Tests;

public class OnboardingTests
{
    // ── Test data ─────────────────────────────────────────────────────────────
    private static readonly AcceptedOffer Offer = new()
        { Name = "Alice", Email = "alice@corp.com", Department = "Engineering", StartDate = new DateTime(2024, 1, 15) };

    private static readonly Employee RegisteredEmployee = new() { Id = 1, Name = "Alice", Email = "alice@corp.com" };

    private static readonly Contract GeneratedContract = new()
        { Id = 100, EmployeeId = 1, StartDate = new DateTime(2024, 1, 15) };

    private static readonly Account ProvisionedAccount = new() { EmployeeId = 1, Login = "alice.corp" };

    private static readonly OnboardingResult EnrollmentResult = new()
        { EmployeeId = 1, Login = "alice.corp", EnrolledAt = new DateTime(2024, 1, 15) };

    private readonly IEmployeeRepository _employees = Substitute.For<IEmployeeRepository>();
    private readonly IHrSystem _hr = Substitute.For<IHrSystem>();
    private readonly IItProvisioning _it = Substitute.For<IItProvisioning>();
    private readonly Onboarding _onboarding;
    private readonly IPayroll _payroll = Substitute.For<IPayroll>();

    public OnboardingTests()
    {
        _onboarding = new Onboarding(_employees, _hr, _it, _payroll);
    }

    // ── Happy path ────────────────────────────────────────────────────────────

    [Fact]
    public void Should_return_enrollment_when_all_steps_succeed()
    {
        _employees.Register(Offer).Returns(RegisteredEmployee);
        _hr.GenerateContract(RegisteredEmployee).Returns(GeneratedContract);
        _it.ProvisionAccount(GeneratedContract).Returns(ProvisionedAccount);
        _payroll.Enroll(ProvisionedAccount).Returns(EnrollmentResult);

        var result = _onboarding.OnboardNewHire(Offer);

        result.Should().Be(EnrollmentResult);
    }

    // ── Failure paths ─────────────────────────────────────────────────────────

    [Fact]
    public void Should_throw_business_exception_when_employee_registration_fails()
    {
        _employees.Register(Offer).Throws(new EmployeeRegistrationException("Duplicate employee record"));

        var act = () => _onboarding.OnboardNewHire(Offer);

        act.Should().Throw<BusinessException>().WithMessage("Duplicate employee record");
    }

    [Fact]
    public void Should_throw_business_exception_when_contract_generation_fails()
    {
        _employees.Register(Offer).Returns(RegisteredEmployee);
        _hr.GenerateContract(RegisteredEmployee).Throws(new ContractGenerationException("Missing salary band"));

        var act = () => _onboarding.OnboardNewHire(Offer);

        act.Should().Throw<BusinessException>().WithMessage("Missing salary band");
    }

    [Fact]
    public void Should_throw_business_exception_when_account_provisioning_fails()
    {
        _employees.Register(Offer).Returns(RegisteredEmployee);
        _hr.GenerateContract(RegisteredEmployee).Returns(GeneratedContract);
        _it.ProvisionAccount(GeneratedContract).Throws(new AccountProvisioningException("Login already taken"));

        var act = () => _onboarding.OnboardNewHire(Offer);

        act.Should().Throw<BusinessException>().WithMessage("Login already taken");
    }

    [Fact]
    public void Should_throw_business_exception_when_payroll_enrollment_fails()
    {
        _employees.Register(Offer).Returns(RegisteredEmployee);
        _hr.GenerateContract(RegisteredEmployee).Returns(GeneratedContract);
        _it.ProvisionAccount(GeneratedContract).Returns(ProvisionedAccount);
        _payroll.Enroll(ProvisionedAccount).Throws(new PayrollEnrollmentException("Payroll system unavailable"));

        var act = () => _onboarding.OnboardNewHire(Offer);

        act.Should().Throw<BusinessException>().WithMessage("Payroll system unavailable");
    }
}