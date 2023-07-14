using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;

namespace MovieMatchMakerLib
{
    public class MovieConnectionBuilder : MovieConnectionBuilderBase
    {
        public MovieConnectionBuilder(IDataCache dataCache)
            : base(dataCache)
        {           
        }

        public override async Task FindMovieConnections()
        {
            foreach (var sourceMovie in _dataCache.Movies)
            {
                var sourceMoviesCredits = await _dataCache.GetCreditsForMovieAsync(sourceMovie.MovieId);
                foreach (var sourceRole in sourceMoviesCredits.Credits.Cast)
                {
                    await FindMovieConnectionsFromRole(sourceMovie, sourceRole);
                }
                foreach (var sourceRole in sourceMoviesCredits.Credits.Crew)
                {
                    await FindMovieConnectionsFromRole(sourceMovie, sourceRole);
                }
            }
        }      
    }
}
