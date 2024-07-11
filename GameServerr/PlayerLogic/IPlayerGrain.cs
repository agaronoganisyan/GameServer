namespace GameServerr.PlayerLogic
{
    public interface IPlayerGrain : IGrainWithStringKey
    {
        Task SetGuess(int guess);
        Task<int> GetGuess();
        Task SetPoints(int score);
        Task<int> GetPoints();
    }   
}