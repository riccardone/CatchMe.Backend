using System.Collections.Generic;
using Evento;

namespace CatchMe.Domain.Events
{
    public class FriendDisconnectedV1 : Event
    {
        public FriendDisconnectedV1(IDictionary<string, string> metadata)
        {
            Metadata = metadata;
        }

        public IDictionary<string, string> Metadata { get; }
    }
}
