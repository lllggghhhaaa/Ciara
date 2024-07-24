using System.Reflection;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ciara.Services;

public class BotService : IHostedService
{
    private readonly ILogger<BotService> _logger;
    private readonly DiscordClient _client;

    public BotService(IConfiguration config, ILogger<BotService> logger, DiscordClient client)
    {
        _logger = logger;
        _client = client;

        _client.UseInteractivity(new InteractivityConfiguration
        {
            Timeout = TimeSpan.FromMinutes(5)
        });
        
        var commandsExtension = _client.UseCommands(new CommandsConfiguration
        {
            DebugGuildId = ulong.Parse(config["Discord:DebugGuildId"] ?? "0"),
            UseDefaultCommandErrorHandler = false
        });

        commandsExtension.AddCommands(Assembly.GetEntryAssembly()!.GetTypes());

        commandsExtension.CommandErrored += async (_, args) =>
        {
            _logger.LogError(args.Exception.ToString());
        };
    }

    public async Task StartAsync(CancellationToken token)
    {
        await _client.ConnectAsync();

        _logger.LogInformation("Logged in to Discord");
    }

    public async Task StopAsync(CancellationToken token)
    {
        await _client.DisconnectAsync();
    }
}