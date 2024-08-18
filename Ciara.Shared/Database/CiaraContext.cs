using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ciara.Shared.Database;

public class CiaraContext(IConfiguration config) : DbContext
{
    public DbSet<BotMember> Members { get; set; }
    public DbSet<BotGuild> Guilds { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(
        config["Database:ConnectionString"])
        .EnableDetailedErrors();
}

public class BotMember
{
    [Key] public required byte[] IdHash { get; set; }
}

public class BotGuild
{
    [Key] public required byte[] IdHash { get; set; }
    public ulong? MemberJoinViewChannelId { get; set; }
}