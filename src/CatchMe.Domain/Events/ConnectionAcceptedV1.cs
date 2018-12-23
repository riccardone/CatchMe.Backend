using System.Collections.Generic;
using Evento;

namespace CatchMe.Domain.Events
{
    public class ConnectionAcceptedV1 : Event
    {
        public ConnectionAcceptedV1(IDictionary<string, string> metadata)
        {
            Metadata = metadata;
        }

        public IDictionary<string, string> Metadata { get; }
    }
}
