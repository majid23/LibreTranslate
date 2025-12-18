using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace LibreTranslate
{
    public class LibreStringLocalizer : ILibreStringLocalizer
    {
        private readonly LibreTranslateService _translateService;
        private readonly LibreTranslateConfig _config;
        private readonly IMemoryCache _memoryCache;

        // برای جلوگیری از دسترسی همزمان مخرب به فایل و دیکشنری داخلی
        private static readonly object _fileLock = new object();

        // فرهنگ => (متن اصلی => ترجمه)
        private static Dictionary<string, Dictionary<string, string>> _fileBasedDictionary
            = new Dictionary<string, Dictionary<string, string>>();

        // زمان انقضای کش در حافظه (مثلاً 24 ساعت)
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(24);

        public LibreStringLocalizer(
            LibreTranslateService translateService,
            IOptions<LibreTranslateConfig> config,
            IMemoryCache memoryCache)
        {
            _translateService = translateService;
            _config = config.Value;
            _memoryCache = memoryCache;
        }

        public LocalizedString this[string name] => GetLocalizedString(name);

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var baseText = GetLocalizedString(name).Value;
                var formatted = string.Format(baseText, arguments);
                return new LocalizedString(name, formatted, false);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return new List<LocalizedString>();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            CultureInfo.CurrentUICulture = culture;
            return this;
        }

        private LocalizedString GetLocalizedString(string originalText)
        {
            if (string.IsNullOrEmpty(originalText))
                return new LocalizedString(string.Empty, string.Empty, true);

            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var cacheKey = $"{culture}:{originalText}";

            // تلاش برای گرفتن از کش حافظه
            if (_memoryCache.TryGetValue(cacheKey, out string cachedTranslation))
            {
                return new LocalizedString(originalText, cachedTranslation, false);
            }

            // سراغ دیکشنری فایل
            string translationFromFile = GetTranslationFromFileDictionary(culture, originalText);
            if (translationFromFile != null)
            {
                // در کش قرار بده
                _memoryCache.Set(cacheKey, translationFromFile, _cacheDuration);
                return new LocalizedString(originalText, translationFromFile, false);
            }

            // اگر در فایل نبود، سرویس بیرونی را فراخوانی کن
            var translated = _translateService
                .TranslateAsync(originalText, _config.Source, culture)
                .GetAwaiter()
                .GetResult();

            if (!string.IsNullOrWhiteSpace(translated))
            {
                translated = translated.Trim();
                if (translated.Contains("؟")) translated = translated.Replace("?", "");
                UpdateFileDictionaryAndSave(culture, originalText, translated);
                _memoryCache.Set(cacheKey, translated, _cacheDuration);

                return new LocalizedString(originalText, translated, false);
            }

            // اگر نشد، همان متن اصلی
            return new LocalizedString(originalText, originalText, true);
        }

        private string GetTranslationFromFileDictionary(string culture, string originalText)
        {
            lock (_fileLock)
            {
                if (!_fileBasedDictionary.ContainsKey(culture))
                {
                    _fileBasedDictionary[culture] = LoadDictionaryFromFile(culture);
                }
                var dictForCulture = _fileBasedDictionary[culture];
                if (dictForCulture.TryGetValue(originalText, out var val))
                {
                    return val;
                }
                else
                {
                    return null;
                }
            }
        }

        private void UpdateFileDictionaryAndSave(string culture, string originalText, string translated)
        {
            lock (_fileLock)
            {
                if (!_fileBasedDictionary.ContainsKey(culture))
                {
                    _fileBasedDictionary[culture] = LoadDictionaryFromFile(culture);
                }
                _fileBasedDictionary[culture][originalText] = translated;
                SaveDictionaryToFile(culture, _fileBasedDictionary[culture]);
            }
        }

        private Dictionary<string, string> LoadDictionaryFromFile(string culture)
        {
            var filePath = GetFilePathForCulture(culture);
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, string>();
            }

            try
            {
                var json = File.ReadAllText(filePath);
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                return dict ?? new Dictionary<string, string>();
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }

        private void SaveDictionaryToFile(string culture, Dictionary<string, string> dictionary)
        {
            var filePath = GetFilePathForCulture(culture);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            var json = JsonSerializer.Serialize(dictionary);
            File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);
        }

        private string GetFilePathForCulture(string culture)
        {
            // پوشه Translations در ریشه پروژه
            return Path.Combine(_config.TargetPath, $"{culture}.json");
        }

        //public LocalizedString GetString(string name)
        //{
        //    return GetLocalizedString(name);
        //}

        //public LocalizedString GetString(string name, params object[] arguments)
        //{
        //    var baseText = GetLocalizedString(name).Value;
        //    var formatted = string.Format(baseText, arguments);
        //    return new LocalizedString(name, formatted, false);
        //}
    }
}
