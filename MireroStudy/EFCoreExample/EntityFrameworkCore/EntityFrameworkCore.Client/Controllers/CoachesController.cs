using EntityFrameworkCore.Client.Features.Coaches.Commands;
using EntityFrameworkCore.Client.Features.Coaches.Queries;
using EntityFrameworkCore.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCore.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoachesController : Controller
    {
        private readonly ILogger<CoachesController> _logger;
        private readonly IMediator _mediator;

        public CoachesController(
            ILogger<CoachesController> logger, 
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetCoaches()
        {
            var coaches = await _mediator.Send(new GetCoachesQuery());
            return Ok(coaches);
        }

        [HttpGet("{id:int}", Name = "GetCoachesById")]
        public async Task<ActionResult> GetCoachesById(int id)
        {
            var coach = await _mediator.Send(new GetCoachByIdQuery(id));
            return Ok(coach);
        }

        [HttpPost]
        public async Task<ActionResult> AddCoach([FromBody] Coach coach)
        {
            await _mediator.Send(new AddCoachCommand(coach));
            return StatusCode(201);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Coach coach)
        {
            await _mediator.Send(new UpdateCoachCommand(coach.Id, coach.Name!));
            return StatusCode(201);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCoachCommand(id));
            return StatusCode(204);
        }
    }
}
