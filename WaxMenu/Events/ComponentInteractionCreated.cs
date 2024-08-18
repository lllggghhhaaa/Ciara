using DSharpPlus;
using DSharpPlus.EventArgs;

namespace WaxMenu.Events;

public class ComponentInteractionCreated(MenuExtension menu): IEventHandler<ComponentInteractionCreatedEventArgs>
{
    public async Task HandleEventAsync(DiscordClient sender, ComponentInteractionCreatedEventArgs eventArgs)
    {
        menu.HandleInteraction(sender, eventArgs);
    }
}