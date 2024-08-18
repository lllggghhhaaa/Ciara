using Ciara.Shared.Database;
using DSharpPlus.Entities;

namespace Ciara.MenuBuilders;

public static class GuildMenuBuilder
{
    public static DiscordMessageBuilder MainMenu(string guildName, BotGuild guild)
    {
        var messageBuilder = new DiscordMessageBuilder();
        messageBuilder.AddEmbed(new DiscordEmbedBuilder
        {
            Title = $"{guildName}'s Menu",
            Description =
                "Lorem ipsum is placeholder text commonly used in the graphic, print, and publishing industries for previewing layouts and visual mockups.",
            Color = DiscordColor.Lilac
        }.AddField("New Member View Channel",
            guild.MemberJoinViewChannelId is null ? "null" : $"<#{guild.MemberJoinViewChannelId}> `<#{guild.MemberJoinViewChannelId}>`"));
        messageBuilder.AddComponents(new DiscordButtonComponent(DiscordButtonStyle.Secondary,
            "menu_guild_newmember", "New Member"));

        return messageBuilder;
    }

    public static DiscordMessageBuilder NewMemberMenu(IEnumerable<DiscordChannel> channels, string guildName,
        ulong currentChannel,
        BotGuild guild)
    {
        var messageBuilder = new DiscordMessageBuilder();
        messageBuilder.AddEmbed(new DiscordEmbedBuilder
        {
            Title = $"{guildName}'s New Member Menu",
            Description =
                "Lorem ipsum is placeholder text commonly used in the graphic, print, and publishing industries for previewing layouts and visual mockups.",
            Color = DiscordColor.Lilac
        }.AddField("New Member View Channel",
            guild.MemberJoinViewChannelId is null ? "null" : $"<#{guild.MemberJoinViewChannelId}> `<#{guild.MemberJoinViewChannelId}>`"));

        var options = new List<DiscordSelectComponentOption>
            { new("Current Channel", currentChannel.ToString(), null!, currentChannel == guild.MemberJoinViewChannelId) };

        options.AddRange(channels.Where(channel =>
                channel.Id != currentChannel &&
                channel.Type == DiscordChannelType.Text)
            .Select(channel =>
                new DiscordSelectComponentOption(channel.Name, channel.Id.ToString(), null!, channel.Id == guild.MemberJoinViewChannelId)
            ));

        messageBuilder.AddComponents(new DiscordButtonComponent(DiscordButtonStyle.Secondary, "menu_guild_home",
            "Home"));
        messageBuilder.AddComponents(new DiscordSelectComponent("menu_guild_newmemberselect", "Select a Channel",
            options));


        return messageBuilder;
    }
}