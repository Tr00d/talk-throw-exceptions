using System;
using LanguageExt;

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
        var text = SafeDivision(a, b).Match(success => $"{a} divided by {b} = {success}", _ => _);
        Console.WriteLine(text);
    }

    private static Either<string, double> SafeDivision(double x, double y) =>
        y == 0
            ? Either<string, double>.Left("Attempted divide by zero.")
            : Either<string, double>.Right(x / y);
}