using System.ComponentModel.DataAnnotations;

using MovieMatchMakerLib.Utils;

namespace MovieMatchMakerLib.Model
{
    public class MovieIdentifier
    {
        [Required]
        [StringLength(256)]     // longest existing movie title is documented as ~188 characters
        [RegularExpression(@"[A-Za-z0-9\\p{L}\\p{N} :'\-!]+")]   // > 0 Ascii letters & numbers, Unicode numbers and letters and space only
        public string Title { get; set; }

        [Required]
        [YearRange()]           // [1, 2023] (earliest movie is documented as 1888)
        public int ReleaseYear { get; set; }

        public MovieIdentifier()
        {
            // required for Model binding
        }

        public MovieIdentifier(string title, int releaseYear)
        {            
            Title = title;
            ReleaseYear = releaseYear;
        }

    }
}
