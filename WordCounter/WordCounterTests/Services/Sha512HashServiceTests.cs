using WordCounter.Services;
using Xunit;

namespace WordCounterTests.Services
{
    public class Sha512HashServiceTests
    {
        [Fact]
        public void GetHashTests()
        {
            var service = new Sha512HashService();
            var res = service.GetHash("asdf");
            var res2 = service.GetHash("“Ryga yra Latvijos sostinė. Rīga yra latvijos sostine.\nSostinę aplanko daug turistų.“");
            Assert.NotNull(res);
            Assert.NotNull(res2);
            Assert.True(res.Length > 0);
            Assert.True(res2.Length > 0);
            Assert.NotEqual(res, res2);
        }
    }
}