using System;
using System.Collections.Generic;
using CatchMe.Domain.Commands;
using CatchMe.Domain.Events;
using Evento;

namespace CatchMe.Domain.Aggregates
{
    public class PositionTracker : AggregateBase
    {
        public override string AggregateId => _aggregateId;
        private string _aggregateId;
        private IDictionary<DateTime, Position> _positions;

        public PositionTracker()
        {
            _positions = new Dictionary<DateTime, Position>();
            RegisterTransition<TrackingPositionStartedV1>(Apply);
            RegisterTransition<GeoInfoUpdatedV1>(Apply);
        }

        public PositionTracker(IDictionary<string, string> metadata) : this()
        {
            RaiseEvent(new TrackingPositionStartedV1(metadata));
        }

        private void Apply(TrackingPositionStartedV1 obj)
        {
            _aggregateId = obj.Metadata["$correlationId"];
        }

        private void Apply(GeoInfoUpdatedV1 obj)
        {
            _positions.Add(DateTime.Parse(obj.Metadata["Applies"]),
                new Position(obj.Longitude, obj.Latitude, obj.Speed, obj.Heading, obj.Altitude));
        }

        public static PositionTracker Start(StartTrackingPosition command)
        {
            ValidateRequiredMetadata(command);

            return new PositionTracker(command.Metadata);
        }

        public void Track(SaveGeoInfo command)
        {
            ValidateRequiredMetadata(command);

            RaiseEvent(new GeoInfoUpdatedV1(command.Longitude, command.Latitude, command.Speed, command.Heading,
                command.Altitude, command.Metadata));
        }

        private static void ValidateRequiredMetadata(Message msg)
        {
            Ensure.NotNull(msg.Metadata, nameof(msg.Metadata));
            Ensure.NotNullOrWhiteSpace(msg.Metadata["$correlationId"], "$correlationId");
            Ensure.NotNullOrWhiteSpace(msg.Metadata["Applies"], "Applies");
        }
    }
}
