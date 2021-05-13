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
    /// <summary>
    /// Custom Dependeny injection
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds the 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns>service collection</returns>
        public static IServiceCollection AddReactTodoBll(this IServiceCollection services, string connectionString) =>
            services.AddTodoContext(connectionString).AddTodoItemService().AddBoardService();

        /// <summary>
        /// Adds the TodoItemService to teh collection
        /// </summary>
        /// <param name="services">The srevices to be added to</param>
        /// <returns></returns>
        public static IServiceCollection AddTodoItemService(this IServiceCollection services) =>
            services.AddScoped<ITodoItemService, TodoItemService>();

        /// <summary>
        /// Adds the BoardService To the service collection
        /// </summary>
        /// <param name="services">The service collection to be added to</param>
        /// <returns></returns>
        public static IServiceCollection AddBoardService(this IServiceCollection services) =>
            services.AddScoped<IBoardService, BoardService>();

        /// <summary>
        /// Adds the DbContext to the service with the given connection string
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">The Connection string to connect to the database</param>
        /// <returns></returns>
        public static IServiceCollection AddTodoContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(connectionString));

        /// <summary>
        /// Mighrates or creates the database if not created yet
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
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
