using DSharpPlus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ciara.Services;

public class BotService(IConfiguration config, ILogger<BotService> logger, DiscordClient client) : IHostedService
{
    public async Task StartAsync(CancellationToken token)
    {
        await client.ConnectAsync();

        logger.LogInformation("Logged in to Discord");
    }

    public async Task StopAsync(CancellationToken token)
    {
        await client.DisconnectAsync();
    }
}