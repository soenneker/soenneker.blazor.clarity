using Microsoft.Extensions.DependencyInjection;
using Soenneker.Blazor.Clarity.Abstract;

namespace Soenneker.Blazor.Clarity.Extensions;

public static class ClarityRegistrar
{
    /// <summary>
    /// Shorthand for <code>services.AddScoped</code>
    /// </summary>
    public static void AddClarity(this IServiceCollection services)
    {
        services.AddScoped<IClarityInterop, ClarityInterop>();
    }
}