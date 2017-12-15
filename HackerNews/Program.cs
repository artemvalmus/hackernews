using Mono.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace HackerNews
{
    class Program
    {
        private const int MaxCount = 100;

        private static int? _postsCount;
        private static bool _showHelp = false;
        private static OptionSet _optionSet;

        static Program()
        {
            _optionSet = new OptionSet()
            {
                {
                    "p|posts=", "the maximum number of posts to print. A positive integer <= 100.",
                    x =>
                    {
                        if (int.TryParse(x, out int value))
                        {
                            _postsCount = value;
                        }
                    }
                },
                {
                    "h|help", "show help", x => _showHelp = x != null
                }
            };
        }

        static void Main(string[] args)
        {
            ParseInputArguments(args);

            if (_postsCount == null || _postsCount > MaxCount || _showHelp)
            {
                ShowHelp();
                return;
            }

            try
            {
                LoadAndPrintPosts(_postsCount.Value);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error occurred while processing the request. Message: { ex.ToString() }");
            }
        }

        private static void LoadAndPrintPosts(int count)
        {
            var validator = new Validator();
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            using (var dataFetcher = new DataFetcher())
            {
                var posts = dataFetcher.GetPosts(count).Result;
                var validPosts = posts.Where(x => validator.Validate(x));

                var json = JsonConvert.SerializeObject(validPosts, jsonSettings);
                var formatted = JToken.Parse(json).ToString(Formatting.Indented);

                Console.WriteLine(formatted);
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Options:");
            _optionSet.WriteOptionDescriptions(Console.Out);
        }

        private static void ParseInputArguments(string[] args)
        {
            try
            {
                _optionSet.Parse(args);
            }
            catch (OptionException ex)
            {
                Console.WriteLine($"Error occurred while parsing passed arguments. Message: { ex.ToString() }");
                Console.WriteLine("Try `hackernews --help` for more information.");
                return;
            }
        }
    }
}
