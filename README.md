[![](https://img.shields.io/nuget/v/Soenneker.Blazor.Clarity.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Clarity/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.clarity/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.clarity/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Blazor.Clarity.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Clarity/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.clarity/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.clarity/actions/workflows/codeql.yml)

# Soenneker.Blazor.Clarity

Scoped Blazor JavaScript interop for Microsoft Clarity initialization, Consent V2, identification, tags, and custom events.

## Installation and registration

```bash
dotnet add package Soenneker.Blazor.Clarity
```

```csharp
using Soenneker.Blazor.Clarity.Registrars;

builder.Services.AddClarityInteropAsScoped();
```

## Initialize with consent

Initialize after the first render, then immediately send the visitor's stored consent choices. Calls are queued by Clarity while its remote script loads.

```razor
@using Soenneker.Blazor.Clarity.Abstract
@inject IClarityInterop Clarity
@inject IConfiguration Configuration

@code {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        await Clarity.Init(Configuration["Clarity:ProjectId"]!);
        await Clarity.Consent(
            adStorage: marketingConsent,
            analyticsStorage: analyticsConsent);
    }
}
```

Call `Consent` again whenever either choice changes. Passing `false` denies the corresponding storage category and asks Clarity to clear applicable cookies. The wrapper requires `Init` before every other operation and prevents the same scoped instance from switching project IDs.

## Identify, tag, and track

```csharp
await Clarity.Identify(
    id: pseudonymousUserId,
    sessionId: sessionId,
    pageId: routeId,
    friendlyName: accountDisplayName);

await Clarity.SetTag("subscription", "business");
await Clarity.SetTag("experiments", new[] { "checkout-a", "nav-b" });
await Clarity.TrackEvent("checkout_completed");
```

Tag values must be a string or string array. Avoid sending email addresses, names, raw account IDs, or other directly identifying data as Clarity IDs, tags, event names, or friendly names; use pseudonymous values approved by your privacy policy.

The browser loads Clarity from `https://www.clarity.ms`. Configure Content Security Policy and consent gating for that origin before enabling the integration. Disposing the scoped interop releases the package's JavaScript module, but it does not unload a Clarity tracker already installed on the page.
