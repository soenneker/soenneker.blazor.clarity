using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Clarity.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;

namespace Soenneker.Blazor.Clarity;

///<inheritdoc cref="IClarityInterop"/>
public class ClarityInterop : IClarityInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<ClarityInterop> _logger;
    private readonly IModuleImportUtil _moduleImportUtil;

    private bool _initialized;

    public ClarityInterop(IJSRuntime jSRuntime, ILogger<ClarityInterop> logger, IModuleImportUtil moduleImportUtil)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _moduleImportUtil = moduleImportUtil;
    }

    private async ValueTask EnsureInitialization(CancellationToken cancellationToken = default)
    {
        if (_initialized)
            return;

        _initialized = true;

        await _moduleImportUtil.Import("Soenneker.Blazor.Clarity/js/clarityinterop.js", cancellationToken);
        await _moduleImportUtil.WaitUntilLoaded("Soenneker.Blazor.Clarity/js/clarityinterop.js", cancellationToken);
    }

    public async ValueTask Init(string key, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing Clarity...");

        await EnsureInitialization(cancellationToken);

        await _jsRuntime.InvokeVoidAsync("ClarityInitializer.init", cancellationToken, key);
    }
}