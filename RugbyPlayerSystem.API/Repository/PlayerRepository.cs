using Microsoft.EntityFrameworkCore;
using RugbyPlayerSystem.API.Data;
using RugbyPlayerSystem.API.Repository.Contract;
using RugbyPlayerSystem.Models;
using System.Numerics;

namespace RugbyPlayerSystem.API.Repository
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly RugbyDbContext _rugbyDbContext;

        public PlayerRepository(RugbyDbContext rugbyDbContext)
        {
            _rugbyDbContext = rugbyDbContext;
        }

        public async Task<Player> AddPlayer(Player player)
        {
            _rugbyDbContext.Players.Add(player);
            await _rugbyDbContext.SaveChangesAsync();
            return player;
        }

        public async Task<Player> GetPlayer(int id)
        {
            var player = await _rugbyDbContext.Players.SingleOrDefaultAsync(p => p.PlayerId == id);
            return player;
        }

        public async Task<List<Player>> GetPlayers()
        {
            var players = await _rugbyDbContext.Players.ToListAsync();
            return players;
        }

        public async Task<List<Player>> GetPlayersByTeam(string teamName)
        {
            var players = await _rugbyDbContext.Players
                          .Where(p => p.TeamName.Contains(teamName)).ToListAsync();
            return players;
        }

        public async Task<List<Player>> SearchPlayersByAge(int age)
        {
            var allPlayers = await _rugbyDbContext.Players.ToListAsync();
            var players = new List<Player>();

            var dateNow = DateOnly.FromDateTime(DateTime.Now);

            foreach(Player p in allPlayers)
            {
                if ((dateNow.Year - p.BirthDate.Year == age) && (dateNow.Month >= p.BirthDate.Month) && (dateNow.Day >= p.BirthDate.Day))
                {
                    players.Add(p);
                }
                else if ((dateNow.Year - p.BirthDate.Year == age + 1) && (dateNow.Month <= p.BirthDate.Month) && (dateNow.Day < p.BirthDate.Day))
                {
                    players.Add(p);
                }
            }
            return players;
        }

        public async Task<Player> SignToATeam(int playerId, string teamName)
        {
            var team = await _rugbyDbContext.Teams.SingleOrDefaultAsync(t => t.Name == teamName);
            if (team == null) return null;
            var findPlayer = await _rugbyDbContext.Players.SingleOrDefaultAsync(p => p.PlayerId == playerId);
            if (findPlayer == null) return null;

            findPlayer.TeamName = team.Name;

            await _rugbyDbContext.SaveChangesAsync();
            return findPlayer;
        }
    }
}
