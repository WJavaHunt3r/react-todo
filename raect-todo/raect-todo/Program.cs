using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReactTodo.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raect_todo
{
    public static class Program
    {
        public static async Task Main(string[] args) =>
            (await CreateHostBuilder(args)
                    .Build()
                    .MigrateOrReacreateDatabaseAsync())
                .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        private static async Task<IHost> MigrateOrReacreateDatabaseAsync(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            await scope.ServiceProvider.MigrateOrReacreateReactTodoDatabaseAsync();
            return host;
        }
    }
}
