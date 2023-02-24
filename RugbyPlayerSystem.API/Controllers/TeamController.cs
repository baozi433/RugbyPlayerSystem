using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RugbyPlayerSystem.API.Repository.Contract;
using RugbyPlayerSystem.Models;

namespace RugbyPlayerSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;

        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpPost("add_team")]
        public async Task<ActionResult<Team>> AddTeam(Team team)
        {
            try
            {
                if(team == null)
                {
                    return BadRequest("The team is null");
                }

                var checkTeam = await _teamRepository.GetTeam(team.Name);

                if (checkTeam != null)
                {
                    return Conflict($"The team {team.Name} already exists");
                }

                var addedTeam = await _teamRepository.AddTeam(team);

                return Ok(addedTeam);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("get_all_teams")]
        public async Task<ActionResult<List<Team>>> GetTeams()
        {
            try
            {
                var teams = await _teamRepository.GetTeams();

                if(teams == null)
                {
                    return NotFound("No teams");
                }

                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
