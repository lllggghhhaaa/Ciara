using System.ComponentModel;
using System.Text;
using Ciara.ContextCheck;
using Ciara.Entities;
using Ciara.Shared.Database;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Ciara.Commands;

[Command("ai"), TermsRequired]
public class SayCommand(IConfiguration config, CiaraContext database)
{
    [Command("say"), Description("Say my name")]
    public async ValueTask SayAsync(CommandContext context, string prompt, DiscordAttachment? attachment = null)
    {
        await context.DeferResponseAsync();

        var member = await database.Members
            .Include(m => m.Messages)
            .FirstOrDefaultAsync(m => m.MemberId == context.User.Id);

        if (member is null)
        {
            member = new BotMember
            {
                MemberId = context.User.Id,
                Messages = new List<AiMessage>()
            };
            await database.Members.AddAsync(member);
            await database.SaveChangesAsync();
        }

        var messageHistory = member.Messages.ToList();

        using var client = new HttpClient();
        client.BaseAddress = new Uri(config["Ollama:Uri"] ?? throw new ArgumentException("Ollama:Uri is null"));

        string? imageData = attachment is null ? null : Convert.ToBase64String(await client.GetByteArrayAsync(attachment.Url));
        
        var chatMessages = messageHistory.Select(message =>
                new OllamaChatMessage(message.FromAssistant ? "assistant" : "user", message.Message))
            .Append(new OllamaChatMessage("user", prompt, imageData is null ? null : [ imageData ])).ToArray();
        var chatRequest = new OllamaChatRequest(attachment is null ? "llama3" : "llava", chatMessages);

        var responseRaw = await (await client.PostAsync("/api/chat",
                new StringContent(JsonConvert.SerializeObject(chatRequest), Encoding.UTF8, "application/json")))
            .Content.ReadAsStringAsync();

        var response = JsonConvert.DeserializeObject<OllamaChatResponse>(responseRaw)!;

        await context.RespondAsync(response.Message.Message);

        var userMessage = new AiMessage { FromAssistant = false, Message = prompt, MemberId = member.MemberId, Image = imageData };
        var assistantMessage = new AiMessage { FromAssistant = true, Message = response.Message.Message, MemberId = member.MemberId };

        await database.Messages.AddAsync(userMessage);
        await database.Messages.AddAsync(assistantMessage);
    
        await database.SaveChangesAsync();
    }

    [Command("history"), Description("Get conversation history")]
    public async ValueTask HistoryAsync(CommandContext context)
    {
        await context.DeferResponseAsync();
        
        var member = await database.Members.Include(botMember => botMember.Messages).FirstAsync(botMember => botMember.MemberId == context.User.Id);

        var sb = new StringBuilder();
        foreach (var message in member.Messages)
        {
            string user = message.FromAssistant ? "`AI`" : $"`{context.User.Username}`";
            sb.AppendLine($"{user}: {message.Message}");
        }
        
        await context.RespondAsync(sb.ToString());
    }

    [Command("delete"), Description("Delete the chat")]
    public async ValueTask DeleteAsync(CommandContext context)
    {
        await context.DeferResponseAsync();
        
        var member = await database.Members.FindAsync(context.User.Id);
        if (member is null)
        {
            await context.RespondAsync("You do not have an active chat.");
            return;
        }

        database.Members.Remove(member);
        await database.SaveChangesAsync();
        await context.RespondAsync("You deleted the chat.");
    }
}