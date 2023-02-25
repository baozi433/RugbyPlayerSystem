using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using RugbyPlayerSystem.API.Controllers;
using RugbyPlayerSystem.API.Repository;
using RugbyPlayerSystem.API.Repository.Contract;
using RugbyPlayerSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RugbyPlayerSystem.Test
{
    public class TeamControllerTest
    {
        private readonly ITeamRepository _teamRepository;
        public TeamControllerTest()
        {
            _teamRepository = A.Fake<ITeamRepository>();
        }

        [Fact]
        public async void Add_Null_Team()
        {
            A.CallTo(() => _teamRepository.AddTeam(null));
            var controller = new TeamController(_teamRepository);

            var actionResult = await controller.AddTeam(null);

            var result = actionResult.Result as BadRequestObjectResult;
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("The team is null", result.Value);
        }

        [Fact]
        public async void Add_Team_Which_Exists()
        {
            var team = A.Fake<Team>();
            A.CallTo(() => _teamRepository.GetTeam(team.Name)).Returns(Task.FromResult(team));
            var controller = new TeamController(_teamRepository);

            var actionResult = await controller.AddTeam(team);

            var result = actionResult.Result as ConflictObjectResult;
            Assert.Equal(409, result.StatusCode);
            Assert.Equal($"The team {team.Name} already exists", result.Value);
        }

        [Fact]
        public async void Get_All_Teams_With_Null()
        {
            A.CallTo(() => _teamRepository.GetTeams()).Returns(Task.FromResult<List<Team>>(null));
            var controller = new TeamController(_teamRepository);

            var actionResult = await controller.GetTeams();

            var result = actionResult.Result as NotFoundObjectResult;
            Assert.Equal(404, result.StatusCode);
            Assert.Equal($"No teams", result.Value);
        }

        [Fact]
        public async void Get_All_Teams_Return_Ok()
        {
            var teams = A.Fake<List<Team>>();

            A.CallTo(() => _teamRepository.GetTeams()).Returns(Task.FromResult(teams));
            var controller = new TeamController(_teamRepository);

            var actionResult = await controller.GetTeams();

            var result = actionResult.Result as OkObjectResult;
            Assert.Equal(200, result.StatusCode);
        }

    }
}
