using System.Collections.Generic;
using Evento;

namespace CatchMe.Domain.Commands
{
    public class DisconnectFriend : Command
    {
        public IDictionary<string, string> Metadata { get; }

        public DisconnectFriend(IDictionary<string, string> metadata)
        {
            Metadata = metadata;
        }
    }
}
