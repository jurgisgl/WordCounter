namespace WordCounter.Exceptions
{
    public class TextEmptyException : WordCounterException
    {
        #region Constructors

        public TextEmptyException(string message) : base(message)
        {
        }

        #endregion
    }
}