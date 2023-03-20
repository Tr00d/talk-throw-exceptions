using System;
using System.Threading.Tasks;
using Vonage.Common.Failures;
using Vonage.Common.Monads;
using Vonage.Request;
using Vonage.Server.Video;
using Vonage.Server.Video.Archives.Common;
using Vonage.Server.Video.Archives.StopArchive;

namespace TalkThrowExceptionsSnippets.Tests.VonageExample;

public class VonageExampleTest
{
    public async Task Example()
    {
        var applicationid = Guid.NewGuid();
        var archiveId = Guid.NewGuid();
        var client = new VideoClient(Credentials.FromApiKeyAndSecret("", ""));
        var result = await StopArchiveRequest
            .Parse(applicationid, archiveId)
            .BindAsync(request => client.ArchiveClient.StopArchiveAsync(request));
        result.Match(this.DoSomethingWithArchive, this.DoSomethingWithFailure);
    }

    private Unit DoSomethingWithArchive(Archive value) => Unit.Default;

    private Unit DoSomethingWithFailure(IResultFailure failure) => Unit.Default;
}