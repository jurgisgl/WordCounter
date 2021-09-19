using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WordCounter.Exceptions;
using WordCounter.Models;
using WordCounter.Options;
using WordCounter.Services.Base;

namespace WordCounter.Services
{
    public class WordCountService : IWordCountService
    {
        #region Fields

        private readonly IMemoryCache cache;
        private readonly IHashService hashService;
        private readonly WordCountOptions options;

        #endregion

        #region Constructors

        public WordCountService(IMemoryCache cache, IHashService hashService, IOptions<WordCountOptions> options)
        {
            this.cache = cache;
            this.hashService = hashService;
            this.options = options.Value;
        }

        #endregion

        #region Public Members

        public Task<List<WordCount>> CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new TextEmptyException("The text does not contain any words");
            }

            if (text.Length > this.options.MaxTextLength)
            {
                throw new TextTooLongException($"The length of the text cannot exceed {this.options.MaxTextLength}");
            }

            var hash = this.hashService.GetHash(text);
            if (this.cache.TryGetValue(hash, out List<WordCount> wordCountList))
            {
                return Task.FromResult(wordCountList);
            }

            var words = this.GetWords(text);
            wordCountList = this.GetWordCount(words);
            this.cache.Set(hash, wordCountList, TimeSpan.FromSeconds(this.options.TextCacheDurationInSeconds));
            return Task.FromResult(wordCountList);
        }

        /*
         This would be a better solution, but the problem was that the method RemoveDiacritics
         doesn't work well for LT culture ("Rīga" becomes "Riga" which isn't equal to "Ryga").
         After some time I gave up searching and switched to another solution which would run 
         slowlier with larger texts, but it works for LT culture.

         private List<WordCount> GetWordCount(string[] words)
         {
             var wordMap = new Dictionary<string, WordCount>();
             foreach (var word in words)
             {
                 var modifiedWord = this.RemoveDiacritics(word).ToLower();
                 WordCount wordCount;
                 if (wordMap.ContainsKey(modifiedWord))
                 {
                     wordCount = wordMap[modifiedWord];
                 }
                 else
                 {
                     wordCount = new WordCount {Word = word};
                     wordMap.Add(modifiedWord, wordCount);
                 }
 
                 wordCount.Count++;
             }
             return wordMap.Values.OrderByDescending(w => w.Count).ToList();
         }
         private string RemoveDiacritics(string text)
         {
             var formD = text.Normalize(NormalizationForm.FormD);
             var sb = new StringBuilder();
             foreach (char ch in formD)
             {
                 var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                 if (uc != UnicodeCategory.NonSpacingMark)
                 {
                     sb.Append(ch);
                 }
             }
 
             return sb.ToString().Normalize(NormalizationForm.FormC);
         }
         */

        public string[] GetWords(string text)
        {
            var matches = Regex.Matches(text, @"\b[\w']*\b");

            var words = from m in matches
                where !string.IsNullOrEmpty(m.Value)
                select m.Value;

            return words.ToArray();
        }

        private List<WordCount> GetWordCount(string[] words)
        {
            /*
             Not the best solution, but replacing diacritics didn't work for LT culture.
             After some time spent on searching I didn't find a way how to replace diacritics.
             */
            var wordList = new List<WordCount>();
            foreach (var word in words)
            {
                var wordCount = wordList.FirstOrDefault(w =>
                    string.Compare(w.Word, word, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0);
                if (wordCount == null)
                {
                    wordCount = new WordCount {Word = word};
                    wordList.Add(wordCount);
                }

                wordCount.Count++;
            }

            return wordList.OrderByDescending(w => w.Count).ToList();
        }

        #endregion
    }
}