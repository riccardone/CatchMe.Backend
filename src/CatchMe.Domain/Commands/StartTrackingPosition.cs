using System.Collections.Generic;
using Evento;

namespace CatchMe.Domain.Commands
{
    public class StartTrackingPosition : Command
    {
        public IDictionary<string, string> Metadata { get; }

        public StartTrackingPosition( IDictionary<string, string> metadata)
        {
            Metadata = metadata;
        }
    }
}
