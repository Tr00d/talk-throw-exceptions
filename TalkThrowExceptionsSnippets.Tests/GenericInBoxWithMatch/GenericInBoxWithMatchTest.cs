using System;
using FluentAssertions;
using Xunit;

namespace TalkThrowExceptionsSnippets.Tests.GenericInBoxWithMatch;

public class GenericInBoxWithMatchTest
{
    [Fact]
    public void Map_ShouldExecuteFunction_GivenValueIsSome() =>
        SchrodingerBox<int>.Some(3)
            .Map(value => value + 1)
            .Map(value => value + 1)
            .Map(value => value + 1)
            .Match(_ => _, () => 0)
            .Should()
            .Be(6);

    [Fact]
    public void Map_ShouldExecuteFunction_GivenValueIsSome_WithMethodGroup() =>
        SchrodingerBox<int>.Some(3)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Match(_ => _, () => 0)
            .Should()
            .Be(6);

    [Fact]
    public void Map_ShouldNotExecuteFunction_GivenValueIsNone_WithMethodGroup() =>
        SchrodingerBox<int>.None()
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Match(_ => _, () => 0)
            .Should()
            .Be(0);

    [Fact]
    public void Match_ShouldExecuteNoneFunction_Declarative() =>
        SchrodingerBox<int>.None()
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Match(ConvertToMessage, GetDefaultMessage)
            .Should()
            .Be(GetDefaultMessage());

    [Fact]
    public void Match_ShouldExecuteNoneFunction_GivenValueIsNone() =>
        SchrodingerBox<int>.None()
            .Map(value => value + 1)
            .Map(value => value + 1)
            .Map(value => value + 1)
            .Match(some => $"The value is some {some}!", () => "The value is none")
            .Should()
            .Be("The value is none");

    [Fact]
    public void Match_ShouldExecuteSomeFunction_Declarative() =>
        SchrodingerBox<int>.Some(3)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Match(ConvertToMessage, GetDefaultMessage)
            .Should()
            .Be(ConvertToMessage(6));

    [Fact]
    public void Match_ShouldExecuteSomeFunction_GivenValueIsSome() =>
        SchrodingerBox<int>.Some(3)
            .Map(value => value + 1)
            .Map(value => value + 1)
            .Map(value => value + 1)
            .Match(some => $"The value is some {some}!", () => "The value is none")
            .Should()
            .Be("The value is some 6!");

    private static string ConvertToMessage(int value) => $"The value is some {value}!";

    private static string GetDefaultMessage() => "The value is none";

    private static int IncrementValue(int value) => value + 1;

    internal class SchrodingerBox<T> where T : struct
    {
        private bool IsSome { get; }
        private readonly T value;

        // Constructor for Some
        private SchrodingerBox(T value)
        {
            this.value = value;
            this.IsSome = true;
        }

        // Constructor for None
        private SchrodingerBox() => this.IsSome = false;

        // Applies the function on the value IF our box contains a value
        public SchrodingerBox<T> Map(Func<T, T> map) =>
            this.IsSome
                ? Some(map(this.value))
                : None();

        // Applies a function depending on the state of the box to return a TResponse
        public TResponse Match<TResponse>(Func<T, TResponse> some, Func<TResponse> none) =>
            this.IsSome
                ? some(this.value)
                : none();

        // Creates a box without a value
        public static SchrodingerBox<T> None() => new();

        // Creates a box with a value
        public static SchrodingerBox<T> Some(T value) => new(value);
    }
}