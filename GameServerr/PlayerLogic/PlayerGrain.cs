namespace GameServerr.PlayerLogic
{
    public class PlayerGrain : Grain, IPlayerGrain
    {
        private int _guess;
        
        private readonly IPersistentState<PlayerState> _state;
        
        public PlayerGrain([PersistentState("playerState", ProjectConstants.StorageProvider)] IPersistentState<PlayerState> state)
        {
            _state = state;
        }
        
        public Task SetGuess(int guess)
        {
            _guess = guess;
            return Task.CompletedTask;
        }
        
        public Task<int> GetGuess()
        {
            return Task.FromResult(_guess);
        }

        public async Task SetPoints(int points)
        {
            if (points < 0) throw new ArgumentOutOfRangeException();
            
            _state.State.Points = points;
            await _state.WriteStateAsync();
        }

        public Task<int> GetPoints()
        {
            return Task.FromResult(_state.State.Points);
        }
    }   
}