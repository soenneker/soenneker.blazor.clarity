using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Clarity.Abstract;

/// <summary>
/// An interop utility for Microsoft Clarity
/// </summary>
public interface IClarityInterop : IAsyncDisposable
{
    /// <summary>
    /// Initializes Clarity with the provided project key
    /// </summary>
    ValueTask Init(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends explicit advertising and analytics storage consent to Clarity using the Consent V2 API.
    /// </summary>
    /// <param name="adStorage">Whether storage related to advertising is permitted.</param>
    /// <param name="analyticsStorage">Whether storage related to analytics is permitted.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    ValueTask Consent(bool adStorage, bool analyticsStorage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Identifies a user with optional session, page, and friendly names
    /// </summary>
    ValueTask Identify(string id, string? sessionId = null, string? pageId = null, string? friendlyName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a custom tag in Clarity. Value can be a string or a string array.
    /// </summary>
    ValueTask SetTag(string key, object value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tracks a custom event in Clarity
    /// </summary>
    ValueTask TrackEvent(string name, CancellationToken cancellationToken = default);
}
