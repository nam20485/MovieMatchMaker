using MovieMatchMakerLib.Data;

namespace MovieMatchMakerLib
{
    public class PoolThreadedMovieDataBuilder : ThreadedMovieDataBuilder
    {
        public PoolThreadedMovieDataBuilder(IDataSource dataSource)
            : base(dataSource)
        {
        }

        protected override void ProcessMovieCreditsRequestAsync(MovieCreditsRequest request)
        {
            base.ProcessMovieCreditsRequestAsync(request);
        }

        protected override void ProcessMovieRequestAsync(MovieRequest movieRequest)
        {
            base.ProcessMovieRequestAsync(movieRequest);
        }

        protected override void ProcessPersonCreditsRequestAsync(PersonCreditsRequest request)
        {
            base.ProcessPersonCreditsRequestAsync(request);
        }
    }
}
