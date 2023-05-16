using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DiscordBot;
using RedisKey = DiscordBot.RedisKey;
using System.Configuration;

namespace ItemBot
{
    public class CommandHandler : AbsButtonHandler
    { 
        public CommandHandler(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
            _client.GuildAvailable += GuildAvailable;
            _client.MessageReceived += MessageReceivedAsync;
            _client.ButtonExecuted += ButtonHandler;
        }

        public override async Task InitializeAsync()
        {
            if(_commands == null )
            {
                return;
            }
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public override async Task MessageReceivedAsync(SocketMessage msg)
        {
            //수신한 메시지가 사용자가 보낸 게 아닐 때 취소
            if (!(msg is SocketUserMessage message))
                return;

            /*if (message.Source != MessageSource.User)
                return;*/

            if (!message.Channel.Id.Equals(getChannelId()))
                return;

            int argPos = 0;

            if (_client == null)
                return;

            //자신이 호출된게 아니거나 다른 봇이 호출했다면 취소
            if (message.HasMentionPrefix(_client.CurrentUser, ref argPos)/* || message.Author.IsBot*/)
                return;

            // 프리픽스 유무
            if (!message.HasCharPrefix('.', ref argPos))
                return;


            var context = new SocketCommandContext(_client, message);                    //수신된 메시지에 대한 컨텍스트 생성   

            if (_commands == null)
                return;

            //모듈이 명령어를 처리하게 설정
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public override async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            var embed = getEmbed($"잘못된 메시지 입력입니다.");

            await context.Channel.SendMessageAsync("", false, embed);
        }

        public override async Task GuildAvailable(SocketGuild arg)
        {
            var textChannels = arg.TextChannels;
            SocketTextChannel botChannel;
            if (textChannels != null)
            {
                botChannel = arg.GetTextChannel(getChannelId());
            }
            else
            {
                return;
            }

            if (botChannel != null)
            {
                var seasonStr = RedisDBManager.Instance.Get(RedisKey.Season.ToString());
                if (seasonStr == null)
                {
                    RedisDBManager.Instance.Set(RedisKey.Season.ToString(), "1");
                }


                EmbedBuilder eb = new EmbedBuilder();
                eb.Color = Color.Blue;
                eb.Description = "섬멸 아이템 분배 봇이 접속되었습니다";
                //eb.AddField("현재 시즌", season);

                await botChannel.SendMessageAsync("", false, eb.Build());

                ItemBidTimer.StartTimer(botChannel);
            }
        }

        public override async Task ButtonHandler(SocketMessageComponent component)
        {
            var customId = component.Data.CustomId;
        }

        public static Embed getEmbed(string message)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.Color = Color.Blue;
            eb.Description = message;

            return eb.Build();
        }

        public string getSeasonRedisKey(RedisSeasonKey key, int season)
        {
            return key.ToString() + "_" + season;
        }

        public string getItemBidUserList(ItemBidInfo itemBidInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("```yaml");
            sb.Append("\n");
            sb.Append(itemBidInfo.Name);
            sb.Append("\n");
            if (itemBidInfo.BidUserIdList.Count > 0)
            {
                
            }

            sb.Append("```");

            return sb.ToString();
        }


        public override ulong getChannelId()
        {
            var chanelIdStr = ConfigurationManager.AppSettings["itemChannelId"];
            ulong channelId;
            ulong.TryParse(chanelIdStr, out channelId);
            return channelId;
        }
    }
}
