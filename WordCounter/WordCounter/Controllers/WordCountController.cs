using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WordCounter.Exceptions;
using WordCounter.Services.Base;

namespace WordCounter.Controllers
{
    [Route("api/v1")]
    [SwaggerTag("The Word Counter")]
    public class WordCountController : ControllerBase
    {
        #region Fields

        private readonly IWordCountService wordCountService;

        #endregion

        #region Constructors

        public WordCountController(IWordCountService wordCountService)
        {
            this.wordCountService = wordCountService;
        }

        #endregion

        #region Public Members

        [HttpGet("word-count/{text}")]
        [SwaggerOperation(Summary = "Count words in the given text",
            Description = "Calculation is case insensitive and accent insensitive. Provides distinct result sorted by count.")]
        public async Task<IActionResult> GetWordCount(
            [SwaggerParameter(Description = "The text in which the words should be counted")] [FromRoute]
            string text
        )
        {
            try
            {
                return this.Ok(await this.wordCountService.CountWords(text));
            }
            catch (WordCounterException e)
            {
                return this.BadRequest(e.Message);
            }
        }

        #endregion
    }
}