using System;

namespace WordCounter.Exceptions
{
    public class WordCounterException : Exception
    {
        #region Constructors

        public WordCounterException(string message) : base(message)
        {
        }

        #endregion
    }
}