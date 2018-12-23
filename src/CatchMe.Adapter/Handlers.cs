using System;
using CatchMe.Domain.Aggregates;
using CatchMe.Domain.Commands;
using Evento;

namespace CatchMe.Adapter
{
    public class Handlers :
        IHandle<SendConnectionRequest>,
        IHandle<AcceptConnectionRequest>,
        IHandle<DisconnectFriend>,
        IHandle<SaveGeoInfo>,
        IHandle<StartTrackingPosition>
    {
        private readonly IDomainRepository _repository;
        //private readonly IDiaryCache _diaryCache;

        public Handlers(IDomainRepository repository)
        {
            _repository = repository;
            //_diaryCache = diaryCache;
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
            throw new NotImplementedException();
        }

        public IAggregate Handle(DisconnectFriend command)
        {
            throw new NotImplementedException();
        }

        public IAggregate Handle(SaveGeoInfo command)
        {
            throw new NotImplementedException();
        }

        public IAggregate Handle(StartTrackingPosition command)
        {
            throw new NotImplementedException();
        }
    }
}
