using System.Collections.Generic;
using Evento;

namespace CatchMe.Domain.Commands
{
    public class SendConnectionRequest : Command
    {
        public string OwnerId { get; }
        public string FriendId { get; }
        public IDictionary<string, string> Metadata { get; }

        public SendConnectionRequest(string ownerId, string friendId, IDictionary<string, string> metadata)
        {
            OwnerId = ownerId;
            FriendId = friendId;
            Metadata = metadata;
        }
    }
}
