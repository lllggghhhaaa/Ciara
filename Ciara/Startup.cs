using System.Reflection;
using Ciara.Commands;
using Ciara.Events;
using Ciara.Services;
using Ciara.Shared.Database;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Extensions;
using WaxMenu;
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
            .ConfigureEventHandlers(handlingBuilder =>
            {
                handlingBuilder.AddEventHandlers<GuildMemberAdded>();
            })
            .AddShardedDiscordClient(
                builder.Configuration["Discord:Token"] ??
                throw new ArgumentNullException("token", "Discord token is null"), DiscordIntents.AllUnprivileged)
            .AddCommandsExtension(extension =>
            {
                extension.AddCommands(Assembly.GetEntryAssembly()!.GetTypes());
            }, new CommandsConfiguration
            {
                DebugGuildId = ulong.Parse(builder.Configuration["Discord:DebugGuildId"] ?? "0"),
                UseDefaultCommandErrorHandler = true
            })
            .AddMenuExtension(extension =>
            {
                extension.AddMenu<GuildCommand>();
            }, new MenuExtensionConfiguration("menu", "_"));

        var app = builder.Build();

        await app.RunAsync();
    }
}