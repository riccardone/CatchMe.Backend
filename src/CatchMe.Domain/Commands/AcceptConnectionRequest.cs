using System.Collections.Generic;
using Evento;

namespace CatchMe.Domain.Commands
{
    public class AcceptConnectionRequest : Command
    {
        public string OwnerId { get; }
        public string FriendId { get; }
        public IDictionary<string, string> Metadata { get; }

        public AcceptConnectionRequest(string ownerId, string friendId, IDictionary<string, string> metadata)
        {
            OwnerId = ownerId;
            FriendId = friendId;
            Metadata = metadata;
        }
    }
}
