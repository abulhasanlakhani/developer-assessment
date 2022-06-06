using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TodoList.Infrastructure.Data;

namespace TodoList.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using TodoContext context = new();
            // Temporary way of creating / testing database
            context.Database.EnsureCreated();

            context.TodoItems.AddRange(Seed.TodoList);
            context.SaveChanges();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
