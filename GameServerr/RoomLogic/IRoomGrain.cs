using GameServerr.PlayerLogic;

namespace GameServerr.RoomLogic
{
    public interface IRoomGrain
    {
        Task AddPlayer(IPlayerGrain player);
        Task<string> ProcessGame();
    }   
}