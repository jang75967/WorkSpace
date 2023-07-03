using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieManagementLibrary.Commands;
using MovieManagementLibrary.Models;
using MovieManagementLibrary.Query;

namespace MovieManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MoviesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        public async Task<List<MovieModel>> Get()
        {
            // 중재자 패턴
            return await _mediator.Send(new GetMovieListQuery());
        }

        [HttpGet("{id}")]
        public async Task<MovieModel> Get(int id)
        {
            return await _mediator.Send(new GetMovieByIdQuery(id));
        }

        [HttpPost]
        public async Task<MovieModel> Post(MovieModel movieModel)
        {
            return await _mediator.Send(new AddMovieCommand(movieModel));
        }
    }
}
