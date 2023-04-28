using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.Clarity.Abstract;

namespace Soenneker.Blazor.Clarity.Registrars;

/// <summary>
/// A Blazor interop library that sets up Microsoft Clarity (https://clarity.microsoft.com/)
/// </summary>
public static class ClarityRegistrar
{
    /// <summary>
    /// Shorthand for <code>services.TryAddScoped</code>
    /// </summary>
    public static void AddClarity(this IServiceCollection services)
    {
        services.TryAddScoped<IClarityInterop, ClarityInterop>();
    }
}