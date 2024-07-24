using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ciara.Shared.Database;

public class CiaraContextFactory : IDesignTimeDbContextFactory<CiaraContext>
{
    public CiaraContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<CiaraContext>().Build();
        return new CiaraContext(config);
    }
}