using Ciara.Services;
using Ciara.Shared.Database;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using Microsoft.Extensions.Logging;

namespace Ciara.ContextCheck;

public class TermsRequiredCheck(MessageHistoryContext database) : IContextCheck<TermsRequiredAttribute>
{
    public async ValueTask<string?> ExecuteCheckAsync(TermsRequiredAttribute attribute, CommandContext context)
    {
        var member = await database.Members.FindAsync(context.User.Id);

        return member is null || !member.TermsAccepted ? "Terms required" : null;
    }
}