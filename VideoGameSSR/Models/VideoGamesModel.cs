using System.ComponentModel.DataAnnotations;

namespace VideoGameSSR.Models
{
    public class VideoGamesModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Publisher is required")]
        public string? Publisher { get; set; }

        [Required(ErrorMessage = "Release Year is required")]
        public int? ReleaseYear { get; set; }
    }
}
