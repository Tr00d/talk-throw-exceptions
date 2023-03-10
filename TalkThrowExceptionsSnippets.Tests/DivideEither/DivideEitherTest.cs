using FluentAssertions;
using FluentAssertions.LanguageExt;
using LanguageExt;
using Xunit;

namespace TalkThrowExceptionsSnippets.Tests.DivideEither;

public class DivideEitherTest
{
    [Fact]
    public void Divide_ShouldReturnLeft_GivenDivisorIsZero() =>
        Divide(50, 0)
            .Should()
            .BeLeft(left => left.Should().Be(new Error("Cannot divide by 0.")));

    [Fact]
    public void Divide_ShouldReturnRight_GivenValueCanBeDivided() =>
        Divide(50, 2)
            .Should()
            .BeRight(value => value.Should().Be(25));

    private static Either<Error, decimal> Divide(decimal value, decimal divisor) =>
        divisor == 0
            ? new Error("Cannot divide by 0.")
            : value / divisor;

    private record Error(string Message);
}