using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.Clarity.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Registrars;
using Soenneker.Blazor.Utils.ResourceLoader.Registrars;

namespace Soenneker.Blazor.Clarity.Registrars;

/// <summary>
/// A Blazor interop library that sets up Microsoft Clarity (https://clarity.microsoft.com/)
/// </summary>
public static class ClarityRegistrar
{
    /// <summary>
    /// Shorthand for <code>services.TryAddSingleton</code>
    /// </summary>
    public static void AddClarity(this IServiceCollection services)
    {
        services.AddResourceLoader();
        services.TryAddSingleton<IClarityInterop, ClarityInterop>();
    }
}