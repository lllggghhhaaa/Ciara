using System.Reflection;
using Ciara.ContextCheck;
using Ciara.Shared.Database;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Exceptions;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ciara.Services;

public class BotService : IHostedService
{
    private readonly ILogger<BotService> _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly DiscordClient _client;

    public BotService(IConfiguration config, ILogger<BotService> logger, IHostApplicationLifetime applicationLifetime, DiscordClient client)
    {
        _logger = logger;
        _applicationLifetime = applicationLifetime;
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

        commandsExtension.AddCheck<TermsRequiredCheck>();

        commandsExtension.AddCommands(Assembly.GetEntryAssembly()!.GetTypes());

        commandsExtension.CommandErrored += async (_, args) =>
        {
            var context = args.Context as SlashCommandContext;
            
            if (context == null) return;
            
            switch (args.Exception)
            {
                case ChecksFailedException checksFailedException:
                    await context.DeferResponseAsync(true);
                    
                    var messageBuilder = new DiscordMessageBuilder { Content = String.Empty }
                        .AddEmbed(new DiscordEmbedBuilder
                        {
                            Title = "Terms",
                            Description = """
                                          You must agree to the terms before using this
                                          - [Terms](https://domain.com/Terms)
                                          - [Privacy Policy](https://domain.com/Privacy)
                                          """,
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                IconUrl = context.Member?.GetGuildAvatarUrl(ImageFormat.Auto),
                                Text = context.Member?.GlobalName
                            }
                        });

                    var button =
                        new DiscordButtonComponent(DiscordButtonStyle.Success, $"terms:{context.User.Id}", "Accept", true);
                    
                    await context.RespondAsync(messageBuilder.AddComponents(new DiscordButtonComponent(button)));

                    await Task.Delay(TimeSpan.FromSeconds(3));
                    
                    button.Enable();
                    messageBuilder.ClearComponents();
                    messageBuilder.AddComponents(button);

                    await context.EditResponseAsync(messageBuilder);

                    var botMessage = await context.GetResponseAsync();
                    
                    var interactivity = _client.GetInteractivity();
                    
                    var result = await interactivity.WaitForButtonAsync(botMessage!, [button]);
                    
                    if (result.TimedOut) return;
                    await result.Result.Interaction.CreateResponseAsync(DiscordInteractionResponseType.DeferredMessageUpdate);
                    
                    var database = _client.ServiceProvider.GetService<CiaraContext>();
                    await database!.Members.ExecuteUpdateAsync(calls =>
                        calls.SetProperty(member => member.TermsAccepted, member => true));
                    
                    await database.SaveChangesAsync();

                    button =
                        new DiscordButtonComponent(DiscordButtonStyle.Success, $"terms:{context.User.Id}", "Accepted", true);
                    messageBuilder.ClearComponents();
                    messageBuilder.AddComponents(button);

                    await context.EditResponseAsync(messageBuilder);
                    break;
                
                default:
                    _logger.LogError(args.Exception.ToString());
                    break;
            }
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