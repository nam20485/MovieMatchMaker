namespace MovieMatchMakerLib.Model
{
    public class TvShow : Production
    {
        public TvShow(string name, int firstAirYear, int apiId, string posterImagePath)
            : base(name, firstAirYear, apiId, posterImagePath)
        {         
        }
    }
}