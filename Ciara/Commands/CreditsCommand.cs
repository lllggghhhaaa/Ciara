using Ciara.Shared.Database;
using Ciara.Utils;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;

namespace Ciara.Commands;

public class CreditsCommand(CiaraContext database)
{
    [Command("credit")]
    public async ValueTask Credit(CommandContext context)
    {
        var member = await database.Members.FindAsync(context.User.Id);
        
        if (member is null || !member.TermsAccepted)
        {
            await TermsUtils.TermsPrompt((context as SlashCommandContext)!, member);
            return;
        }

        await context.RespondAsync($"{context.User.Mention} credits: {member.Credits}");
    }
}