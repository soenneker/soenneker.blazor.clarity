[![NuGet Version](https://img.shields.io/nuget/v/Soenneker.Blazor.Clarity.svg?style=flat)](https://www.nuget.org/packages/Soenneker.Blazor.Clarity/)

# Soenneker.Blazor.Clarity
### A small Blazor interop library that sets up [Microsoft Clarity](https://clarity.microsoft.com/)

## Installation

```
Install-Package Soenneker.Blazor.Clarity
```

## Usage

1. Register the interop within DI:

```csharp
public static async Task Main(string[] args)
{
    ...
    builder.Services.AddClarity();
}
```

2. Inject `IClarityInterop` within your `App.Razor` file.


```csharp
    @inject IClarityInterop ClarityInterop
```


3. Call ASAP within `OnInitializedAsync` within `App.Razor` with your Clarity project key.

```csharp
protected override async Task OnInitializedAsync()
{
    await ClarityInterop.Init("your-key-here");
    ...
}
```