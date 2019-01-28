using System;
using CatchMe.Adapter;
using Evento.Repository;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;

namespace CatchMe.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var connBuilder = new ConnectionBuilder(new Uri("eventstore:1113"), ConnectionSettings.Default, "catchme-subscriber",
                new UserCredentials("admin", "changeit"));
            var repo = new EventStoreDomainRepository("catchme", connBuilder.Build());
            var endpoint = new EndPoint(repo, connBuilder, new Handlers(repo));
            endpoint.Start();

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }
}
