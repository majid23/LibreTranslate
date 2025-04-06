# 📌 کتابخانه ای برای پروژه های چند زبانه با ترجمه خودکار آنلاین با ذخیره سازی با استفاده از LibreTranslate 

با استفاده از این کتابخانه و LibreTranslate پروزه های خود را بدون نیاز به ترجمه به زبانهای مختلف می توانید به صورت آفلاین یا آنلاین و اتومات پروژه به صورت چند زبانه تبدیل شود.

---

## 🔧 ویژگی‌ها (Features)

- ✅ پروژه را فقط به یک زبان ایجاد کنید کتابخانه پروژه شما را چند زبانه میکند
- 📂 ایجاد فایلهای ترجمه برای هر زبان
- 🌐 آنلاین و به صورت اتومات و یا آفلاین
- 🍂 قابلیت تغییر ترجمه ها حتی بعد از پیاده سازی
- 🗝️ امکان تعریف کلید

---

## 🚀 شروع سریع (Getting Started)

### نصب

1. پکیج را از ناگت دانلود یا نصب کنید([nuget.org](https://www.nuget.org/packages/LibreTranslate)):

```bash
dotnet add package LibreTranslate --version 1.0.0
```

---

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

## 🚀 نحوه استفاده (How to use)


```bash

```
