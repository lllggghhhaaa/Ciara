using Ciara.Shared.Database;
using DSharpPlus;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ciara.Utils;

public static class TermsUtils
{
    public static async Task TermsPrompt(SlashCommandContext context, BotMember? member)
    {
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
            new DiscordButtonComponent(DiscordButtonStyle.Success, $"terms:{context.User.Id}", "Accept",
                true);

        await context.RespondAsync(messageBuilder.AddComponents(new DiscordButtonComponent(button)));

        await Task.Delay(TimeSpan.FromSeconds(3));

        button.Enable();
        messageBuilder.ClearComponents();
        messageBuilder.AddComponents(button);

        await context.EditResponseAsync(messageBuilder);

        var botMessage = await context.GetResponseAsync();

        var interactivity = context.Client.GetInteractivity();

        var result = await interactivity.WaitForButtonAsync(botMessage!, [button]);

        if (result.TimedOut) return;
        await result.Result.Interaction.CreateResponseAsync(DiscordInteractionResponseType
            .DeferredMessageUpdate);

        var database = context.Client.ServiceProvider.GetService<CiaraContext>();

        if (member is null)
            await database!.AddAsync(new BotMember { MemberId = context.User.Id, TermsAccepted = true });
        else
            await database!.Members.ExecuteUpdateAsync(calls => 
                calls.SetProperty(m => m.TermsAccepted, m => true));

        await database.SaveChangesAsync();

        button =
            new DiscordButtonComponent(DiscordButtonStyle.Success, $"terms:{context.User.Id}",
                "Accepted", true);
        messageBuilder.ClearComponents();
        messageBuilder.AddComponents(button);

        await context.EditResponseAsync(messageBuilder);
    }
}