using System.Collections.Generic;
using System.Threading.Tasks;
using WordCounter.Models;

namespace WordCounter.Services.Base
{
    public interface IWordCountService
    {
        #region Public Members

        Task<List<WordCount>> CountWords(string text);

        #endregion
    }
}