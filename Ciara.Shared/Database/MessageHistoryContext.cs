using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ciara.Shared.Database;

public class MessageHistoryContext : DbContext
{
    public DbSet<BotMember> Members { get; set; }
    public DbSet<AiMessage> Messages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(
        Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
        "Server=localhost;Port=5432;Database=Ciara;User Id=postgres;Password=Ceira123#;")
        .EnableDetailedErrors();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BotMember>()
            .HasMany(e => e.Messages)
            .WithOne(e => e.Member)
            .HasForeignKey(e => e.MemberId)
            .HasPrincipalKey(e => e.MemberId);
        
        modelBuilder.Entity<AiMessage>()
            .Property(e => e.MessageId)
            .UseHiLo()
            .ValueGeneratedOnAdd();
    }
}

public class BotMember
{
    [Key]
    public ulong MemberId { get; set; }

    public bool TermsAccepted { get; set; }
    
    public ICollection<AiMessage> Messages { get; set; }
}

public class AiMessage
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MessageId { get; set; }
    public bool FromAssistant { get; set; }
    public string Message { get; set; }
    public string? Image { get; set; }
    
    public ulong MemberId { get; set; }
    public BotMember Member { get; set; }
}