using MediatR;
using MovieManagementLibrary.Models;
using MovieManagementLibrary.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagementLibrary.Handlers
{
    public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, MovieModel>
    {
        private readonly IMediator _mediator;

        public GetMovieByIdHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<MovieModel> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            // 중재자 패턴
            var movies = await _mediator.Send(new GetMovieListQuery());
            var movie = movies.FirstOrDefault(u => u.Id == request.id);
            return movie;
        }
    }
}
