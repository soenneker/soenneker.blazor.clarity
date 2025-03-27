using System;
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

    private readonly AsyncSingleton _scriptInitializer;

    private const string _modulePath = "Soenneker.Blazor.Clarity/js/clarityinterop.js";
    private const string _moduleName = "ClarityInterop";

    public ClarityInterop(IJSRuntime jSRuntime, ILogger<ClarityInterop> logger, IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncSingleton(async (token, _) =>
        {
            await _resourceLoader.ImportModuleAndWaitUntilAvailable(_modulePath, _moduleName, 100, token).NoSync();
            return new object();
        });
    }

    public async ValueTask Init(string key, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing Clarity...");

        await _scriptInitializer.Init(cancellationToken).NoSync();

        await _jsRuntime.InvokeVoidAsync($"{_moduleName}.init", cancellationToken, key).NoSync();
    }

    public async ValueTask Consent(CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(cancellationToken).NoSync();
        await _jsRuntime.InvokeVoidAsync($"{_moduleName}.consent", cancellationToken).NoSync();
    }

    public async ValueTask Identify(string id, string? sessionId = null, string? pageId = null, string? friendlyName = null, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(cancellationToken).NoSync();
        await _jsRuntime.InvokeVoidAsync($"{_moduleName}.identify", cancellationToken, id, sessionId, pageId, friendlyName).NoSync();
    }

    public async ValueTask SetTag(string key, object value, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(cancellationToken).NoSync();
        await _jsRuntime.InvokeVoidAsync($"{_moduleName}.setTag", cancellationToken, key, value).NoSync();
    }

    public async ValueTask TrackEvent(string name, CancellationToken cancellationToken = default)
    {
        await _scriptInitializer.Init(cancellationToken).NoSync();
        await _jsRuntime.InvokeVoidAsync($"{_moduleName}.trackEvent", cancellationToken, name).NoSync();
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await _resourceLoader.DisposeModule(_modulePath).NoSync();

        await _scriptInitializer.DisposeAsync().NoSync();
    }
}