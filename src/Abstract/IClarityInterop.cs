using System.Threading.Tasks;

namespace Soenneker.Blazor.Clarity.Abstract;

/// <summary>
/// A Blazor interop library that sets up Microsoft Clarity (https://clarity.microsoft.com/)
/// </summary>
public interface IClarityInterop 
{
    /// <summary>
    /// Calls the Clarity JS interop initialization code, and begins the connection to Clarity. <para/>
    /// Should be called ASAP in the app, typically in App.razor within OnInitializedAsync
    /// </summary>
    ValueTask Init(string key);
}