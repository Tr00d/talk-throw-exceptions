using FluentAssertions.LanguageExt;
using LanguageExt;
using Xunit;

namespace TalkThrowExceptionsSnippets.Tests.LanguageExtBox;

public class LanguageExtBox
{
    [Fact]
    public void Map_ShouldExecuteFunction_GivenValueIsSome_WithMethodGroup() =>
        Option<int>.Some(3)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Should()
            .Be(6);

    [Fact]
    public void Map_ShouldNotExecuteFunction_GivenValueIsNone() =>
        Option<int>.None
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Should()
            .BeNone();

    private static int IncrementValue(int value) => value + 1;
}