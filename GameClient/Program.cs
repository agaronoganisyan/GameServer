using GameServerr.GameLogic;
using GameServerr.PlayerLogic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Program
{
    static async Task Main(string[] args)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder(args)
            .UseOrleansClient(client =>
            {
                client.UseLocalhostClustering();
            })
            .ConfigureLogging(logging => logging.AddConsole())
            .UseConsoleLifetime();

        using IHost host = builder.Build();
        await host.StartAsync();

        var client = host.Services.GetRequiredService<IClusterClient>();

        IGameGrain gameGrain = client.GetGrain<IGameGrain>(0);
        
        string player1Id = "Player1";
        IPlayerGrain player1Grain = client.GetGrain<IPlayerGrain>(player1Id);
        
        await gameGrain.AddToQueue(player1Grain);
        
        Console.WriteLine($"{player1Id} joined the game.");

        string player2Id = "Player2";
        IPlayerGrain player2Grain = client.GetGrain<IPlayerGrain>(player2Id);
        
        await gameGrain.AddToQueue(player2Grain);
        
        Console.WriteLine($"{player2Id} joined the game.");

        await gameGrain.WaitingForGameStart();
        
        await gameGrain.StartGame();
        
        Console.WriteLine("Game started!");
        
        int player1Guess = GetValidGuess(player1Id);
        await player1Grain.SetGuess(player1Guess);

        int player2Guess = GetValidGuess(player2Id);
        await player2Grain.SetGuess(player2Guess);

        string winnerId = await gameGrain.GetWinnerID();
        Console.WriteLine($"Winner is: {winnerId}");

        var player1Score = await player1Grain.GetPoints();
        var player2Score = await player2Grain.GetPoints();

        Console.WriteLine($"Player 1 Score: {player1Score}");
        Console.WriteLine($"Player 2 Score: {player2Score}");

        await host.StopAsync();
    }
    
    
    private static int GetValidGuess(string playerName)
    {
        int guess;
        while (true)
        {
            Console.WriteLine($"{playerName}, guess a number between 0 and 100:");
            var input = Console.ReadLine();
            if (int.TryParse(input, out guess) && guess >= 0 && guess <= 100)
            {
                break;
            }
            Console.WriteLine("Invalid input. Please enter a number between 0 and 100.");
        }
        return guess;
    }
}