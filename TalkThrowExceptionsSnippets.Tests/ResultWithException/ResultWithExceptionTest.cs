using Vonage.Server.Common.Failures;
using Vonage.Server.Common.Monads;

namespace TalkThrowExceptionsSnippets.Tests.ResultWithException;

public class ResultWithExceptionTest
{
    public void ResultExamples()
    {
        var result = Result<int>.FromSuccess(4);

        // Providing a function for both states
        result.Match(DoSomethingWithValue, DoSomethingWithFailure);

        // Receiving an exception if the monad is in the Failure state
        try
        {
            var value = result.GetSuccessUnsafe();
            DoSomethingWithValue(value);
        }
        catch (UnsafeValueException exception)
        {
            DoSomethingWithFailure(ResultFailure.FromErrorMessage(exception.Message));
        }
    }

    private static Unit DoSomethingWithFailure(IResultFailure failure)
    {
        return Unit.Default;
    }

    private static Unit DoSomethingWithValue(int value)
    {
        return Unit.Default;
    }
}