using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HackerNews.IntegrationTests
{
    [TestClass]
    public class DataFetcherTest
    {
        private DataFetcher _dataFetcher;

        public DataFetcherTest()
        {
            _dataFetcher = new DataFetcher();
        }

        [TestMethod]
        public void GetPosts_ValidParameterPassed_ShouldReturnPosts()
        {
            var count = 15;
            var posts = _dataFetcher.GetPosts(count).Result;

            Assert.IsNotNull(posts);
            Assert.IsTrue(posts.Any() && posts.Count() == count);
        }
    }
}
