[![NuGet Version](https://img.shields.io/nuget/v/Soenneker.Blazor.Clarity.svg?style=flat)](https://www.nuget.org/packages/Soenneker.Blazor.Clarity/)

# Soenneker.Blazor.Clarity
### A small Blazor interop library that sets up [Microsoft Clarity](https://clarity.microsoft.com/)

## Installation

```
Install-Package Soenneker.Blazor.Clarity
```

## Usage

1. Insert this in your `index.html` as one of the very first scripts

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

3. Add the following statement to your `_Imports.razor`
```csharp
@using Soenneker.Blazor.Clarity
```

4. Inject `IClarityInterop` within your `App.Razor` file


```csharp
@inject IClarityInterop ClarityInterop
```


5. Call ASAP within `OnInitializedAsync` within `App.Razor` using your Clarity project key

```csharp
protected override async Task OnInitializedAsync()
{
    await ClarityInterop.Init("your-key-here");
    ...
}
```