using FakeItEasy;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RugbyPlayerSystem.API.Controllers;
using RugbyPlayerSystem.API.Repository.Contract;
using RugbyPlayerSystem.Models;
using System.Numerics;

namespace RugbyPlayerSystem.Test
{
    public class PlayerControllerTest
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;

        public PlayerControllerTest()
        {
            _playerRepository = A.Fake<IPlayerRepository>();
            _teamRepository = A.Fake<ITeamRepository>();
        }

        [Fact]
        public async void Add_Null_Player()
        {
            A.CallTo(() => _playerRepository.AddPlayer(null));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.AddPlayer(null);

            var result = actionResult.Result as BadRequestObjectResult;
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("The player is null", result.Value);
        }

        [Fact]
        public async void Add_Player_Return_Created()
        {
            var player = A.Fake<Player>();
            A.CallTo(() => _playerRepository.AddPlayer(player));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.AddPlayer(player);

            var result = actionResult.Result as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void Sign_To_A_Team_With_Invalid_Player_OR_Team_Return_NotFound()
        {
            var playerId = 1; 
            string teamName = null;
            A.CallTo(() => _playerRepository.SignToATeam(playerId, teamName)).Returns(Task.FromResult<Player>(null));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.SignToATeam(playerId, teamName);

            var result = actionResult.Result as NotFoundObjectResult;
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("The player or Team does not exist", result.Value);
        }

        [Fact]
        public async void Sign_To_A_Team_Return_OK()
        {
            var playerId = 1;
            var teamName = "Team Auckland";
            A.CallTo(() => _playerRepository.SignToATeam(playerId, teamName));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.SignToATeam(playerId, teamName);

            var result = actionResult.Result as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void Get_All_Players_Return_NotFound_When_Players_Null()
        {
            A.CallTo(() => _playerRepository.GetPlayers()).Returns(Task.FromResult<List<Player>>(null));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayers();

            var result = actionResult.Result as NotFoundObjectResult;
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("There is no player", result.Value);
        }

        [Fact]
        public async void Get_Player_Signed_Team_When_Player_IsNull()
        {
            var playerId = 1;
            A.CallTo(() => _playerRepository.GetPlayer(playerId)).Returns(Task.FromResult<Player>(null));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayerSingedTeam(playerId);

            var result = actionResult.Result as NotFoundObjectResult;
            Assert.Equal(404, result.StatusCode);
            Assert.Equal("The player does not exist", result.Value);
        }

        [Fact]
        public async void Get_Player_Signed_Team_When_Player_hasNoTeam()
        {
            var playerId = 1;
            var player = new Player { PlayerId = playerId, TeamName = null };
            A.CallTo(() => _playerRepository.GetPlayer(playerId)); 
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayerSingedTeam(playerId);

            var result = actionResult.Result as BadRequestObjectResult;
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("The player has not yet signed to any team", result.Value);
        }

        [Fact]
        public async void Get_Player_Signed_Team_ReturnOk()
        {
            A.CallTo(() => _playerRepository.GetPlayer(A<int>._)).Returns(new Player { PlayerId = 1, Name = "Player 1", TeamName = "Auckland"});
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayerSingedTeam(1);

            var result = actionResult.Result as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void Get_Players_By_Team_With_Invalid_Team()
        {
            var teamName = "The son of the Ocean";
            A.CallTo(() => _teamRepository.GetTeam(teamName)).Returns(Task.FromResult<Team>(null));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayersByTeam(teamName);

            var result = actionResult.Result as BadRequestObjectResult;
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("The team does not exist", result.Value);
        }

        [Fact]
        public async void Get_Players_By_Team_With_No_Player_Signed()
        {
            var teamName = "Wellington";
            A.CallTo(() => _playerRepository.GetPlayersByTeam(teamName)).Returns(Task.FromResult<List<Player>>(null));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayersByTeam(teamName);

            var result = actionResult.Result as NoContentResult;
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async void Get_Players_By_Team_ReturnOk()
        {
            var teamName = "Bay of Plenty";
            A.CallTo(() => _playerRepository.GetPlayersByTeam(teamName));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayersByTeam(teamName);

            var result = actionResult.Result as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async void Get_Players_By_Age_With_Null()
        {
            int age = 20;
            A.CallTo(() => _playerRepository.SearchPlayersByAge(age)).Returns(Task.FromResult<List<Player>>(null));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayersByAge(age);

            var result = actionResult.Result as NotFoundObjectResult;
            Assert.Equal(404, result.StatusCode);
            Assert.Equal($"No player is {age} years old", result.Value);
        }

        [Fact]
        public async void Get_Players_By_Age_ReturnOk()
        {
            A.CallTo(() => _playerRepository.SearchPlayersByAge(A<int>._)).Returns(Task.FromResult<List<Player>>(new List<Player>
            {
                new Player { PlayerId = 1, BirthDate = new DateOnly(2000, 1, 1) },
                new Player { PlayerId = 2, BirthDate = new DateOnly(2000, 1, 2) }
            }));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayersByAge(23);

            var result = actionResult.Result as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
            var playersResult = Assert.IsType<List<Player>>(result.Value);
            Assert.Equal(2, playersResult.Count);
        }

        [Fact]
        public async void Get_Players_By_Coach_With_No_Teams()
        {
            var coach = "Super Man";
            A.CallTo(() => _teamRepository.SearchTeamsByCoach(coach)).Returns(Task.FromResult<List<Team>>(null));
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayersByCoach(coach);

            var result = actionResult.Result as NotFoundObjectResult;
            Assert.Equal(404, result.StatusCode);
            Assert.Equal($"The coach {coach} does not exist in any teams", result.Value);
        }

        [Fact]
        public async void Get_Players_By_Coach_With_Teams_No_Players()
        {
            var coach = "Iron Man";
            var teams = new List<Team>
            {
                new Team{ Name = "Team 1", Coach = "Iron Man" },
                new Team{ Name = "Team 2", Coach = "Iron Man" }
            };

            var players = new List<Player>();

            A.CallTo(() => _teamRepository.SearchTeamsByCoach(coach)).Returns(Task.FromResult(teams));
            A.CallTo(() => _playerRepository.GetPlayersByTeam(A<string>._)).ReturnsLazily((string teamName) => players.Where(p => p.TeamName == teamName).ToList());
            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayersByCoach(coach);

            var result = actionResult.Result as NotFoundObjectResult;
            Assert.Equal(404, result.StatusCode);
            Assert.Equal($"There is no player in {coach}'s teams", result.Value);
        }

        [Fact]
        public async void Get_Players_By_Coach_ReturnOk()
        {
            var coach = "Bat Man";

            var teams = new List<Team>
            {
                new Team{ Name = "Team 1", Coach = "Bat Man" },
                new Team{ Name = "Team 2", Coach = "Bat Man" }
            };

            var players = new List<Player>
            {
                new Player{ PlayerId = 1, Name = "Player 1", TeamName = "Team 1" },
                new Player{ PlayerId = 2, Name = "Player 2", TeamName = "Team 1" },
                new Player{ PlayerId = 3, Name = "Player 3", TeamName = "Team 2" }
            };
            
            A.CallTo(() => _teamRepository.SearchTeamsByCoach(coach)).Returns(Task.FromResult(teams));
            A.CallTo(() => _playerRepository.GetPlayersByTeam(A<string>._)).ReturnsLazily((string teamName) => players.Where(p => p.TeamName == teamName).ToList());

            var controller = new PlayerController(_playerRepository, _teamRepository);

            var actionResult = await controller.GetPlayersByCoach(coach);

            var result = actionResult.Result as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
            var playersResult = Assert.IsType<List<Player>>(result.Value);
            Assert.Equal(3, playersResult.Count);
        }


    }
}