using System.Collections.Generic;
using CatchMe.Domain.Commands;
using Newtonsoft.Json;

namespace CatchMe.Adapter.Mappings
{
    public class SaveGeoInfoFromJson : SaveGeoInfo
    {
        public SaveGeoInfoFromJson(string bodyAsJson, string metadataAsJson) : base(
            (double)JsonConvert.DeserializeObject<dynamic>(bodyAsJson).longitude.Value,
            (double)JsonConvert.DeserializeObject<dynamic>(bodyAsJson).latitude.Value,
            JsonConvert.DeserializeObject<dynamic>(bodyAsJson).speed.Value as double?,
            JsonConvert.DeserializeObject<dynamic>(bodyAsJson).heading.Value as double?,
            JsonConvert.DeserializeObject<dynamic>(bodyAsJson).altitude.Value as double?,
            JsonConvert.DeserializeObject<dynamic>(bodyAsJson).timestamp.Value as long?,
            (double)JsonConvert.DeserializeObject<dynamic>(bodyAsJson).accuracy.Value,
            (IDictionary<string, string>)JsonConvert.DeserializeObject<IDictionary<string, string>>(metadataAsJson))
        {
            //var body = JsonConvert.DeserializeObject<dynamic>(bodyAsJson);
            //var metadata = JsonConvert.DeserializeObject<IDictionary<string, string>>(metadataAsJson);

            //Latitude = body.latitude.Value;
            //Longitude = body.longitude.Value;
            //Altitude = body.altitude.Value;
            //Heading = body.heading.Value;
            //Speed = body.speed.Value;
            //Timestamp = body.timestamp.Value;
            //Accuracy = body.accuracy.Value;

            //Metadata = metadata;
        }
    }
}
