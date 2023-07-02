using CleanMovie.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanMovie.Application
{
    // Dependency Injection
    public interface IMovieRepository
    {
        List<Movie> GetAllMovies();
    }
}
