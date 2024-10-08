﻿using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;

namespace Ciara.Commands;

public class PingCommand
{
    [Command("ping"), RequireGuild]
    public ValueTask ExecuteAsync(CommandContext context) => context.RespondAsync($"Pong! Latency is {context.Client.GetConnectionLatency(context.Guild!.Id).Milliseconds}ms.");
}