using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.Clarity.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Utils.CancellationScopes;
using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.CancellationTokens;

namespace Soenneker.Blazor.Clarity;

/// <inheritdoc cref="IClarityInterop"/>
public sealed class ClarityInterop : IClarityInterop
{
    private readonly ILogger<ClarityInterop> _logger;
    private readonly IModuleImportUtil _moduleImportUtil;

    private const string _modulePath = "_content/Soenneker.Blazor.Clarity/js/clarityinterop.js";

    private readonly CancellationScope _cancellationScope = new();
    private readonly AsyncInitializer<string> _initializer;
    private string? _projectKey;

    public ClarityInterop(ILogger<ClarityInterop> logger, IModuleImportUtil moduleImportUtil)
    {
        _logger = logger;
        _moduleImportUtil = moduleImportUtil;
        _initializer = new AsyncInitializer<string>(Initialize);
    }

    public async ValueTask Init(string key, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _initializer.Init(key, linked).ConfigureAwait(false);

            if (!string.Equals(_projectKey, key, StringComparison.Ordinal))
                throw new InvalidOperationException("Clarity has already been initialized with a different project key.");
        }
    }

    private async ValueTask Initialize(string key, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Initializing Clarity...");
        IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, cancellationToken).ConfigureAwait(false);
        await module.InvokeVoidAsync("init", cancellationToken, key).ConfigureAwait(false);
        _projectKey = key;
    }

    public async ValueTask Consent(bool adStorage, bool analyticsStorage, CancellationToken cancellationToken = default)
    {
        EnsureInitialized();
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("consent", linked, adStorage, analyticsStorage);
        }
    }

    public async ValueTask Identify(string id, string? sessionId = null, string? pageId = null, string? friendlyName = null,
        CancellationToken cancellationToken = default)
    {
        EnsureInitialized();
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("identify", linked, id, sessionId, pageId, friendlyName);
        }
    }

    public async ValueTask SetTag(string key, object value, CancellationToken cancellationToken = default)
    {
        EnsureInitialized();
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(value);

        if (value is not string && value is not string[])
            throw new ArgumentException("Clarity tag values must be a string or string array.", nameof(value));

        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("setTag", linked, key, value);
        }
    }

    public async ValueTask TrackEvent(string name, CancellationToken cancellationToken = default)
    {
        EnsureInitialized();
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked);
            await module.InvokeVoidAsync("trackEvent", linked, name);
        }
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async ValueTask DisposeAsync()
    {
        _cancellationScope.Cancel();
        await _initializer.DisposeAsync().ConfigureAwait(false);
        await _moduleImportUtil.DisposeContentModule(_modulePath).ConfigureAwait(false);
        await _cancellationScope.DisposeAsync().ConfigureAwait(false);
    }

    private void EnsureInitialized()
    {
        if (!_initializer.IsInitialized)
            throw new InvalidOperationException("Init must be called before using Clarity.");
    }
}
