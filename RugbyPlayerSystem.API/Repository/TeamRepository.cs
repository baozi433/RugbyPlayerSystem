using Microsoft.EntityFrameworkCore;
using RugbyPlayerSystem.API.Data;
using RugbyPlayerSystem.API.Repository.Contract;
using RugbyPlayerSystem.Models;

namespace RugbyPlayerSystem.API.Repository
{
    public class TeamRepository : ITeamRepository
    {
        private readonly RugbyDbContext _rugbyDbContext;
        public TeamRepository(RugbyDbContext rugbyDbContext)
        {
            _rugbyDbContext = rugbyDbContext;
        }

        public async Task<Team> AddTeam(Team team)
        {
            _rugbyDbContext.Teams.Add(team);
            await _rugbyDbContext.SaveChangesAsync();
            return team;
        }

        public async Task<Team> GetTeam(string teamName)
        {
            var team = await _rugbyDbContext.Teams.SingleOrDefaultAsync(t => t.Name.Contains(teamName));
            return team;
        }

        public async Task<List<Team>> GetTeams()
        {
            var teams = await _rugbyDbContext.Teams.ToListAsync();
            return teams;
        }

        public async Task<List<Team>> SearchTeamsByCoach(string coach)
        {
            var teams = await _rugbyDbContext.Teams.Where(t => t.Coach == coach).ToListAsync();
            return teams;
        }
    }
}
