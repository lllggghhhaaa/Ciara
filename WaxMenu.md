## Introduction
WaxMenu is a library to create interactive menus and component tasks with [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus)

## How to use

### Setup
````csharp
// With DiscordClientBuilder
builder.UseWaxMenu(extension =>
    {
        // Add a menu to be used
        extension.AddMenu<GuildCommand>();
    }, new MenuExtensionConfiguration("menu", "_"));
````

````csharp
// With ServiceProvider
builder.Services
    // ...
    .AddMenuExtension(extension =>
    {
        // Add a menu to be used
        extension.AddMenu<GuildCommand>();
    }, new MenuExtensionConfiguration("menu", "_"));
````

### Adding a menu
You need to add a menu attribute to the class that you want to make the menu. You can use with Command extensions too
<br/>Inside the class, you can add Menu Actions. This actions are triggered in every component interaction where match the requisites (See [ID Pattern](#id-pattern))

````csharp
// Example
[Command("guild"), Menu("guild")]
public class GuildCommand(CiaraContext database)
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
````

### ID Pattern
The component CustomId should match this pattern
``prefix_command_subcommand_param1_param2``
- `prefix`* The prefix that you added on configuration
- `_`* The separator that you added on configuration
- `command`* Is the name of the menu (`[Menu("guild")]`)
- `subcommand`* Is the name of the action (`[MenuAction("home")]`)
- `params`*(optional)* Are parameters that is passed through the method (See [Action Parameters](#action-parameters))

````csharp
// Example
messageBuilder.AddComponents(new DiscordButtonComponent(DiscordButtonStyle.Secondary, "menu_guild_home", "Home"));
````

### Action Parameters

| Type           | Format          | Note |
|----------------|-----------------|------|
| bool           | `true`/`false`  |      |
| byte           | `0` - `255`     |      |
| sbyte          | `-128` - `127`  |      |
| char           | `'c'`           |      |
| decimal        | `547589.43642`  |      | 
| double         | `325436.4535`   |      |
| float          | `3524.56`       |      | 
| int            | `-352456`       |      |
| uint           | `246356`        |      |
| long           | `-32534456254`  |      |
| ulong          | `352434436346`  |      |
| short          | `-2356`         |      |
| ushort         | `4367`          |      |
| string         | `"text"`        |      |
| DiscordChannel | `3253423463456` | ID   |
| DiscordUser    | `4364536534623` | ID   |
| DiscordGuild   | `5473536453632` | ID   |