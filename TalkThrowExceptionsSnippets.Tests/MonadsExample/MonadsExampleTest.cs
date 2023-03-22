using System;
using System.Threading.Tasks;
using Vonage.Common.Monads;

namespace TalkThrowExceptionsSnippets.Tests.MonadsExample;

public class MonadsExampleTest
{
    public async Task OptionalAsyncSupport()
    {
        var defaultValue = 15;
        var result = await Result<int>
            .FromSuccess(4)
            .MapAsync(this.Increment)
            .IfFailure(defaultValue);
    }

    public void OptionalDefaultValueWhenNoneState()
    {
        var defaultValue = 15;
        int? nullableInt = 4;
        var nonNullableInt = nullableInt.HasValue ? nullableInt.Value : defaultValue;
        var optionalInt = Maybe<int>.Some(4);
        nonNullableInt = optionalInt.IfNone(defaultValue);
    }

    public void OptionalExecutesFunctionWhenSomeState()
    {
        int? nullableInt = 4;
        if (nullableInt != null)
        {
            Console.WriteLine(nullableInt);
        }

        var optionalInt = Maybe<int>.Some(4);
        optionalInt.IfSome(Console.WriteLine);
    }

    private Task<int> Increment(int value) => Task.FromResult(value + 1);
}