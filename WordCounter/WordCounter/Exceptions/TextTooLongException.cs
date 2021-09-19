namespace WordCounter.Exceptions
{
    public class TextTooLongException : WordCounterException
    {
        #region Constructors

        public TextTooLongException(string message) : base(message)
        {
        }

        #endregion
    }
}