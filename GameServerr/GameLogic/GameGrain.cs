using GameServerr.PlayerLogic;
using GameServerr.RoomLogic;

namespace GameServerr.GameLogic
{
    public class GameGrain : Grain, IGameGrain
    {
        private IRoomGrain _roomGrain;
        
        private readonly List<IPlayerGrain> _playersInQueue = new();
        private TaskCompletionSource<bool> _gameStartedTcs = new();
        
        public Task AddToQueue(IPlayerGrain player)
        {
            _playersInQueue.Add(player);

            if (_playersInQueue.Count >= 2)
            {
                _gameStartedTcs.SetResult(true);
            }

            return Task.CompletedTask;
        }

        public Task WaitingForGameStart()
        {
            return _gameStartedTcs.Task;
        }

        public async Task StartGame()
        {
            _roomGrain = GrainFactory.GetGrain<IRoomGrain>(0);
            
            for (int i=0;i<_playersInQueue.Count;i++)
            {
                await _roomGrain.AddPlayer(_playersInQueue[i]);
            }

            _playersInQueue.Clear();
        }

        public async Task<string> GetWinnerID()
        {
            return await _roomGrain.ProcessGame();
        }
    }      
}