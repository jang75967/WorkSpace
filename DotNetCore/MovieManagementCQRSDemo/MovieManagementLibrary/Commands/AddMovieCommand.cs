using MediatR;
using MovieManagementLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagementLibrary.Commands
{
    public record AddMovieCommand(MovieModel model) : IRequest<MovieModel>;
}
