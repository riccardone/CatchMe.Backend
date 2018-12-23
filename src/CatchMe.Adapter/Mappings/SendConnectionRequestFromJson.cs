using System.Collections.Generic;
using Evento;
using Newtonsoft.Json;

namespace CatchMe.Adapter.Mappings
{
    public class SendConnectionRequestFromJson : Command
    {
        public SendConnectionRequestFromJson(string bodyAsJson, string metadataAsJson)
        {
            var body = JsonConvert.DeserializeObject<dynamic>(bodyAsJson);
            var metadata = JsonConvert.DeserializeObject<IDictionary<string, string>>(metadataAsJson);
           
            OwnerId = body.ownerId.Value;
            FriendId = body.friendId.Value;
            Metadata = metadata;
        }

        public string FriendId { get; set; }

        public string OwnerId { get; set; }

        public IDictionary<string, string> Metadata { get; }
    }
}
