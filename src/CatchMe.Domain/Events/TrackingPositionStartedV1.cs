using System.Collections.Generic;
using Evento;

namespace CatchMe.Domain.Events
{
    public class TrackingPositionStartedV1 : Event
    {
        public IDictionary<string, string> Metadata { get; }

        public TrackingPositionStartedV1(IDictionary<string, string> metadata)
        {
            Metadata = metadata;
        }
    }
}
