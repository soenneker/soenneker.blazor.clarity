using Microsoft.JSInterop;
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
        await _util.Consent(adStorage: false, analyticsStorage: true);
    }
}
