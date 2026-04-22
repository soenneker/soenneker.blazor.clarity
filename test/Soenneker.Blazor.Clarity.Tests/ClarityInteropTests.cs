using Soenneker.Blazor.Clarity.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.Clarity.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class ClarityInteropTests : HostedUnitTest
{
    private readonly IClarityInterop _util;

    public ClarityInteropTests(Host host) : base(host)
    {
        _util = Resolve<IClarityInterop>(true);
    }

    [Test]
    public void Default()
    {

    }
}
