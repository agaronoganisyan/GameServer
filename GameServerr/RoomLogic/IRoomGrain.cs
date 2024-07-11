using GameServerr.PlayerLogic;

namespace GameServerr.RoomLogic
{
    public interface IRoomGrain : IGrainWithIntegerKey
    {
        Task AddPlayer(IPlayerGrain player);
        Task<string> ProcessGame();
    }   
}