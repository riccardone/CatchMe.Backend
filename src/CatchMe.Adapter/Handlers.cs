using CatchMe.Domain.Aggregates;
using CatchMe.Domain.Commands;
using Evento;

namespace CatchMe.Adapter
{
    public class Handlers :
        IHandle<SendConnectionRequest>,
        IHandle<AcceptConnectionRequest>,
        IHandle<DisconnectFriend>,
        IHandle<SaveGeoInfo>
    {
        private readonly IDomainRepository _repository;

        public Handlers(IDomainRepository repository)
        {
            _repository = repository;
        }

        public IAggregate Handle(SendConnectionRequest command)
        {
            FriendSession aggregate;
            try
            {
                return _repository.GetById<FriendSession>(command.Metadata["$correlationId"], 5);
            }
            catch (AggregateNotFoundException)
            {
                aggregate = FriendSession.Create(command);
            }
            return aggregate;
        }

        public IAggregate Handle(AcceptConnectionRequest command)
        {
            var aggregate = _repository.GetById<FriendSession>(command.Metadata["$correlationId"], 5);
            aggregate.AcceptConnectionRequest(command);
            return aggregate;
        }

        public IAggregate Handle(DisconnectFriend command)
        {
            var aggregate = _repository.GetById<FriendSession>(command.Metadata["$correlationId"], 5);
            aggregate.Disconnect(command);
            return aggregate;
        }

        public IAggregate Handle(SaveGeoInfo command)
        {
            PositionTracker aggregate;
            try
            {
                aggregate = _repository.GetById<PositionTracker>(command.Metadata["$correlationId"], 5);
                aggregate.Track(command);
            }
            catch (AggregateNotFoundException)
            {
                aggregate = PositionTracker.Start(command);
            }
            return aggregate;
        }
    }
}
