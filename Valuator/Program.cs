using Microsoft.Extensions.Configuration;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace Valuator;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string connectionString = builder.Configuration.GetConnectionString("RedisConnection")!;
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);

        builder.Services.AddSingleton(redis);
        builder.Services.AddRazorPages();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}
