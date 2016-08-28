using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using common;
using System.Collections.Concurrent;
using log4net;
using System.Timers;

namespace server
{
    class ISManager : InterServerChannel, IDisposable
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

        public ISManager()
            : base(Program.Database, Program.InstanceId)
        {
            log.InfoFormat("Server's Id is {0}", Program.InstanceId);

            AddHandler<NetworkMsg>(NETWORK, HandleNetwork);
            AddHandler<Message>(CHAT, HandleChat);

            Publish(NETWORK, new NetworkMsg()
            {
                Code = NetworkCode.JOIN,
                Type = "Account Server"
            });
        }

        ConcurrentDictionary<string, int> availableInstance = new ConcurrentDictionary<string, int>();

        Timer tmr = new Timer(2000);
        public void Run()
        {
            tmr.Elapsed += (sender, e) => Tick();
            tmr.Start();
        }
        void Tick()
        {
            Publish(NETWORK, new NetworkMsg() { Code = NetworkCode.PING });

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

        public void Dispose()
        {
            tmr.Stop();
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
                            Type = "Account Server"
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

        //Chat

        const char TELL = 't';
        const char GUILD = 'g';
        const char ANNOUNCE = 'a';

        struct Message
        {
            public char Type;
            public string Inst;

            public int ObjId;
            public int Stars;
            public int From;

            public int To;
            public string Text;
        }

        void HandleChat(object sender, InterServerEventArgs<Message> e)
        {
            switch (e.Content.Type)
            {
                case TELL:
                    {
                        string from = Database.ResolveIgn(e.Content.From);
                        string to = Database.ResolveIgn(e.Content.To);
                        log.InfoFormat("<{0} -> {1}> {2}", from, to, e.Content.Text);
                    } break;
                case GUILD:
                    {
                        string from = Database.ResolveIgn(e.Content.From);
                        log.InfoFormat("<{0} -> Guild> {1}", from, e.Content.Text);
                    } break;
                case ANNOUNCE:
                    {
                        log.InfoFormat("<Announcement> {0}", e.Content.Text);
                    } break;
            }
        }
    }
}
