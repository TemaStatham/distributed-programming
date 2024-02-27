using Microsoft.Extensions.Configuration;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System.Security.Authentication;

namespace Valuator;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string? connectionString = builder.Configuration.GetSection("Redis").Value;
        if (connectionString == null)
        {
            Console.WriteLine("Value cannot be null.(Parameter 'configuration')");
            return;
        }
        ConfigurationOptions redisConfiguration = new ConfigurationOptions();
        redisConfiguration.ConnectRetry = 16;
        redisConfiguration.ConnectTimeout = 5000;
        redisConfiguration.KeepAlive = 2;
        redisConfiguration.AbortOnConnectFail = false;
        /*redisConfiguration.Ssl = true;
        redisConfiguration.SslProtocols = SslProtocols.Tls12;*/
        redisConfiguration.EndPoints.Add(connectionString);


        IConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConfiguration);

        builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
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
