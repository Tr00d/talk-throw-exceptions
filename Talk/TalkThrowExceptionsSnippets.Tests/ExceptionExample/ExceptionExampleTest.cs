using System;

namespace TalkThrowExceptionsSnippets.Tests.ExceptionExample;

public class ExceptionExampleTest
{
    public void ExceptionShouldShortCircuit()
    {
        Console.WriteLine("This code will be executed.");
        throw new NotImplementedException();
        Console.WriteLine("This code won't be executed.");
    }
}

public class MyCustomException : ApplicationException
{
    public Foo Value { get; }

    public MyCustomException(Foo value, string message) : base(message) =>
        this.Value = value;
}

public record Foo(string First, string Second);