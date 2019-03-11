using System.Collections.Generic;
using Evento;

namespace CatchMe.Domain.Commands
{
    public class SaveGeoInfo : Command
    {
        public double Longitude { get; }
        public double Latitude { get; }
        public double? Altitude { get; }
        public double? Heading { get; }
        public double? Speed { get; }
        public long? Timestamp { get; }
        public double Accuracy { get; }
        public IDictionary<string, string> Metadata { get; }

        public SaveGeoInfo(double longitude, double latitude, double? speed, double? heading, double? altitude,
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
