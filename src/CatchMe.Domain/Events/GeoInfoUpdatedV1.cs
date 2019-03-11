using System.Collections.Generic;
using Evento;

namespace CatchMe.Domain.Events
{
    public class GeoInfoUpdatedV1 : Event
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double? Altitude { get; set; }
        public double? Heading { get; set; }
        public double? Speed { get; set; }
        public long? Timestamp { get; set; }
        public double Accuracy { get; set; }
        public IDictionary<string, string> Metadata { get; }

        public GeoInfoUpdatedV1(double longitude, double latitude, double? speed, double? heading, double? altitude,
            long? timestamp, double accuracy, IDictionary<string, string> metadata)
        {
            Longitude = longitude;
            Latitude = latitude;
            Speed = speed;
            Heading = heading;
            Altitude = altitude;
            Timestamp = timestamp;
            Accuracy = accuracy;
            Metadata = metadata;
        }
    }
}
