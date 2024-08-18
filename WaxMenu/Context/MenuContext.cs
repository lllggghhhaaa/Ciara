using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace WaxMenu.Context;

public class MenuContext(DiscordClient client, ComponentInteractionCreatedEventArgs args)
{
    public DiscordClient Client = client;
    public DiscordInteraction Interaction = args.Interaction;
    public string InteractionId = args.Id;
    public DiscordUser User = args.User;
    public DiscordGuild Guild = args.Guild;
    public DiscordChannel Channel = args.Channel;
    public string[] SelectValues = args.Values;
    public DiscordMessage Message = args.Message;
    public string Locale = args.Locale;
    public string GuildLocale = args.GuildLocale;

    public async Task EditResponse(DiscordMessageBuilder messageBuilder) =>
        await Interaction.CreateResponseAsync(DiscordInteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder(messageBuilder));
}