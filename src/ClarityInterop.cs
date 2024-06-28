using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Clarity.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Blazor.Clarity;

///<inheritdoc cref="IClarityInterop"/>
public class ClarityInterop : IClarityInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<ClarityInterop> _logger;
    private readonly IModuleImportUtil _moduleImportUtil;

    private readonly AsyncSingleton<object> _scriptInitializer;

    public ClarityInterop(IJSRuntime jSRuntime, ILogger<ClarityInterop> logger, IModuleImportUtil moduleImportUtil)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _moduleImportUtil = moduleImportUtil;

        _scriptInitializer = new AsyncSingleton<object>(async objects =>
        {
            var cancellationToken = (CancellationToken)objects[0];

            await _moduleImportUtil.Import("Soenneker.Blazor.Clarity/clarityinterop.js", cancellationToken);
            await _moduleImportUtil.WaitUntilLoadedAndAvailable("Soenneker.Blazor.Clarity/clarityinterop.js", "ClarityInitializer", 100, cancellationToken);
            return new object();
        });
    }

    public async ValueTask Init(string key, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing Clarity...");

        await _scriptInitializer.Get(cancellationToken).NoSync();

        await _jsRuntime.InvokeVoidAsync("ClarityInitializer.init", cancellationToken, key);
    }

    public ValueTask DisposeAsync()
    {
        return _moduleImportUtil.DisposeModule("Soenneker.Blazor.Clarity/clarityinterop.js");
    }
}