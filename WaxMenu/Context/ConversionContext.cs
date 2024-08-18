using DSharpPlus;
using DSharpPlus.EventArgs;

namespace WaxMenu.Context;

public record ConversionContext(DiscordClient Client, ComponentInteractionCreatedEventArgs EventArgs, string Content);