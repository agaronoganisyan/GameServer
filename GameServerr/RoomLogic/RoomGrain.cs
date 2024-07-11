using GameServerr.PlayerLogic;

namespace GameServerr.RoomLogic
{
    public class RoomGrain : Grain, IRoomGrain
    {
        private readonly List<IPlayerGrain> _players = new List<IPlayerGrain>();
        private int _targetNumber;
        
        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _targetNumber = new Random().Next(0, 101);
            return base.OnActivateAsync(cancellationToken);
        }

        public Task AddPlayer(IPlayerGrain player)
        {
            _players.Add(player); 
            return Task.CompletedTask;
        }

        public async Task<string> ProcessGame()
        {
            IPlayerGrain winner = null;

            if (_players.Count == 0)
            {
                throw new InvalidOperationException("No players in the game.");
            }
        
            for (int i = 0; i < _players.Count; i++)
            {
                if (i == 0)
                {
                    winner = _players[i];
                }
                else
                {
                    int currentWinnerGuess = await winner.GetGuess();
                    int currentPlayerGuess = await _players[i].GetGuess();
                
                    winner = Math.Abs(_targetNumber - currentWinnerGuess) < Math.Abs(_targetNumber - currentPlayerGuess) ? winner : _players[i];
                }
            }
        
            int winnerScore = await winner.GetPoints();
            await winner.SetPoints(winnerScore + 1);

            return winner.GetPrimaryKeyString();
        }
    }   
}