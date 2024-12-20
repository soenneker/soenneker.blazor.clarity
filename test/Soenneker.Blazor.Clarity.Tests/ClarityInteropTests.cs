using Soenneker.Blazor.Clarity.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;


namespace Soenneker.Blazor.Clarity.Tests;

[Collection("Collection")]
public class ClarityInteropTests : FixturedUnitTest
{
    private readonly IClarityInterop _util;

    public ClarityInteropTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IClarityInterop>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
