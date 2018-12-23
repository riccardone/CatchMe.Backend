﻿using System.Collections.Generic;
using Evento;

namespace CatchMe.Domain.Commands
{
    public class SaveGeoInfo : Command
    {
        public double Longitude { get; }
        public double Latitude { get; }
        public double Speed { get; }
        public double Heading { get; }
        public double Altitude { get; }
        public IDictionary<string, string> Metadata { get; }

        public SaveGeoInfo(double longitude, double latitude, double speed, double heading, double altitude, IDictionary<string, string> metadata)
        {
            Longitude = longitude;
            Latitude = latitude;
            Speed = speed;
            Heading = heading;
            Altitude = altitude;
            Metadata = metadata;
        }
    }
}
