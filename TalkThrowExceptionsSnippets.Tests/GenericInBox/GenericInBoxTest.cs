using System;
using FluentAssertions;
using Xunit;

namespace TalkThrowExceptionsSnippets.Tests.GenericInBox;

public class GenericInBoxTest
{
    [Fact]
    public void ImperativeEquivalent()
    {
        var value = 3;
        if (value != default)
        {
            value += 1;
        }

        if (value != default)
        {
            value += 1;
        }

        if (value != default)
        {
            value += 1;
        }

        value.Should().Be(3);
    }

    [Fact]
    public void Map_ShouldExecuteFunction_GivenValueIsSome() =>
        SchrodingerBox<int>.Some(3)
            .Map(value => value + 1)
            .Map(value => value + 1)
            .Map(value => value + 1)
            .OpenBox()
            .Should()
            .Be(6);

    [Fact]
    public void Map_ShouldExecuteFunction_GivenValueIsSome_WithMethodGroup() =>
        SchrodingerBox<int>.Some(3)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .OpenBox()
            .Should()
            .Be(6);

    [Fact]
    public void Map_ShouldNotExecuteFunction_GivenValueIsNone() =>
        SchrodingerBox<int>.None()
            .Map(IncrementValue)
            .Map(IncrementValue)
            .Map(IncrementValue)
            .OpenBox()
            .Should()
            .Be(default);

    private static int IncrementValue(int value) => value + 1;

    internal class SchrodingerBox<T>
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

        // Creates a box without a value
        public static SchrodingerBox<T> None() => new();

        public T OpenBox() => this.value;

        // Creates a box with a value
        public static SchrodingerBox<T> Some(T value) => new(value);
    }
}