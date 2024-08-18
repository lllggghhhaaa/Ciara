using Ciara.MenuBuilders;
using Ciara.Services;
using Ciara.Shared.Database;
using Ciara.Shared.Extensions;
using Ciara.Shared.Utils;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using WaxMenu;
using WaxMenu.Attributes;
using WaxMenu.Context;

namespace Ciara.Commands;

[Command("guild"), Menu("guild")]
public class GuildCommand(CiaraContext database, ILogger<BotService> logger)
{
    [Command("menu"), RequireGuild]
    public async ValueTask Menu(CommandContext context)
    {
        var idHash = Hash.HashId(context.Guild!.Id);
        var dbGuild = await database.Guilds.FindOrCreateAsync(idHash, () => new BotGuild { IdHash = idHash }, database);
        
        await context.RespondAsync(GuildMenuBuilder.MainMenu(context.Guild.Name, dbGuild));
    }

    [MenuAction("home")]
    public async ValueTask Menu(MenuContext context)
    {
        var idHash = Hash.HashId(context.Guild.Id);
        var dbGuild = await database.Guilds.FindOrCreateAsync(idHash, () => new BotGuild { IdHash = idHash }, database);
        
        await context.EditResponse(GuildMenuBuilder.MainMenu(context.Guild.Name, dbGuild));
    }

    [MenuAction("newmember")]
    public async ValueTask NewMember(MenuContext context)
    {
        var idHash = Hash.HashId(context.Guild.Id);
        var dbGuild = await database.Guilds.FindOrCreateAsync(idHash, () => new BotGuild { IdHash = idHash }, database);

        await context.EditResponse(GuildMenuBuilder.NewMemberMenu(context.Guild.Channels.Values,
            context.Guild.Name, context.Channel.Id, dbGuild));
    }

    [MenuAction("newmemberselect")]
    public async ValueTask NewMemberSelect(MenuContext context)
    {
        var idHash = Hash.HashId(context.Guild.Id);
        var dbGuild = await database.Guilds.FindOrCreateAsync(idHash, () => new BotGuild { IdHash = idHash }, database);

        dbGuild.MemberJoinViewChannelId = ulong.Parse(context.SelectValues[0]);
        database.Guilds.Update(dbGuild);
        await database.SaveChangesAsync();

        await context.EditResponse(GuildMenuBuilder.NewMemberMenu(context.Guild.Channels.Values,
            context.Guild.Name, context.Channel.Id, dbGuild));
    }
}