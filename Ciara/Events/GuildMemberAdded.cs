using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Ciara.Events;

public class GuildMemberAdded : IEventHandler<GuildMemberAddedEventArgs>
{
    public Task HandleEventAsync(DiscordClient sender, GuildMemberAddedEventArgs eventArgs)
    {
        throw new NotImplementedException();
    }
}