using RugbyPlayerSystem.Models;

namespace RugbyPlayerSystem.API.Repository.Contract
{
    public interface IPlayerRepository
    {
        Task<Player> AddPlayer(Player player);
        Task<Player> SignToATeam(int playerId, string teamName);
        Task<Player> GetPlayer(int id);
        Task<List<Player>> GetPlayers();
        Task<List<Player>> GetPlayersByTeam(string teamName);
        Task<List<Player>> SearchPlayersByAge(int age);
    }
}
