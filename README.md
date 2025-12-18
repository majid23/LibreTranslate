# 📌 A Library for Multilingual Projects with Online Auto-Translation and Storage Using LibreTranslate 

Using this library and LibreTranslate, you can easily make your project multilingual without manually translating it into different languages. Whether offline or online, the project becomes multilingual automatically.

Thanks [LibreTranslate project](https://github.com/LibreTranslate/LibreTranslate) Free and Open Source Machine Translation API, entirely self-hosted. Unlike other APIs, it doesn't rely on proprietary providers such as Google or Azure to perform translations.
Using Docker, you can host the service on your server or locally.

---

## 🔧 Features

- ✅ Build your project in just one language — the library will handle the multilingual part.
- 📂 Generates translation files for each language.
- 🌐 Supports both automatic online translation and offline use.
- 🍂 Ability to edit translations even after implementation.
- 🗝️ Supports defining translation keys.
- 🗄️ Saves translations to file.
- 🔂 Reads translations from file on subsequent loads.
- 💡  Uses caching to avoid redundant file reads.

---

## 🚀 Getting Started

### Installation

1. Download or install the package via NuGet([nuget.org](https://www.nuget.org/packages/LibreTranslate)):

```bash
dotnet add package LibreTranslate --version 1.0.0
```

2. self-hoste with docker

### Docker
[Docker Documents](https://hub.docker.com/r/libretranslate/libretranslate)
```bash
docker run -d -p 5000:5000 --name libretranslate -v libretranslate_data:/app/data libretranslate/libretranslate --api-keys --req-limit 120
```

3. After installation, you can use it in different parts of your project as shown below.
---

### Dependency Injection
```bash
// Add HttpClient for LibreTranslateService
builder.Services.AddHttpClient<LibreTranslateService>();

// Add memory cache
builder.Services.AddMemoryCache();

// LibreTranslate config
services.Configure<LibreTranslateConfig>(
    config.GetSection(LibreTranslateConfig.Configuration)
);

// Register localizer
builder.Services.AddScoped<ILibreTranslateService, LibreTranslateService>();
builder.Services.AddScoped<ILibreStringLocalizer, LibreStringLocalizer>();
```

### Usage in a Service
```bash
public class MyService : IMyService
{
    private readonly ILibreStringLocalizer _localizer;

    public MyService(ILibreStringLocalizer localizer)
    {
        _localizer = localizer;
    }

    public void Do()
    {
        var translatedText = _localizer["Hello, how are you?"];
        var age = 18;
        var ageTranslatedText = _localizer["My age is {0}", age];
    }
}
```

### Usage in PageModel
```bash
public class IndexModel : PageModel
{
    private readonly ILibreStringLocalizer _localizer;

    public string TranslatedText { get; set; }

    public IndexModel(ILibreStringLocalizer localizer)
    {
        _localizer = localizer;
    }

    public void OnGet()
    {
        TranslatedText = _localizer["Hello, how are you?"];
    }
}
```

### Usage in HTML
```bash
@page
@using LibreTranslate
@model IndexModel
@inject ILibreStringLocalizer localizer

<h2>@localizer["Welcome to my website"]</h2>

<p>@Model.TranslatedText</p>
```

## ⚙️ Configuration

### LibreTranslateConfig Class

```bash
public class LibreTranslateConfig
{
    public static readonly string Configuration = "LibreTranslateConfig";
    public string ApiAddress { get; set; }
    public string ApiKey { get; set; }
    public string Source { get; set; }
    public string TargetPath { get; set; }
}
```

###  JSON Configuration
```bash
"LibreTranslateConfig": {
  "ApiAddress": "https://libretranslate.com/translate",
  "ApiKey": "",
  "Source": "en",
  "TargetPath": "translations"
}
```

---

    

