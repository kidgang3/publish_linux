using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ItemBot
{
    public static class ItemBidTimer
    {
        private static System.Timers.Timer? _timer;
        private static SocketTextChannel? _channel;

        public static void StartTimer(SocketTextChannel channel)
        {
            _channel = channel;

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public static void OnTimerElapsed(object source, ElapsedEventArgs e)
        {

        }
    }
}
