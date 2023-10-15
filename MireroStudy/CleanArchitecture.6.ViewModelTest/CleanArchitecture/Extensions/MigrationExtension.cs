using Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Extensions;

public static class MigrationExtension
{
    public static async Task<WebApplication> Migration(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            // TODO: need to add check here to only run migrations if it was applicable
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync();
        }
        return app;
    }
}
