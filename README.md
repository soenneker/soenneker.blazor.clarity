[![](https://img.shields.io/nuget/v/Soenneker.Blazor.Clarity.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Clarity/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.clarity/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.clarity/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Blazor.Clarity.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.Clarity/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.Clarity
### A Blazor interop library that sets up [Microsoft Clarity](https://clarity.microsoft.com/)

## Installation

```
dotnet add package Soenneker.Blazor.Clarity
```

## Usage

1. Register the interop within DI (`Program.cs`)

```csharp
public static async Task Main(string[] args)
{
    ...
    builder.Services.AddClarity();
}
```

2. Inject `IClarityInterop` within your `App.Razor` file

```csharp
@using Soenneker.Blazor.Clarity.Abstract
@inject IClarityInterop ClarityInterop
```

3. Initialize the interop in `OnInitializedAsync` within `App.Razor` using your Clarity project key

```csharp
protected override async Task OnInitializedAsync()
{
    await ClarityInterop.Init("your-key-here");
    ...
}
```