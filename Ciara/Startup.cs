using Ciara.Services;
using Ciara.Shared.Database;
using DSharpPlus;
using DSharpPlus.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ciara;

public class Startup
{
    public async Task Start()
    {
        var builder = Host.CreateApplicationBuilder();
        
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<BotService>();

        builder.Services
            .AddHostedService<BotService>()
            .AddDbContext<CiaraContext>()
            .AddDiscordClient(
                builder.Configuration["Discord:Token"] ??
                throw new ArgumentNullException("token", "Discord token is null"), DiscordIntents.AllUnprivileged);

        var app = builder.Build();

        await app.RunAsync();
    }
}