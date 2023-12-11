using System;
using FluentAssertions;
using Xunit;

namespace TalkThrowExceptionsSnippets.Tests.GenericInBoxWithBind;

public class GenericInBoxWithBindTest
{
	[Fact]
	public void Bind_ShouldReturnSome1_GivenValueIs8() =>
		SchrodingerBox<int>.Some(8)
			.Bind(Half) // becomes Some(4)
			.Bind(Half) // becomes Some(2)
			.Bind(Half) // becomes Some(1)
			.OpenBox()
			.Should()
			.Be(1);

	[Fact]
	public void Bind_ShouldReturnNull_GivenValueIs4() =>
		SchrodingerBox<int>.Some(4)
			.Bind(Half) // becomes Some(2)
			.Bind(Half) // becomes Some(1)
			.Bind(Half) // becomes None
			.OpenBox()
			.Should()
			.Be(null);
	
	private static SchrodingerBox<int> Half(int value) => value % 2 == 0 
		? SchrodingerBox<int>.Some(value / 2) 
		: SchrodingerBox<int>.None();

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
		public SchrodingerBox<TResult> Bind<TResult>(Func<T, SchrodingerBox<TResult>> bind) where TResult : struct =>
			this.IsSome
				? bind(this.value)
				: SchrodingerBox<TResult>.None();

		// Applies the function on the value IF our box contains a value
		public SchrodingerBox<TResult> Map<TResult>(Func<T, TResult> map) where TResult : struct =>
			this.IsSome
				? Some(map(this.value))
				: SchrodingerBox<TResult>.None();

		// Creates a box without a value
		public static SchrodingerBox<T> None() => new();

		public T? OpenBox() => this.IsSome ? this.value : null;

		// Creates a box with a value
		public static SchrodingerBox<TResult> Some<TResult>(TResult value) where TResult : struct => new(value);
	}
}