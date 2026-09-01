using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
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
    private readonly SemaphoreSlim _initializationLock = new(1, 1);
    private string? _projectKey;

    public ClarityInterop(ILogger<ClarityInterop> logger, IModuleImportUtil moduleImportUtil)
    {
        _logger = logger;
        _moduleImportUtil = moduleImportUtil;
    }

    public async ValueTask Init(string key, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _initializationLock.WaitAsync(linked).ConfigureAwait(false);

            try
            {
                if (_projectKey is not null)
                {
                    if (!string.Equals(_projectKey, key, StringComparison.Ordinal))
                        throw new InvalidOperationException("Clarity has already been initialized with a different project key.");

                    return;
                }

                _logger.LogDebug("Initializing Clarity...");
                IJSObjectReference module = await _moduleImportUtil.GetContentModuleReference(_modulePath, linked).ConfigureAwait(false);
                await module.InvokeVoidAsync("init", linked, key).ConfigureAwait(false);
                _projectKey = key;
            }
            finally
            {
                _initializationLock.Release();
            }
        }
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
        await _initializationLock.WaitAsync().ConfigureAwait(false);

        try
        {
            await _moduleImportUtil.DisposeContentModule(_modulePath).ConfigureAwait(false);
            await _cancellationScope.DisposeAsync().ConfigureAwait(false);
        }
        finally
        {
            _initializationLock.Release();
        }

        _initializationLock.Dispose();
    }

    private void EnsureInitialized()
    {
        if (_projectKey is null)
            throw new InvalidOperationException("Init must be called before using Clarity.");
    }
}
