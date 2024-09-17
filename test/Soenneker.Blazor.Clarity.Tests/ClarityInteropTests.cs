using Soenneker.Blazor.Clarity.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Blazor.Clarity.Tests;

[Collection("Collection")]
public class ClarityInteropTests : FixturedUnitTest
{
    private readonly IClarityInterop _interop;

    public ClarityInteropTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _interop = Resolve<IClarityInterop>(true);
    }
}
