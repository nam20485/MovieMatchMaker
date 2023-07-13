using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieMatchMakerLib
{
    public class MovieConnectionBuilder
    {
        private readonly IDataCache _dataCache;
        public MovieConnection.List MovieConnections { get; }

        public MovieConnectionBuilder(IDataCache dataCache)
        {
            MovieConnections = new MovieConnection.List();
            _dataCache = dataCache;
        }

        public async Task FindMovieConnections()
        {
            foreach (var sourceMovie in _dataCache.Movies)
            {
                var sourceMoviesCredits = await _dataCache.GetCreditsForMovieAsync(sourceMovie.MovieId);
                foreach (var sourceRole in sourceMoviesCredits.Credits.Cast)
                {
                    var personCredits = await _dataCache.GetMovieCreditsForPersonAsync(sourceRole.Id);
                    if (personCredits != null)
                    {
                        foreach (var targetRole in personCredits.MovieCredits.Cast)
                        {
                            if (targetRole.ReleaseDate.HasValue)
                            {
                                var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                                if (targetMovie != null)
                                {
                                    if (targetMovie != sourceMovie)
                                    {
                                        var movieConnection = GetMovieConnection(sourceMovie, targetMovie);
                                        movieConnection.TargetMovie = targetMovie;
                                        var connectedRole = new ConnectedRole
                                        {
                                            Name = new Name(sourceRole.Name),
                                            SourceJob = sourceRole.Character,
                                            TargetJob = targetRole.Character
                                        };
                                        movieConnection.ConnectedRoles.Add(connectedRole);
                                        if (sourceMovie.Title == targetMovie.Title &&
                                            sourceMovie.Title == "Dark City")
                                        {
                                            Console.WriteLine();
                                        }
                                    }
                                }
                            }
                        }
                        foreach (var targetRole in personCredits.MovieCredits.Crew)
                        {
                            if (targetRole.ReleaseDate.HasValue)
                            {
                                var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                                if (targetMovie != null)
                                {
                                    if (targetMovie != sourceMovie)
                                    {
                                        var movieConnection = GetMovieConnection(sourceMovie, targetMovie);
                                        movieConnection.TargetMovie = targetMovie;
                                        var connectedRole = new ConnectedRole
                                        {
                                            Name = new Name(sourceRole.Name),
                                            SourceJob = sourceRole.Character,
                                            TargetJob = targetRole.Job
                                        };
                                        movieConnection.ConnectedRoles.Add(connectedRole);
                                        if (sourceMovie.Title == targetMovie.Title &&
                                            sourceMovie.Title == "Dark City")
                                        {
                                            Console.WriteLine();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var sourceRole in sourceMoviesCredits.Credits.Crew)
                {
                    var personCredits = await _dataCache.GetMovieCreditsForPersonAsync(sourceRole.Id);
                    if (personCredits != null)
                    {
                        foreach (var targetRole in personCredits.MovieCredits.Crew)
                        {
                            if (targetRole.ReleaseDate.HasValue)
                            {
                                var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                                if (targetMovie != null)
                                {
                                    if (targetMovie != sourceMovie)
                                    {
                                        var movieConnection = GetMovieConnection(sourceMovie, targetMovie);
                                        movieConnection.TargetMovie = targetMovie;
                                        var connectedRole = new ConnectedRole
                                        {
                                            Name = new Name(sourceRole.Name),
                                            SourceJob = sourceRole.Job,
                                            TargetJob = targetRole.Job
                                        };
                                        movieConnection.ConnectedRoles.Add(connectedRole);
                                        if (sourceMovie.Title == targetMovie.Title &&
                                            sourceMovie.Title == "Dark City")
                                        {
                                            Console.WriteLine();
                                        }
                                    }
                                }
                            }
                        }
                        foreach (var targetRole in personCredits.MovieCredits.Cast)
                        {
                            if (targetRole.ReleaseDate.HasValue)
                            {
                                var targetMovie = await _dataCache.GetMovieAsync(targetRole.Title, targetRole.ReleaseDate.Value.Year);
                                if (targetMovie != null)
                                {
                                    if (targetMovie != sourceMovie)
                                    {
                                        var movieConnection = GetMovieConnection(sourceMovie, targetMovie);
                                        movieConnection.TargetMovie = targetMovie;
                                        movieConnection.ConnectedRoles.Add(new ConnectedRole()
                                        {
                                            Name = new Name(sourceRole.Name),
                                            SourceJob = sourceRole.Job,
                                            TargetJob = targetRole.Character
                                        });
                                        if (sourceMovie.Title == targetMovie.Title &&
                                            sourceMovie.Title == "Dark City")
                                        {
                                            Console.WriteLine();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private MovieConnection GetMovieConnection(Movie sourceMovie, Movie targetMovie)
        {
            var movieConnection = MovieConnections.Find(mc =>
            {
                if (mc.SourceMovie == sourceMovie && mc.TargetMovie == targetMovie)
                {
                    return true;
                }
                //else if (mc.SourceMovie == targetMovie && mc.TargetMovie == sourceMovie)
                //{
                //    return true;
                //}
                else
                {
                    return false;
                }
            });

            if (movieConnection is null)
            {
                movieConnection = new MovieConnection(sourceMovie, targetMovie);
                MovieConnections.Add(movieConnection);
            }

            return movieConnection;
        }
    }
}
