using System;
using FluentAssertions;
using Xunit;

namespace TalkThrowExceptionsSnippets.Tests.GenericInBoxWithMatch;

public class GenericInBoxWithMatchTest
{
	[Fact]
	public void ImperativeEquivalent()
	{
		// Initialize the Box
		int? value = 3;

		// Map value => value +1
		if (value != null)
		{
			value += 1;
		}

		// Map value => value +1
		if (value != null)
		{
			value += 1;
		}

		// Map value => value +1
		if (value != null)
		{
			value += 1;
		}

		// Open the Box
		var result = value != null ? value : null;
		result.Should().Be(6);
	}

	[Fact]
	public void Map_ShouldReturnValue_GivenValueIsSome() =>
		SchrodingerBox<int>.Some(3)
			.Map(value => value + 1)
			.Map(value => value + 1)
			.Map(value => value + 1)
			.Bind(value => Half(value))
			.Match(_ => _, () => 0)
			.Should()
			.Be(6);
	
	

	[Fact]
	public void Match_ShouldMatchNoneMessage_Declarative() =>
		SchrodingerBox<int>.None()
			.Map(Increment)
			.Map(Increment)
			.Map(Increment)
			.Bind(Half)
			.Match(ConvertToSomeMessage, GetNoneMessage)
			.Should()
			.Be(GetNoneMessage());

	[Fact]
	public void Match_ShouldMatchNoneMessage_GivenValueIsNone() =>
		SchrodingerBox<int>.None()
			.Map(value => value + 1)
			.Map(value => value + 1)
			.Map(value => value + 1)
			.Bind(value => Half(value))
			.Match(some => $"The value is some {some}!", () => "The value is none")
			.Should()
			.Be("The value is none");

	[Fact]
	public void Match_ShouldMatchSomeMessage_Declarative() =>
		SchrodingerBox<int>.Some(3)
			.Map(Increment)
			.Map(Increment)
			.Map(Increment)
			.Bind(Half)
			.Match(ConvertToSomeMessage, GetNoneMessage)
			.Should()
			.Be(ConvertToSomeMessage(6));

	[Fact]
	public void Match_ShouldMatchNoneMessage_GivenValueIsSome() =>
		SchrodingerBox<int>.Some(3)
			.Map(value => value + 1)
			.Map(value => value + 1)
			.Map(value => value + 1)
			.Match(some => $"The value is some {some}!", () => "The value is none")
			.Should()
			.Be("The value is some 6!");

	private static string ConvertToSomeMessage(int value) => $"The value is some {value}!";

	private static string GetNoneMessage() => "The value is none";

	private static int Increment(int value) => value + 1;

	private static SchrodingerBox<int> Half(int value) => value % 2 == 0
		? SchrodingerBox<int>.Some(value / 2)
		: SchrodingerBox<int>.None();

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
		public SchrodingerBox<TResult> Bind<TResult>(Func<T, SchrodingerBox<TResult>> bind) where TResult : struct =>
			this.IsSome
				? bind(this.value)
				: SchrodingerBox<TResult>.None();

		// Applies the function on the value IF our box contains a value
		public SchrodingerBox<TResult> Map<TResult>(Func<T, TResult> map) where TResult : struct =>
			this.IsSome
				? Some(map(this.value))
				: SchrodingerBox<TResult>.None();

		// Applies a function depending on the state of the box to return a TResponse
		public TResponse Match<TResponse>(Func<T, TResponse> some, Func<TResponse> none) =>
			this.IsSome
				? some(this.value)
				: none();

		// Creates a box without a value
		public static SchrodingerBox<T> None() => new();

		// Creates a box with a value
		public static SchrodingerBox<TResult> Some<TResult>(TResult value) where TResult : struct => new(value);
	}
}