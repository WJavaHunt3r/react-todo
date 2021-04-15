using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactTodo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactTodo.Bll
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddReactTodoBll(this IServiceCollection services, string connectionString) =>
            services.AddTodoContext(connectionString).AddTodoItemService();

        public static IServiceCollection AddTodoItemService(this IServiceCollection services) =>
            services.AddScoped<ITodoItemService, TodoItemService>();

        public static IServiceCollection AddTodoContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(connectionString));

        public static async Task MigrateOrReacreateReactTodoDatabaseAsync(this IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<TodoContext>();
            var allMigrations = dbContext.Database.GetMigrations().ToHashSet();
            var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
            if (appliedMigrations.Any(m => !allMigrations.Contains(m)))
            {
                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.MigrateAsync();
            }
            else if (allMigrations.Any(m => !appliedMigrations.Contains(m)))
                await dbContext.Database.MigrateAsync();
        }
    }
}
