using System;
using LanguageExt;

namespace TalkThrowExceptionsSnippets.Tests.DivideExample;

internal interface ICanDivide
{
	decimal Divide(decimal x, decimal y);
}

public class WeirdDivideImplementation1 : ICanDivide
{
	public decimal Divide(decimal x, decimal y) => y == default ? default : x / y;
}

public class WeirdDivideImplementation2 : ICanDivide
{
	private const int DefaultMagicValue = 785;
	public decimal Divide(decimal x, decimal y) => y == default ? DefaultMagicValue : x / y;
}

public class WeirdDivideImplementation3 : ICanDivide
{
	public decimal Divide(decimal x, decimal y) => y == default ? throw new DivideByZeroException() : x / y;
}

public class WeirdDivideImplementation4 : ICanDivide
{
	public decimal Divide(decimal x, decimal y) => y == default ? throw new MyCustomException() : x / y;

	public class MyCustomException : Exception
	{
	}
}

public class DivideExampleTest
{
	public void Main()
	{
		double a = 98, b = 0;
		var text = SafeDivision(a, b).Match(success => $"{a} divided by {b} = {success}", _ => _);
		Console.WriteLine(text);
	}

	private static Either<string, double> SafeDivision(double x, double y) =>
		y == 0
			? Either<string, double>.Left("Attempted divide by zero.")
			: Either<string, double>.Right(x / y);
}