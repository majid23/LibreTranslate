# 📌 کتابخانه ای برای پروژه های چند زبانه با ترجمه خودکار آنلاین با ذخیره سازی با استفاده از LibreTranslate 

با استفاده از این کتابخانه و LibreTranslate پروزه های خود را بدون نیاز به ترجمه به زبانهای مختلف می توانید به صورت آفلاین یا آنلاین و اتومات پروژه به صورت چند زبانه تبدیل شود.

---

## 🔧 ویژگی‌ها (Features)

- ✅ پروژه را فقط به یک زبان ایجاد کنید کتابخانه پروژه شما را چند زبانه میکند
- 📂 ایجاد فایلهای ترجمه برای هر زبان
- 🌐 آنلاین و به صورت اتومات و یا آفلاین
- 🍂 قابلیت تغییر ترجمه ها حتی بعد از پیاده سازی
- 🗝️ امکان تعریف کلید
- 🗄️ ذخیره ترجمه ها در فایل
- 🔂 خواندن دفعات بعد از فایل ترجمه
- 💡 استفاده از کش برای جلوگیری از خواندن متعدد ترجمه ها از فایل

---

## 🚀 شروع سریع (Getting Started)

### نصب

1. پکیج را از ناگت دانلود یا نصب کنید([nuget.org](https://www.nuget.org/packages/LibreTranslate)):

```bash
dotnet add package LibreTranslate --version 1.0.0
```

2. بعد از نصب پکیج می توانید در قسمت های مختلف پروژه به شکل زیر استفاده کنید
---

### افزودن وابستگی
```bash
// Add HttpClient for LibreTranslateService
builder.Services.AddHttpClient<LibreTranslateService>();

// Add memory cache
builder.Services.AddMemoryCache();

// Register localizer
builder.Services.AddScoped<ILibreStringLocalizer, LibreStringLocalizer>();
```

### استفاده در سرویس
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

### استفاده در PageModel
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

### استفاده در Html
```bash
@page
@using LibreTranslate
@model IndexModel
@inject ILibreStringLocalizer localizer

<h2>@localizer["Welcome to my website"]</h2>

<p>@Model.TranslatedText</p>
```

## ⚙️ تنظیمات (Configuration)

### کلاس LibreTranslateConfig

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

###  فایل جیسون
```bash
"LibreTranslateConfig": {
  "ApiAddress": "https://libretranslate.com/translate",
  "ApiKey": "",
  "Source": "en",
  "TargetPath": "translations"
}
```

---

    

