using EmployeeOnboarding.Externals;
using EmployeeOnboarding.Models;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeeOnboarding.Tests;

public class OnboardingTests
{
    private readonly ICandidateRepository _candidates = Substitute.For<ICandidateRepository>();
    private readonly IHrSystem _hr = Substitute.For<IHrSystem>();
    private readonly IItProvisioning _it = Substitute.For<IItProvisioning>();
    private readonly Onboarding _onboarding;

    // ── Test data ─────────────────────────────────────────────────────────────
    private static readonly Department Engineering = new("Engineering");
    private static readonly Department Marketing   = new("Marketing");

    private static readonly Candidate Alice        = new("Alice", "alice@corp.com");
    private static readonly Candidate Bob          = new("Bob",   "bob@corp.com");

    private static readonly Contract AliceContract = new("Alice", "alice@corp.com");
    private static readonly Contract BobContract   = new("Bob",   "bob@corp.com");

    private static readonly Account AliceAccount   = new("Alice", "alice.corp");
    private static readonly Account BobAccount     = new("Bob",   "bob.corp");

    public OnboardingTests()
    {
        _onboarding = new Onboarding(_candidates, _hr, _it);
    }

    // ── Happy path ────────────────────────────────────────────────────────────

    [Fact]
    public void Should_onboard_candidate_when_all_steps_succeed()
    {
        _candidates.FindApprovedCandidate(Engineering).Returns(Alice);
        _hr.GenerateContract(Alice).Returns(AliceContract);
        _it.ProvisionAccount(Alice.Email).Returns(AliceAccount);

        var result = _onboarding.ProcessNewHires(Engineering);

        result.Should().ContainSingle()
            .Which.Should().Be("Onboarded: Alice is ready to start!");
    }

    [Fact]
    public void Should_process_multiple_departments()
    {
        _candidates.FindApprovedCandidate(Engineering).Returns(Alice);
        _hr.GenerateContract(Alice).Returns(AliceContract);
        _it.ProvisionAccount(Alice.Email).Returns(AliceAccount);

        _candidates.FindApprovedCandidate(Marketing).Returns(Bob);
        _hr.GenerateContract(Bob).Returns(BobContract);
        _it.ProvisionAccount(Bob.Email).Returns(BobAccount);

        var result = _onboarding.ProcessNewHires(Engineering, Marketing);

        result.Should().HaveCount(2)
            .And.Contain("Onboarded: Alice is ready to start!")
            .And.Contain("Onboarded: Bob is ready to start!");
    }

    // ── Failure paths — currently SILENT ─────────────────────────────────────
    // These tests document what the code does TODAY.
    // Your goal: make each failure case explicit in the output.

    [Fact]
    public void Should_throw_when_no_departments_provided()
    {
        var act = () => _onboarding.ProcessNewHires();

        act.Should().Throw<ArgumentException>()
            .WithMessage("No departments to process!");
    }

    [Fact]
    public void Should_silently_skip_when_no_approved_candidate()
    {
        _candidates.FindApprovedCandidate(Engineering).Returns((Candidate?)null);

        var result = _onboarding.ProcessNewHires(Engineering);

        // ⚠️ Silent! The caller has no idea why Engineering was skipped.
        result.Should().BeEmpty();
    }

    [Fact]
    public void Should_silently_skip_when_contract_generation_fails()
    {
        _candidates.FindApprovedCandidate(Engineering).Returns(Alice);
        _hr.GenerateContract(Alice).Returns((Contract?)null);

        var result = _onboarding.ProcessNewHires(Engineering);

        // ⚠️ Silent! Indistinguishable from the case above.
        result.Should().BeEmpty();
    }

    [Fact]
    public void Should_silently_skip_when_it_provisioning_fails()
    {
        _candidates.FindApprovedCandidate(Engineering).Returns(Alice);
        _hr.GenerateContract(Alice).Returns(AliceContract);
        _it.ProvisionAccount(Alice.Email).Returns((Account?)null);

        var result = _onboarding.ProcessNewHires(Engineering);

        // ⚠️ Silent! Three different failures, one indistinguishable outcome.
        result.Should().BeEmpty();
    }
}
