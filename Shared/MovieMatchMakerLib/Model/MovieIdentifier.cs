using System;
using System.ComponentModel.DataAnnotations;

namespace MovieMatchMakerLib.Model
{
    public class MovieIdentifier
    {
        [Required]
        [StringLength(200)]
        [RegularExpression("[A-Za-z0-9\\p{L}\\p{N} :']+")]   // > 0 Ascii letters & numbers, Unicode numbers and letters and space only
        public string Title { get; set; }

        [Required]
        [Range(1888, 2023)]
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
