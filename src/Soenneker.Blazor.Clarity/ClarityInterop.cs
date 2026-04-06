using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Clarity.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Utils.CancellationScopes;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.CancellationTokens;

namespace Soenneker.Blazor.Clarity;

///<inheritdoc cref="IClarityInterop"/>
public sealed class ClarityInterop : IClarityInterop
{
    private readonly ILogger<ClarityInterop> _logger;
    private readonly IModuleImportUtil _moduleImportUtil;

    private const string _modulePath = "/_content/Soenneker.Blazor.Clarity/js/clarityinterop.js";

    private readonly CancellationScope _cancellationScope = new();

    public ClarityInterop(ILogger<ClarityInterop> logger, IModuleImportUtil moduleImportUtil)
    {
        _logger = logger;
        _moduleImportUtil = moduleImportUtil;
    }

    public async ValueTask Init(string key, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing Clarity...");

        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("init", linked, key);
        }
    }

    public async ValueTask Consent(CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("consent", linked);
        }
    }

    public async ValueTask Identify(string id, string? sessionId = null, string? pageId = null, string? friendlyName = null,
        CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("identify", linked, id, sessionId, pageId, friendlyName);
        }
    }

    public async ValueTask SetTag(string key, object value, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("setTag", linked, key, value);
        }
    }

    public async ValueTask TrackEvent(string name, CancellationToken cancellationToken = default)
    {
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("trackEvent", linked, name);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _moduleImportUtil.DisposeContentModule(_modulePath);
        await _cancellationScope.DisposeAsync();
    }
}
