using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WordCounter.Controllers;
using WordCounter.Exceptions;
using WordCounter.Models;
using WordCounter.Services.Base;
using Xunit;

namespace WordCounterTests.Controllers
{
    public class WordCountControllerTests
    {
        private readonly DefaultHttpContext httpContext = new DefaultHttpContext();
        private readonly Mock<IWordCountService> wordCountService = new Mock<IWordCountService>();

        private WordCountController CreateController()
        {
            return new WordCountController(this.wordCountService.Object)
            {
                ControllerContext = {HttpContext = this.httpContext}
            };
        }

        [Fact]
        public async void GetWordCountExceptionTests()
        {
            this.wordCountService.Setup(x => x.CountWords(It.IsAny<string>())).ThrowsAsync(new WordCounterException("message"));
            var controller = this.CreateController();
            var res = await controller.GetWordCount("test");
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async void GetWordCountTests()
        {
            this.wordCountService.Setup(x => x.CountWords(It.IsAny<string>())).ReturnsAsync(new List<WordCount> {new WordCount()});
            var controller = this.CreateController();
            var res = await controller.GetWordCount("test");
            Assert.IsType<OkObjectResult>(res);
        }
    }
}