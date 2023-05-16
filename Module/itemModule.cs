using Discord;
using Discord.Commands;
using DiscordBot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemBot
{
    public class itemModule : AbsModule
    {
        public override ulong getChannelId()
        {
            // 섬멸 - 뽑기
            ulong channelId = 1044924809156493394;
            // 동화 - 아이템
            //ulong channelId = 1108046986764095508;

            return channelId;
        }

        [Command("뽑기")]
        public async Task RandomCommand(params string[] nickNames)
        {
            //if(!Context.Channel.Id.Equals(itemChannelId))
            // {
            //   return;
            // }

            Random random = new Random();
            int target = random.Next(0, nickNames.Length);

            StringBuilder sb = new StringBuilder();
            sb.Append("```");
            sb.Append("\n");
            sb.Append("뽑기 결과");
            sb.Append("\n");
            sb.Append("[" + nickNames[target] + "]");
            sb.Append("\n");
            sb.Append("```");

            await Context.Channel.SendMessageAsync(sb.ToString());
        }

        [Command("입찰등록")]
        public async Task ItemBidCommand(string bossGuid, string itemName, string price, string expireHour)
        {
            var now = DateTime.Now;
            int expireAddHour;
            int.TryParse(expireHour, out expireAddHour);
            var expireTime = now.AddHours(expireAddHour);

            StringBuilder sb = new StringBuilder();
            sb.Append("```scss");
            sb.Append("\n");
            sb.Append("등록 물품 : " + itemName + "(입찰가 : " + price);
            sb.Append("\n");
            sb.Append("입찰 마감 : " + expireTime.ToString("HH:mm:ss"));
            sb.Append("\n");
            sb.Append("```");

            var itemGuid = Guid.NewGuid().ToString();

            var button = new ComponentBuilder()
                .WithButton("입찰", itemGuid, ButtonStyle.Success)
                ;

            await ReplyAsync(sb.ToString(), components: button.Build());
        }
    }
}
