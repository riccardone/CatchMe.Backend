namespace CatchMe.Domain.Aggregates.ValueObjects
{
    public class Position
    {
        public double Lon { get; }
        public double Lat { get; }
        public double? Speed { get; }
        public double? Heading { get; }
        public double? Altitude { get; }

        public Position(double lon, double lat, double? speed, double? heading, double? altitude)
        {
            Lon = lon;
            Lat = lat;
            Speed = speed;
            Heading = heading;
            Altitude = altitude;
        }
    }
}
