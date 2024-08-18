using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;

namespace Ciara.Commands;

[Command("ban")]
public class BanCommand
{
    [Command("member"), RequireGuild, RequirePermissions(DiscordPermissions.BanMembers)]
    public async ValueTask BanMember(CommandContext context, DiscordMember member,
        [MinMaxValue(0, 7)] int deleteMessagesDays = 0, string reason = "No reason provided")
    {
        await member.BanAsync(TimeSpan.FromDays(deleteMessagesDays), reason);
    }

    [Command("id"), RequireGuild, RequirePermissions(DiscordPermissions.BanMembers)]
    public async ValueTask BanId(CommandContext context, ulong id,
        [MinMaxValue(0, 7)] int deleteMessagesDays = 0, string reason = "No reason provided")
    {
        await context.Guild!.BanMemberAsync(id, TimeSpan.FromDays(deleteMessagesDays), reason);
    }
}