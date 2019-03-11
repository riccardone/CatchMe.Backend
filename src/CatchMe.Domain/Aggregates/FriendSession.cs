using System;
using System.Collections.Generic;
using CatchMe.Domain.Commands;
using CatchMe.Domain.Events;
using Evento;

namespace CatchMe.Domain.Aggregates
{
    public class FriendSession : AggregateBase
    {
        private string _ownerId;
        public override string AggregateId => _aggregateId;
        private string _aggregateId;
        private string _friendId;
        private bool _active;
        private DateTime _connectionEstablishedAt;
        private DateTime _disconnectedAt;

        public FriendSession()
        {
            RegisterTransition<ConnectionEstablishedV1>(Apply);
            RegisterTransition<ConnectionAcceptedV1>(Apply);
            RegisterTransition<FriendDisconnectedV1>(Apply);
        }

        private void Apply(FriendDisconnectedV1 obj)
        {
            _active = false;
            _disconnectedAt = DateTime.Parse(obj.Metadata["applies"]);
        }

        private void Apply(ConnectionAcceptedV1 obj)
        {
            _active = true;
            _connectionEstablishedAt = DateTime.Parse(obj.Metadata["applies"]);
        }

        public FriendSession(string ownerId, string friendId, IDictionary<string, string> metadata) : this()
        {
            RaiseEvent(new ConnectionEstablishedV1(ownerId, friendId, metadata));
        }

        private void Apply(ConnectionEstablishedV1 obj)
        {
            _aggregateId = obj.Metadata["$correlationId"];
            _friendId = obj.FriendId;
            _ownerId = obj.OwnerId;
        }

        public static FriendSession Create(SendConnectionRequest command)
        {
            Ensure.NotNullOrWhiteSpace(command.OwnerId, nameof(command.OwnerId));
            Ensure.NotNullOrWhiteSpace(command.FriendId, nameof(command.FriendId));
            ValidateRequiredMetadata(command);

            return new FriendSession(command.OwnerId, command.FriendId, command.Metadata);
        }

        public void AcceptConnectionRequest(AcceptConnectionRequest command)
        {
            ValidateRequiredMetadata(command);

            RaiseEvent(new ConnectionAcceptedV1(command.Metadata));
        }

        public void Disconnect(DisconnectFriend command)
        {
            ValidateRequiredMetadata(command);

            RaiseEvent(new FriendDisconnectedV1(command.Metadata));
        }

        public override string ToString()
        {
            return $"OwnerId: {_ownerId}, FriendId: {_friendId}, AggregateId {_aggregateId}, Active: {_active}";
        }

        private static void ValidateRequiredMetadata(Message msg)
        {
            Ensure.NotNull(msg.Metadata, nameof(msg.Metadata));
            Ensure.NotNullOrWhiteSpace(msg.Metadata["$correlationId"], "$correlationId");
            Ensure.NotNullOrWhiteSpace(msg.Metadata["Applies"], "applies");
        }
    }
}
