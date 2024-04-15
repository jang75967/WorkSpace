using EntityFrameworkCore.Client.Features.Teams.Commands;
using EntityFrameworkCore.Client.Features.Teams.Queries;
using EntityFrameworkCore.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkCore.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : Controller
    {
        private readonly ILogger<TeamsController> _logger;
        private readonly IMediator _mediator;

        public TeamsController(
            ILogger<TeamsController> logger, 
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetTeamss()
        {
            var teams = await _mediator.Send(new GetTeamsQuery());
            return Ok(teams);
        }

        [HttpGet("{id:int}", Name = "GetTeamsById")]
        public async Task<ActionResult> GetTeamsById(int id)
        {
            var team = await _mediator.Send(new GetTeamByIdQuery(id));
            return Ok(team);
        }

        [HttpPost]
        public async Task<ActionResult> AddTeam([FromBody] Team team)
        {
            await _mediator.Send(new AddTeamCommand(team));
            return StatusCode(201);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Team team)
        {
            await _mediator.Send(new UpdateTeamCommand(team.TeamId, team.Name!));
            return StatusCode(201);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteTeamCommand(id));
            return StatusCode(204);
        }
    }
}
