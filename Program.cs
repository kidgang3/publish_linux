
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;

namespace ItemBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public string getToken()
        {
            return ConfigurationManager.AppSettings["token"];
        }

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();

                client.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                // Tokens should be considered secret data and never hard-coded.
                // We can read from the environment variable to avoid hard coding.
                //var token = Environment.GetEnvironmentVariable("token");
                await client.LoginAsync(TokenType.Bot, getToken());
                await client.StartAsync();

                // Here we initialize the logic required to register our commands.
                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
                })
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }


        private Task LogAsync(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());  //로그 출력
            return Task.CompletedTask;
        }

    }
}