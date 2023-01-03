using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Clarity.Abstract;

namespace Soenneker.Blazor.Clarity;

///<inheritdoc cref="IClarityInterop"/>
public class ClarityInterop : IClarityInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<ClarityInterop> _logger;

    public ClarityInterop(IJSRuntime jSRuntime, ILogger<ClarityInterop> logger)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
    }

    ///<inheritdoc/>
    public async ValueTask Init(string key)
    {
        _logger.LogDebug("Initializing Clarity...");

        await _jsRuntime.InvokeVoidAsync("initClarity", key);
    }
}