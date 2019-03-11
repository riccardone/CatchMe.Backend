﻿using System;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace CatchMe.Adapter
{
    public interface IConnectionBuilder
    {
        Uri ConnectionString { get; }
        ConnectionSettings ConnectionSettings { get; }
        string ConnectionName { get; }
        UserCredentials Credentials { get; }
        IEventStoreConnection Build(bool open = false);
    }
}
