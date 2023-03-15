using System;
using Xunit;

namespace TalkThrowExceptionsSnippets.Tests.DivideExample;

public class DivideExampleTest
{
    private readonly ICanDivide calculator;

    public DivideExampleTest() => this.calculator = new Calculator();

    [Fact]
    public void Blabla()
    {
        this.calculator.Divide(0, 0);
    }
}

internal interface ICanDivide
{
    decimal Divide(decimal x, decimal y);
}

internal class Calculator : ICanDivide
{
    public decimal Divide(decimal x, decimal y) =>
        y == 0
            ? throw new DivideByZeroException()
            : x / y;
}