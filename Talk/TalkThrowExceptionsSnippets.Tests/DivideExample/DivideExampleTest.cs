using System;
using LanguageExt;
using TalkThrowExceptionsSnippets.Tests.ExceptionExample;

namespace TalkThrowExceptionsSnippets.Tests.DivideExample;

internal interface ICanDivide
{
    decimal Divide(decimal x, decimal y);
}

public class DivideExampleTest
{
    public void Main()
    {
        double a = 98, b = 0;
        var text = SafeDivision(a, b).IfRight(success => $"{a} divided by {b} = {success}");
        Console.WriteLine(text);
    }

    private static Either<string, double> SafeDivision(double x, double y) =>
        y == 0
            ? Either<string, double>.Left("Attempted divide by zero.")
            : Either<string, double>.Right(x / y);
}

public class Implementation1 : ICanDivide
{
    public decimal Divide(decimal x, decimal y) => y == 0 ? 0 : x / y;
}

public class Implementation2 : ICanDivide
{
    private const int DefaultMagicValue = 123;
    public decimal Divide(decimal x, decimal y) => y == 0 ? DefaultMagicValue : x / y;
}

public class Implementation3 : ICanDivide
{
    public decimal Divide(decimal x, decimal y) => y == 0 ? throw new DivideByZeroException() : x / y;
}

public class Implementation4 : ICanDivide
{
    public decimal Divide(decimal x, decimal y) => y == 0 ? throw new Exception() : x / y;
}