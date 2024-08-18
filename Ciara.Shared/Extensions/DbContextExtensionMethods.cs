using Microsoft.EntityFrameworkCore;

namespace Ciara.Shared.Extensions;

public static class DbContextExtensionMethods
{
    public static async Task<T> FindOrCreateAsync<T>(this DbSet<T> dbSet, object id, Func<T> createEntity, DbContext context) where T : class
    {
        var entity = await dbSet.FindAsync(id);

        if (entity is null)
        {
            entity = createEntity();
            dbSet.Add(entity);
            await context.SaveChangesAsync();
        }

        return entity;
    }
}
