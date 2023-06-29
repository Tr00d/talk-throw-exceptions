using System;
using Vonage.Common.Exceptions;
using Vonage.Common.Failures;
using Vonage.Common.Monads;

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
        catch (VonageAuthenticationException exception)
        {
            DoSomethingWithVonageAuthenticationException(exception);
        }
        catch (VonageHttpRequestException exception)
        {
            DoSomethingWithVonageHttpRequestException(exception);
        }
        catch (VonageException exception)
        {
            DoSomethingWithVonageException(exception);
        }
        catch (Exception exception)
        {
            DoSomethingWithException(exception);
        }
    }

    private static Unit DoSomethingWithException(Exception exception) => Unit.Default;

    private static Unit DoSomethingWithFailure(IResultFailure failure)
    {
        return Unit.Default;
    }

    private static Unit DoSomethingWithValue(int value)
    {
        return Unit.Default;
    }

    private static Unit DoSomethingWithVonageAuthenticationException(VonageAuthenticationException exception) =>
        Unit.Default;

    private static Unit DoSomethingWithVonageException(VonageException exception) => Unit.Default;

    private static Unit DoSomethingWithVonageHttpRequestException(VonageHttpRequestException exception) => Unit.Default;
}