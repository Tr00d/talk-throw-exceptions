using System;
using System.Threading.Tasks;
using Vonage;
using Vonage.Common.Failures;
using Vonage.Common.Monads;
using Vonage.Request;
using Vonage.VerifyV2.VerifyCode;

namespace TalkThrowExceptionsSnippets.Tests.VonageExample;

public class VonageExampleTest
{
    public async Task Example()
    {
        var requestId = Guid.NewGuid();
        var code = string.Empty;
        var client = new VonageClient(Credentials.FromApiKeyAndSecret("", ""));
        var result = await VerifyCodeRequest.Build().WithRequestId(requestId)
            .WithCode(code)
            .Create()
            .BindAsync(request => client.VerifyV2Client.VerifyCodeAsync(request));
        result.Match(DoSomethingWithResponse, DoSomethingWithFailure);
    }

    private static void DoSomethingWithFailure(IResultFailure failure)
    {
    }

    private static void DoSomethingWithResponse(Unit value)
    {
    }
}