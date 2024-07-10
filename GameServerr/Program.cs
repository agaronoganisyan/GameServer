using GameServerr;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Program
{
    static async Task Main(string[] args)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder(args)
            .UseOrleans(silo =>
            {
                silo.UseLocalhostClustering()
                    .AddMemoryGrainStorage(ProjectConstants.StorageProvider)
                    .ConfigureLogging(logging => logging.AddConsole());
            })
            .UseConsoleLifetime();
        
        using IHost host = builder.Build();

        await host.RunAsync();
    }
}