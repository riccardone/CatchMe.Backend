using System;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace CatchMe.Adapter
{
    public class ConnectionBuilder : IConnectionBuilder
    {
        public Uri ConnectionString { get; }
        public ConnectionSettings ConnectionSettings { get; }
        public string ConnectionName { get; }
        public UserCredentials Credentials { get; }

        public IEventStoreConnection Build(bool open = false)
        {
            var connection = EventStoreConnection.Create(ConnectionSettings, ConnectionString, ConnectionName);
            if (open)
                connection.ConnectAsync().Wait();

            return connection;
        }

        public ConnectionBuilder(Uri connectionString, ConnectionSettings connectionSettings, string connectionName, UserCredentials credentials)
        {
            ConnectionString = connectionString;
            ConnectionSettings = connectionSettings;
            ConnectionName = connectionName;
            Credentials = credentials;
        }
    }
}
