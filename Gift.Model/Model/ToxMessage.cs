using LiteDB;

namespace Gift.Model
{
    public class ToxFriendMessage
    {
        public ObjectId Id { get; set; }

        public string Message { get; set; }

        public long Timestamp { get; set; }

        public bool Sent { get; set; }
    }
}