using System.Collections.Generic;
using Evento;

namespace CatchMe.Domain.Events
{
    public class ConnectionEstablishedV1 : Event
    {
        public string OwnerId { get; }
        public string FriendId { get; }
        public IDictionary<string, string> Metadata { get; }

        public ConnectionEstablishedV1(string ownerId, string friendId, IDictionary<string, string> metadata)
        {
            OwnerId = ownerId;
            FriendId = friendId;
            Metadata = metadata;
        }
    }
}
