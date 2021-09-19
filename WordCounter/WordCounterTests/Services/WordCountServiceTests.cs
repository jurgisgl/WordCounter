using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using WordCounter.Options;
using WordCounter.Services;
using WordCounter.Services.Base;
using Xunit;

namespace WordCounterTests.Services
{
    public class WordCountServiceTests
    {
        private readonly Mock<IHashService> hashService = new Mock<IHashService>();
        private readonly IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        private readonly WordCountOptions options = new WordCountOptions();

        [Fact]
        public async void CountWordsTest()
        {
            this.hashService.Setup(x => x.GetHash(It.IsAny<string>())).Returns("hash");
            var service = new WordCountService(this.cache, this.hashService.Object, Options.Create(this.options));
            var words = await service.CountWords("“Ryga yra Latvijos sostinė. Rīga yra latvijos sostine.\nSostinę aplanko daug turistų.“");
            Assert.Equal(7, words.Count);
            Assert.Equal(3, words.FirstOrDefault(w => w.Word == "sostinė")?.Count);
            Assert.Equal(2, words.FirstOrDefault(w => w.Word == "Ryga")?.Count);
            Assert.Equal(2, words.FirstOrDefault(w => w.Word == "yra")?.Count);
            Assert.Equal(2, words.FirstOrDefault(w => w.Word == "Latvijos")?.Count);
            Assert.Equal(1, words.FirstOrDefault(w => w.Word == "aplanko")?.Count);
            Assert.Equal(1, words.FirstOrDefault(w => w.Word == "daug")?.Count);
            Assert.Equal(1, words.FirstOrDefault(w => w.Word == "turistų")?.Count);
            for (var i = 0; i < words.Count - 1; i++)
            {
                Assert.True(words[i].Count >= words[i + 1].Count);
            }

            Assert.NotNull(this.cache.Get("hash"));
        }

        [Fact]
        public void GetWordsTest()
        {
            var service = new WordCountService(this.cache, this.hashService.Object, Options.Create(this.options));
            var words = service.GetWords("“Ryga yra Latvijos sostinė. Rīga yra latvijos sostine.\nSostinę aplanko daug turistų.“");
            Assert.Equal(12, words.Length);
            Assert.Equal("Ryga", words[0]);
            Assert.Equal("yra", words[1]);
            Assert.Equal("Latvijos", words[2]);
            Assert.Equal("sostinė", words[3]);
            Assert.Equal("Rīga", words[4]);
            Assert.Equal("yra", words[5]);
            Assert.Equal("latvijos", words[6]);
            Assert.Equal("sostine", words[7]);
            Assert.Equal("Sostinę", words[8]);
            Assert.Equal("aplanko", words[9]);
            Assert.Equal("daug", words[10]);
            Assert.Equal("turistų", words[11]);
        }

        [Fact]
        public void GetWordsWithApostropheTest()
        {
            var service = new WordCountService(this.cache, this.hashService.Object, Options.Create(this.options));
            var words = service.GetWords("It's");
            Assert.Equal(1, words.Length);
        }
    }
}