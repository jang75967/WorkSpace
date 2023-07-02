using CleanMovie.Application.Interface;
using CleanMovie.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanMovie.Infrastructure
{
    public class MovieRepository : IMovieRepository
    {
        //public static List<Movie> Movies = new List<Movie>()
        //{
        //    new Movie {ID = 1, Name = "Passion of Christ", Cost = 2},
        //    new Movie {ID = 2, Name = "Home Alone 4", Cost = 1},
        //};

        private readonly MovieDbContext _movieDbContext;

        public MovieRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public Movie CraeteMovie(Movie movie)
        {
            _movieDbContext.Movies.Add(movie);
            _movieDbContext.SaveChanges();

            return movie;
        }

        public List<Movie> GetAllMovies()
        {
            //return Movies;

            return _movieDbContext.Movies.ToList();
        }
    }
}
