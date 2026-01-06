using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Clarity.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Utils.CancellationScopes;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Asyncs.Initializers;
using Soenneker.Extensions.CancellationTokens;

namespace Soenneker.Blazor.Clarity;

///<inheritdoc cref="IClarityInterop"/>
public sealed class ClarityInterop : IClarityInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<ClarityInterop> _logger;
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncInitializer _scriptInitializer;

    private const string _modulePath = "Soenneker.Blazor.Clarity/js/clarityinterop.js";
    private const string _moduleName = "ClarityInterop";

    private readonly CancellationScope _cancellationScope = new();

    public ClarityInterop(IJSRuntime jSRuntime, ILogger<ClarityInterop> logger, IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _resourceLoader = resourceLoader;
        _scriptInitializer = new AsyncInitializer(InitializeScript);
    }

    private async ValueTask InitializeScript(CancellationToken token)
    {
        await _resourceLoader.ImportModuleAndWaitUntilAvailable(_modulePath, _moduleName, 100, token);
    }

    public async ValueTask Init(string key, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing Clarity...");

        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _scriptInitializer.Init(linked);

            await _jsRuntime.InvokeVoidAsync("ClarityInterop.init", linked, key);
        }
    }

    public async ValueTask Consent(CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _scriptInitializer.Init(linked);
            await _jsRuntime.InvokeVoidAsync("ClarityInterop.consent", linked);
        }
    }

    public async ValueTask Identify(string id, string? sessionId = null, string? pageId = null, string? friendlyName = null,
        CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _scriptInitializer.Init(linked);
            await _jsRuntime.InvokeVoidAsync("ClarityInterop.identify", linked, id, sessionId, pageId, friendlyName);
        }
    }

    public async ValueTask SetTag(string key, object value, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _scriptInitializer.Init(linked);
            await _jsRuntime.InvokeVoidAsync("ClarityInterop.setTag", linked, key, value);
        }
    }

    public async ValueTask TrackEvent(string name, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _scriptInitializer.Init(linked);
            await _jsRuntime.InvokeVoidAsync("ClarityInterop.trackEvent", linked, name);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_modulePath);

        await _scriptInitializer.DisposeAsync();

        await _cancellationScope.DisposeAsync();
    }
}
