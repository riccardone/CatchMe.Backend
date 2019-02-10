using System.Collections.Generic;
using Evento;
using Newtonsoft.Json;

namespace CatchMe.Adapter.Mappings
{
    public class SavePositionReceived : Command
    {
        public SavePositionReceived(string bodyAsJson, string metadataAsJson)
        {
            var body = JsonConvert.DeserializeObject<dynamic>(bodyAsJson);
            var metadata = JsonConvert.DeserializeObject<IDictionary<string, string>>(metadataAsJson);

            Latitude = body.ownerId.Value;
            Longitude = body.friendId.Value;
            // TODO
            Metadata = metadata;
        }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public IDictionary<string, string> Metadata { get; }
    }
}
