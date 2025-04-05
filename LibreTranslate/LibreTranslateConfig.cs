namespace LibreTranslate
{
    public class LibreTranslateConfig
    {
        public static readonly string Configuration = "LibreTranslateConfig";
        public string ApiAddress { get; set; }
        public string ApiKey { get; set; }
        public string Source { get; set; }
        public string TargetPath { get; set; }
    }
}
