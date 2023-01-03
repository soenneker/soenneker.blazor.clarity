[![NuGet Version](https://img.shields.io/nuget/v/Soenneker.Blazor.Clarity.svg?style=flat)](https://www.nuget.org/packages/Soenneker.Blazor.Clarity/)

# Soenneker.Blazor.Clarity
### A small Blazor interop library that sets up [Microsoft Clarity](https://clarity.microsoft.com/)

## Installation

```
Install-Package Soenneker.Blazor.Clarity
```

## Usage

1. Insert the script in `wwwroot/index.html` at the bottom of your `<body>` but before the other scripts

```html
<script src="_content/Soenneker.Blazor.Clarity/clarity.js"></script>
```

2. Register the interop within DI (`Program.cs`)

```csharp
public static async Task Main(string[] args)
{
    ...
    builder.Services.AddClarity();
}
```

3. Inject `IClarityInterop` within your `App.Razor` file


```csharp
@using Soenneker.Blazor.Clarity.Abstract
@inject IClarityInterop ClarityInterop
```


4. Initialize the interop in `OnInitializedAsync` within `App.Razor` using your Clarity project key

```csharp
protected override async Task OnInitializedAsync()
{
    await ClarityInterop.Init("your-key-here");
    ...
}
```