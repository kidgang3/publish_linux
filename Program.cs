
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ItemBot
{
    class Program
    {
        // 섬멸 아이템봇
        string token = "MTEwNzY2NTg5ODA3ODIyNDQxNQ.GLfRWs.l0J2H8WfmVn-klUy9zvP2rAoU1uQyhLb6unHiE";
        // 동화 아이템봇
        //string token = "MTEwODA0ODAyODI2ODQ5ODk1NA.G8AckS.qrwGJ6HpagCcUSdarFLb4OXPoSOF7yG4E2zrX4";

        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
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
                await client.LoginAsync(TokenType.Bot, token);
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