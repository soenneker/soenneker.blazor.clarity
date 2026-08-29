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
    /// <param name="key">Key used to locate the target entry.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the clarity is ready for use.</returns>
    ValueTask Init(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends explicit advertising and analytics storage consent to Clarity using the Consent V2 API.
    /// </summary>
    /// <param name="adStorage">Whether storage related to advertising is permitted.</param>
    /// <param name="analyticsStorage">Whether storage related to analytics is permitted.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the consent operation is complete.</returns>
    ValueTask Consent(bool adStorage, bool analyticsStorage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Identifies a user with optional session, page, and friendly names
    /// </summary>
    /// <param name="id">Identifier of the clarity instance or registration to target.</param>
    /// <param name="sessionId">Identifier of the session to target.</param>
    /// <param name="pageId">Identifier of the page to target.</param>
    /// <param name="friendlyName">Name of the friendly to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the identify operation is complete.</returns>
    ValueTask Identify(string id, string? sessionId = null, string? pageId = null, string? friendlyName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a custom tag in Clarity. Value can be a string or a string array.
    /// </summary>
    /// <param name="key">Key used to locate the target entry.</param>
    /// <param name="value">Tag value, supplied as text or an array of text values.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the tag has been stored.</returns>
    ValueTask SetTag(string key, object value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tracks a custom event in Clarity
    /// </summary>
    /// <param name="name">Name of the Clarity value to target.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the track event operation is complete.</returns>
    ValueTask TrackEvent(string name, CancellationToken cancellationToken = default);
}
