using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CatchMe.Adapter.Mappings;
using CatchMe.Domain.Commands;
using Evento;
using EventStore.ClientAPI;
using NLog;

namespace CatchMe.Adapter
{
    public class EndPoint
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly IDomainRepository _domainRepository;
        private IEventStoreConnection _connection;
        private readonly IConnectionBuilder _connectionBuilder;
        private const string InputStream = "input-catchme";
        private const string PersistentSubscriptionGroup = "catchme-processors";
        private readonly Dictionary<string, Func<string[], Command>> _deserialisers;
        private readonly Dictionary<string, Func<object, IAggregate>> _eventHandlerMapping;
        private readonly Handlers _handlers;

        public EndPoint(IDomainRepository domainRepository, IConnectionBuilder connectionBuilder, Handlers handlers)
        {
            _deserialisers = CreateDeserialisersMapping();
            _eventHandlerMapping = CreateEventHandlerMapping();
            _domainRepository = domainRepository;
            _connectionBuilder = connectionBuilder;
            _handlers = handlers;
        }

        public async Task Start()
        {
            _connection = _connectionBuilder.Build();
            _connection.Connected += _connection_Connected;
            _connection.Disconnected += _connection_Disconnected;
            _connection.ErrorOccurred += _connection_ErrorOccurred;
            _connection.Closed += _connection_Closed;
            _connection.Reconnecting += _connection_Reconnecting;
            _connection.AuthenticationFailed += _connection_AuthenticationFailed;
            await _connection.ConnectAsync();
            Log.Info($"Listening from '{InputStream}' stream");
            Log.Info($"Joined '{PersistentSubscriptionGroup}' group");
            Log.Info($"Log EndPoint started");
        }

        private void _connection_AuthenticationFailed(object sender, ClientAuthenticationFailedEventArgs e)
        {
            Log.Error($"EndpointConnection AuthenticationFailed: {e.Reason}");
        }

        private void _connection_Reconnecting(object sender, ClientReconnectingEventArgs e)
        {
            Log.Warn($"EndpointConnection Reconnecting...");
        }

        private void _connection_Closed(object sender, ClientClosedEventArgs e)
        {
            Log.Info($"EndpointConnection Closed: {e.Reason}");
        }

        private async Task CreateSubscription()
        {
            await _connection.CreatePersistentSubscriptionAsync(InputStream, PersistentSubscriptionGroup,
                PersistentSubscriptionSettings.Create().StartFromBeginning().DoNotResolveLinkTos(),
                _connectionBuilder.Credentials);
        }

        private static void _connection_ErrorOccurred(object sender, ClientErrorEventArgs e)
        {
            Log.Error($"EndpointConnection ErrorOccurred: {e.Exception.Message}");
        }

        private static void _connection_Disconnected(object sender, ClientConnectionEventArgs e)
        {
            Log.Error($"EndpointConnection Disconnected from {e.RemoteEndPoint}");
        }

        private async void _connection_Connected(object sender, ClientConnectionEventArgs e)
        {
            Log.Info($"EndpointConnection Connected to {e.RemoteEndPoint}");
            try
            {
                await CreateSubscription();
            }
            catch (Exception)
            {
                // already exist
            }
            await Subscribe();
        }

        public void Stop()
        {
            _connection.Close();
        }

        private async Task Subscribe()
        {
            await _connection.ConnectToPersistentSubscriptionAsync(InputStream, PersistentSubscriptionGroup, EventAppeared, SubscriptionDropped);
        }

        private Task EventAppeared(EventStorePersistentSubscriptionBase eventStorePersistentSubscriptionBase, ResolvedEvent resolvedEvent)
        {
            try
            {
                Process(resolvedEvent.Event.EventType, resolvedEvent.Event.Metadata, resolvedEvent.Event.Data);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                eventStorePersistentSubscriptionBase.Fail(resolvedEvent, PersistentSubscriptionNakEventAction.Park,
                    ex.GetBaseException().Message);
            }
            return Task.CompletedTask;
        }

        private void Process(string eventType, byte[] metadata, byte[] data)
        {
            if (!_deserialisers.ContainsKey(eventType))
                return;

            var command = _deserialisers[eventType](new[]
            {
                Encoding.UTF8.GetString(metadata),
                Encoding.UTF8.GetString(data)
            });

            if (command == null)
            {
                Log.Error($"Message format not recognised! EventType: {eventType}");
                return;
            }

            foreach (var key in _eventHandlerMapping.Keys)
            {
                if (!eventType.EndsWith(key))
                    continue;
                var aggregate = _eventHandlerMapping[key](command);
                _domainRepository.Save(aggregate);
                Log.Debug($"Handled '{eventType}' AggregateId: {aggregate.AggregateId}");
                return;
            }
            throw new Exception($"I can't find any handler for {eventType}");
        }

        private static void SubscriptionDropped(EventStorePersistentSubscriptionBase eventStorePersistentSubscriptionBase, SubscriptionDropReason subscriptionDropReason, Exception arg3)
        {
            Log.Error(subscriptionDropReason.ToString(), arg3);
        }

        private static Dictionary<string, Func<string[], Command>> CreateDeserialisersMapping()
        {
            return new Dictionary<string, Func<string[], Command>>
            {
                {"PositionReceived", ToSaveGeoInfo}
            };
        }
        private Dictionary<string, Func<object, IAggregate>> CreateEventHandlerMapping()
        {
            return new Dictionary<string, Func<object, IAggregate>>
            {
                {"PositionReceived", o => _handlers.Handle(o as SaveGeoInfo)}
            };
        }

        private static Command ToSaveGeoInfo(string[] arg)
        {
            return new SaveGeoInfoFromJson(arg[1], arg[0]);
        }
    }
}
