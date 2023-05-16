using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemBot
{
    public class ItemBidInfo
    {
        public string Guid { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public DateTime ExpireTime { get; set; }
        public List<ulong> BidUserIdList { get; set; } = new List<ulong>();
        public ulong MessageId { get; set; }
        public ulong bidUserId { get; set; }
        public bool isComplete { get; set; }
    }

    public class UserItemPaymentInfo
    {
        public ulong UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ulong paymentGem { get; set; }
    }
}
