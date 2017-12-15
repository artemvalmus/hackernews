using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNews
{
    public class DataFetcher : IDisposable
    {
        private const string BaseUrl = "https://hacker-news.firebaseio.com/v0/";

        private bool disposed;

        private readonly HttpClient _webClient;

        public DataFetcher()
        {
            _webClient = new HttpClient();
        }

        public async Task<IEnumerable<Post>> GetPosts(int count)
        {
            var result = new List<Post>();
            var ids = await GetTopPostIds();

            var maxCount = ids.Count() < count ? ids.Count() : count;

            for (int i = 0; i < maxCount; i++)
            {
                var json = JObject.Parse(await LoadString($"{BaseUrl}item/{ids[i]}.json"));

                result.Add(new Post
                {
                    Author = json["by"]?.ToString(),
                    Comments = (int?)json["descendants"],
                    Points = (int?)json["score"],
                    Title = json["title"]?.ToString(),
                    Uri = json["url"]?.ToString(),
                    Rank = i + 1
                });
            }

            return result;
        }

        private async Task<int[]> GetTopPostIds()
        {
            var ids = await LoadString($"{BaseUrl}topstories.json");

            if (string.IsNullOrEmpty(ids))
            {
                throw new Exception(
                    "Error occurred while processing the request. Please try again later.");
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(ids);
        }

        private async Task<string> LoadString(string url)
        {
            return await _webClient.GetStringAsync(url);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                _webClient.Dispose();
            }

            disposed = true;
        }
    }
}
