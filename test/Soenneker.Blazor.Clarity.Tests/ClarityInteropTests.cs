using System;
using Microsoft.JSInterop;
using AwesomeAssertions;
using Soenneker.Blazor.Clarity.Abstract;
using Soenneker.Blazor.MockJsRuntime.Abstract;
using Soenneker.Tests.HostedUnit;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Clarity.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class ClarityInteropTests : HostedUnitTest
{
    private readonly IClarityInterop _util;

    public ClarityInteropTests(Host host) : base(host)
    {
        var jsRuntime = (IMockJsRuntime) Resolve<IJSRuntime>(true);
        jsRuntime.SetupMockResult<IJSObjectReference>("import", new TestJsObjectReference());
        _util = Resolve<IClarityInterop>(true);
    }

    [Test]
    public async Task Consent_v2_can_be_invoked()
    {
        await _util.Init("project-key");
        await _util.Consent(adStorage: false, analyticsStorage: true);
    }

    [Test]
    public async Task Init_rejects_a_blank_project_key()
    {
        Func<Task> act = async () => await _util.Init("   ");

        await act.Should().ThrowAsync<ArgumentException>();
    }
}
