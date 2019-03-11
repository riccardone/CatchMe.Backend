using System;
using CatchMe.Adapter;
using Evento.Repository;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using NLog;

namespace CatchMe.Host
{
    class Program
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                ConfigureLogging();
                var subscriber = new ConnectionBuilder(new Uri("tcp://eventstore:1113"), ConnectionSettings.Default, "catchme-subscriber",
                    new UserCredentials("admin", "changeit"));
                var processing = new ConnectionBuilder(new Uri("tcp://eventstore:1113"), ConnectionSettings.Default, "catchme-processing",
                    new UserCredentials("admin", "changeit"));
                var repo = new EventStoreDomainRepository("catchme", processing.Build(true));
                var endpoint = new EndPoint(repo, subscriber, new Handlers(repo));
                endpoint.Start().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
            Log.Info("Press enter to exit");
            Console.ReadLine();
        }

        private static void ConfigureLogging()
        {
            var config = new NLog.Config.LoggingConfiguration();

            //var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            //config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;
        }
    }
}
