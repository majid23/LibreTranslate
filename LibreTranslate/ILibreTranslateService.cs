using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibreTranslate
{
    public interface ILibreTranslateService
    {
        Task<string> TranslateAsync(string text, string source, string target);
    }
}
