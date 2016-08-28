using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using common;
using log4net;
using log4net.Config;
using System.Globalization;
using System.Collections.Concurrent;

namespace server
{
    class Program
    {
        static HttpListener listener;

        internal static SimpleSettings Settings;
        internal static XmlData GameData;
        internal static Database Database;
        internal static string InstanceId;

        static ILog log = LogManager.GetLogger("Server");

        static void Main(string[] args)
        {
            Console.Title = "Stable Account Server";
            Console.WindowWidth = 110;
            XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.Name = "Entry";

            using (Settings = new SimpleSettings("server"))
            using (Database = new Database(
                    Settings.GetValue<string>("db_host", "127.0.0.1"),
                    Settings.GetValue<int>("db_port", "6379"),
                    Settings.GetValue<string>("db_auth", "")))
            {
                GameData = new XmlData();
                InstanceId = Guid.NewGuid().ToString();

                int port = Settings.GetValue<int>("port", "8888");

                listener = new HttpListener();
                listener.Prefixes.Add("http://*:" + port + "/");
                listener.Start();

                listener.BeginGetContext(ListenerCallback, null);
                Console.CancelKeyPress += (sender, e) => e.Cancel = true;
                log.Info("Listening at port " + port + "...");

                ISManager manager = new ISManager();
                manager.Run();

                while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;

                log.Info("Terminating...");
                manager.Dispose();
                listener.Stop();
                GameData.Dispose();
            }
        }

        static void ListenerCallback(IAsyncResult ar)
        {
            if (!listener.IsListening) return;
            var context = listener.EndGetContext(ar);
            listener.BeginGetContext(ListenerCallback, null);
            ProcessRequest(context);
        }

        static void ProcessRequest(HttpListenerContext context)
        {
            try
            {
                log.InfoFormat("Dispatching request '{0}'@{1}",
                    context.Request.Url.LocalPath, context.Request.RemoteEndPoint);
                RequestHandler handler;

                if (!RequestHandlers.Handlers.TryGetValue(context.Request.Url.LocalPath, out handler))
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Bad request";
                    using (StreamWriter wtr = new StreamWriter(context.Response.OutputStream))
                        wtr.Write("<h1>Bad request</h1>");
                }
                else
                    handler.HandleRequest(context);
            }
            catch (Exception e)
            {
                using (StreamWriter wtr = new StreamWriter(context.Response.OutputStream))
                    wtr.Write("<Error>Internal Server Error</Error>");
                log.Error("Error when dispatching request", e);
            }

            context.Response.Close();
        }
    }
}
