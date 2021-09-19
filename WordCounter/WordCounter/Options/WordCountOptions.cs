namespace WordCounter.Options
{
    public class WordCountOptions
    {
        #region Properties

        public int MaxTextLength { get; set; } = 500;

        public int TextCacheDurationInSeconds { get; set; } = 86_400;

        #endregion
    }
}