using Microsoft.EntityFrameworkCore;

namespace Dashboard.Database;

public class OpenIddictDbContext(DbContextOptions options) : DbContext(options)
{
    
}