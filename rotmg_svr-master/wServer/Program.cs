using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using wServer.realm;
using common;
using System.Net.NetworkInformation;
using wServer.networking;
using System.Globalization;
using log4net;
using log4net.Config;
using System.IO;

namespace wServer
{
    static class Program
    {
        internal static SimpleSettings Settings;

        static ILog log = LogManager.GetLogger("Server");

        static void Main(string[] args)
        {
            Console.Title = "Stable World Server";
            Console.WindowWidth = 110;
            XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            using (Settings = new SimpleSettings("wServer"))
            using (var db = new Database(
                        Settings.GetValue<string>("db_host", "127.0.0.1"),
                        Settings.GetValue<int>("db_port", "6379"),
                        Settings.GetValue<string>("db_auth", "")))
            {
                RealmManager manager = new RealmManager(
                    Settings.GetValue<int>("maxClient", "100"),
                    Settings.GetValue<int>("tps", "20"),
                    db);

                manager.Initialize();
                manager.Run();

                Server server = new Server(manager, 2050);
                PolicyServer policy = new PolicyServer();

                Console.CancelKeyPress += (sender, e) => e.Cancel = true;

                policy.Start();
                server.Start();
                log.Info("Server initialized.");

                while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;

                log.Info("Terminating...");
                server.Stop();
                policy.Stop();
                manager.Stop();
                db.Dispose();
                log.Info("Server terminated.");
            }
        }
    }
}
