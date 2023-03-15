using System;
using FluentAssertions;
using Xunit;

namespace TalkThrowExceptionsSnippets.Tests.GenericInBox;

public class GenericInBoxTest
{
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
    public void Map_ShouldNotExecuteFunction_GivenValueIsNone() =>
        SchrodingerBox<int>.None()
            .Map(value => value + 1)
            .Map(value => value + 1)
            .Map(value => value + 1)
            .OpenBox()
            .Should()
            .BeNull();

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

        // Creates a box without a value
        public static SchrodingerBox<T> None() => new();

        public T? OpenBox() => this.IsSome ? this.value : null;

        // Creates a box with a value
        public static SchrodingerBox<T> Some(T value) => new(value);
    }
}