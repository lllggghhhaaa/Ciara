using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Entities;

namespace Ciara.Commands;

public class MetricsCommand
{
    [Command("rest"), RequireGuild]
    public async ValueTask Rest(CommandContext context)
    {
        var metrics = context.Client.GetRequestMetrics();

        await context.RespondAsync(new DiscordEmbedBuilder
        {
            Title = "Rest metrics"
        }
            .AddField("Total Requests", metrics.TotalRequests.ToString(), true)
            .AddField("Successful Requests", metrics.SuccessfulRequests.ToString(), true)
            .AddField("Failed Requests", metrics.FailedRequests.ToString(), true)
            .AddField("Rate limit Hit", metrics.RatelimitsHit.ToString(), true)
            .AddField("Global Rate limit Hit", metrics.GlobalRatelimitsHit.ToString(), true)
            .AddField("Bucket Rate limit Hit", metrics.BucketRatelimitsHit.ToString(), true)
            .AddField("Bad Requests", metrics.BadRequests.ToString(), true)
            .AddField("Forbidden", metrics.Forbidden.ToString(), true)
            .AddField("Not Found", metrics.NotFound.ToString(), true)
            .AddField("Too Large", metrics.TooLarge.ToString(), true)
            .AddField("Server Errors", metrics.ServerErrors.ToString(), true)
            .AddField("Duration", FormatTimeSpan(metrics.Duration), true));
    }
    
    static string FormatTimeSpan(TimeSpan timeSpan)
    {
        string formatted = $"{(timeSpan.Days > 0 ? $"{timeSpan.Days} day{(timeSpan.Days > 1 ? "s" : "")}, " : "")}" +
                           $"{(timeSpan.Hours > 0 ? $"{timeSpan.Hours} hour{(timeSpan.Hours > 1 ? "s" : "")}, " : "")}" +
                           $"{(timeSpan.Minutes > 0 ? $"{timeSpan.Minutes} minute{(timeSpan.Minutes > 1 ? "s" : "")}, " : "")}" +
                           $"{(timeSpan.Seconds > 0 ? $"{timeSpan.Seconds} second{(timeSpan.Seconds > 1 ? "s" : "")}" : "")}";

        if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

        return formatted;
    }
}