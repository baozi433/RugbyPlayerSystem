using RugbyPlayerSystem.Models;

namespace RugbyPlayerSystem.API.Repository.Contract
{
    public interface ITeamRepository
    {
        Task<Team> AddTeam(Team team);
        Task<List<Team>> GetTeams();
        Task<Team> GetTeam(string teamName);
        Task<List<Team>> SearchTeamsByCoach(string coach);
    }
}
