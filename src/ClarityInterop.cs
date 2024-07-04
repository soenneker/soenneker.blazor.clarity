using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Clarity.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Blazor.Clarity;

///<inheritdoc cref="IClarityInterop"/>
public class ClarityInterop : IClarityInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<ClarityInterop> _logger;
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncSingleton<object> _scriptInitializer;

    public ClarityInterop(IJSRuntime jSRuntime, ILogger<ClarityInterop> logger, IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncSingleton<object>(async (token, _) =>
        {
            await _resourceLoader.ImportModuleAndWaitUntilAvailable("Soenneker.Blazor.Clarity/clarityinterop.js", "ClarityInterop", 100, token);
            return new object();
        });
    }

    public async ValueTask Init(string key, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing Clarity...");

        await _scriptInitializer.Get(cancellationToken).NoSync();

        await _jsRuntime.InvokeVoidAsync("ClarityInterop.init", cancellationToken, key);
    }

    public ValueTask DisposeAsync()
    {
        return _resourceLoader.DisposeModule("Soenneker.Blazor.Clarity/clarityinterop.js");
    }
}