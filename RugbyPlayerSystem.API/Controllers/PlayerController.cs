using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RugbyPlayerSystem.API.Repository.Contract;
using RugbyPlayerSystem.Models;

namespace RugbyPlayerSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;

        public PlayerController(IPlayerRepository playerRepository, ITeamRepository teamRepository)
        {
            _playerRepository = playerRepository;
            _teamRepository = teamRepository;
        }

        [HttpPost("add_player")]
        public async Task<ActionResult<Player>> AddPlayer(Player player)
        {
            try
            {
                var addedPlayer = await _playerRepository.AddPlayer(player);

                if (addedPlayer == null)
                {
                    return NoContent();
                }

                return Created("addplayer", addedPlayer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("sign_player{playerId}_To/{teamName}")]
        public async Task<ActionResult<Player>> SignToATeam(int playerId, string teamName)
        {
            try
            {
                var signedPlayer = await _playerRepository.SignToATeam(playerId, teamName);

                if (signedPlayer == null)
                {
                    return NotFound("The player or Team does not exist");
                }

                return Ok(signedPlayer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("get_all_players")]
        public async Task<ActionResult<List<Player>>> GetPlayers()
        {
            try
            {
                var players = await _playerRepository.GetPlayers();

                if(players == null)
                {
                    return NotFound("There is no player");
                }

                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("get_player_signed_team/{playerId}")]
        public async Task<ActionResult<string>> GetPlayerSingedTeam(int playerId)
        {
            try
            {
                var player = await _playerRepository.GetPlayer(playerId);

                if(player == null)
                {
                    return NotFound("The player does not exist");
                }

                var teamName = player.TeamName;
                
                if(teamName == null)
                {
                    return BadRequest("The player has not yet signed to any team");
                }

                return Ok(teamName);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("get_players_by_team/{teamName}")]
        public async Task<ActionResult<List<Player>>> GetPlayersByTeams(string teamName)
        {
            try
            {
                var checkTeam = await _teamRepository.GetTeam(teamName);

                if(checkTeam == null)
                {
                    return BadRequest("The team does not exist");
                }

                var players = await _playerRepository.GetPlayersByTeam(teamName);

                if(players == null)
                {
                    return NoContent();
                }

                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("get_players_by_age/{age}")]
        public async Task<ActionResult<List<Player>>> GetPlayersByAge(int age)
        {
            try
            {
                var players = await _playerRepository.SearchPlayersByAge(age);

                if(players == null || players.Count == 0)
                {
                    return NotFound($"No player is {age} years old");
                }

                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("get_players_by_coach/{coach}")]
        public async Task<ActionResult<List<Player>>> GetPlayersByCoach(string coach)
        {
            try
            {
                var teams = await _teamRepository.SearchTeamsByCoach(coach);

                if(teams == null || teams.Count == 0)
                {
                    return NotFound($"The coach {coach} does not exist in any teams");
                }

                var players = new List<Player>();

                foreach(Team team in teams)
                {
                    var playersTmp = await _playerRepository.GetPlayersByTeam(team.Name);
                    players.AddRange(playersTmp);
                }

                if(players.Count == 0)
                {
                    return NotFound($"There is no player in {coach}'s teams");
                }

                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }

}
