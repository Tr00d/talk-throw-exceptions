using FluentAssertions;
using FluentAssertions.LanguageExt;
using LanguageExt;
using Xunit;
using static LanguageExt.Prelude;

namespace TalkThrowExceptionsSnippets.Tests.DivideTry;

public class DivideTryTest
{
    [Fact]
    public void Divide_ShouldReturnFailure_GivenDivisorIsZero() =>
        Divide(50, 0)
            .Should()
            .BeFail()
            .Which
            .Message
            .Should()
            .Be("Attempted to divide by zero.");

    [Fact]
    public void Divide_ShouldReturnSuccess_GivenValueCanBeDivided() =>
        Divide(50, 2)
            .Should()
            .BeSuccess(value => value.Should().Be(25));

    private static Try<decimal> Divide(decimal value, decimal divisor) => 
        Try(() => value / divisor);
}