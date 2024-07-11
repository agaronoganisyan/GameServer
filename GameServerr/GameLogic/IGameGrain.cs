using GameServerr.PlayerLogic;

namespace GameServerr.GameLogic
{
    public interface IGameGrain : IGrainWithIntegerKey
    {
        Task AddToQueue(IPlayerGrain player);
        Task WaitingForGameStart();
        Task StartGame();
        Task<string> GetWinnerID();
    }   
}