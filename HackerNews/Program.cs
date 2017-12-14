using Mono.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace HackerNews
{
    class Program
    {
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

            if (_postsCount == null || _showHelp)
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
                System.Console.WriteLine($"Error occurred while processing the request. Message: { ex.ToString() }");
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

                System.Console.WriteLine(formatted);
            }
        }

        private static void ShowHelp()
        {
            System.Console.WriteLine("Options:");
            _optionSet.WriteOptionDescriptions(System.Console.Out);
        }

        private static void ParseInputArguments(string[] args)
        {
            try
            {
                _optionSet.Parse(args);
            }
            catch (OptionException ex)
            {
                System.Console.WriteLine($"Error occurred while parsing passed arguments. Message: { ex.ToString() }");
                System.Console.WriteLine("Try `hackernews --help` for more information.");
                return;
            }
        }
    }
}
