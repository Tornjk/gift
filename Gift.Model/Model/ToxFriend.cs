using LiteDB;
using System.Collections.Generic;

namespace Gift.Model
{
    public class ToxFriend
    {
        public ObjectId Id { get; set; }

        public byte[] PublicKey { get; set; }

        public string Alias { get; set; }

        public List<ToxFriendMessage> History { get; set; }
    }
}