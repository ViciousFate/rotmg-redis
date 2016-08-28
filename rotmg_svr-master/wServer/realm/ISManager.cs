using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using common;
using System.Collections.Concurrent;
using log4net;

namespace wServer.realm
{
    public class ISManager : InterServerChannel, IDisposable
    {
        ILog log = LogManager.GetLogger(typeof(ISManager));

        public const string NETWORK = "network";
        public const string CHAT = "chat";
        public const string CONTROL = "control";   //maybe later...

        enum NetworkCode
        {
            JOIN,
            PING,
            QUIT
        }
        struct NetworkMsg
        {
            public NetworkCode Code;
            public string Type;
        }

        RealmManager manager;
        public ISManager(RealmManager manager)
            : base(manager.Database, manager.InstanceId)
        {
            log.InfoFormat("Server's Id is {0}", manager.InstanceId);
            this.manager = manager;

            AddHandler<NetworkMsg>(NETWORK, HandleNetwork);

            Publish(NETWORK, new NetworkMsg()
            {
                Code = NetworkCode.JOIN,
                Type = "World Server"
            });
        }

        ConcurrentDictionary<string, int> availableInstance = new ConcurrentDictionary<string, int>();

        long remaining = 2000;
        public void Tick(RealmTime t)
        {
            remaining -= t.thisTickTimes;
            if (remaining < 0)
            {
                Publish(NETWORK, new NetworkMsg() { Code = NetworkCode.PING });
                remaining = 2000;

                foreach (var i in availableInstance.Keys)
                {
                    if (availableInstance.ContainsKey(i) && --availableInstance[i] == 0)
                    {
                        int val;
                        availableInstance.TryRemove(i, out val);
                        //race condition may occur, but dc for 10 sec...well let it be
                        log.InfoFormat("Server '{0}' timed out.", i);
                    }
                }
            }
        }

        public void Dispose()
        {
            Publish(NETWORK, new NetworkMsg() { Code = NetworkCode.QUIT });
        }

        void HandleNetwork(object sender, InterServerEventArgs<NetworkMsg> e)
        {
            switch (e.Content.Code)
            {
                case NetworkCode.JOIN:
                    if (availableInstance.TryAdd(e.InstanceId, 5))
                    {
                        log.InfoFormat("Server '{0}' ({1}) joined the network.",
                            e.InstanceId, e.Content.Type);
                        Publish(NETWORK, new NetworkMsg()   //for the new instances
                        {
                            Code = NetworkCode.JOIN,
                            Type = "World Server"
                        });
                    }
                    else
                        availableInstance[e.InstanceId] = 5;
                    break;
                case NetworkCode.PING:
                    if (!availableInstance.ContainsKey(e.InstanceId))
                        log.InfoFormat("Server '{0}' re-joined the network.", e.InstanceId);
                    availableInstance[e.InstanceId] = 5;
                    break;
                case NetworkCode.QUIT:
                    int dummy;
                    availableInstance.TryRemove(e.InstanceId, out dummy);
                    log.InfoFormat("Server '{0}' quited the network.", e.InstanceId);
                    break;
            }
        }
    }
}
