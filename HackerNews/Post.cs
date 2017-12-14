using System.ComponentModel.DataAnnotations;

namespace HackerNews
{
    public class Post
    {
        [Required, MaxLength(256)]
        public string Title { get; set; }

        [Required, Url]
        public string Uri { get; set; }

        [Required, MaxLength(256)]
        public string Author { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int? Points { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int? Comments { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int? Rank { get; set; }
    }
}
