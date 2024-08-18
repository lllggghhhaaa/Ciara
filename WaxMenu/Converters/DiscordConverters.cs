using DSharpPlus.Entities;
using WaxMenu.Context;

namespace WaxMenu.Converters;

public static class DiscordConverters
{
    public static async Task<object> ChannelConverter(ConversionContext ctx) =>
        await ctx.Client.GetChannelAsync(ulong.Parse(ctx.Content));

    public static async Task<object> UserConverter(ConversionContext ctx) =>
        await ctx.Client.GetUserAsync(ulong.Parse(ctx.Content));

    public static async Task<object> GuildConverter(ConversionContext ctx) =>
        await ctx.Client.GetGuildAsync(ulong.Parse(ctx.Content));
}