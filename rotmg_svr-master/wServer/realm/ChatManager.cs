using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using wServer.realm.entities;
using wServer.networking.svrPackets;
using Newtonsoft.Json;
using common;

namespace wServer.realm
{
    public class ChatManager
    {
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

        static ILog log = LogManager.GetLogger(typeof(ChatManager));

        RealmManager manager;
        public ChatManager(RealmManager manager)
        {
            this.manager = manager;
            manager.InterServer.AddHandler<Message>(ISManager.CHAT, HandleChat);
        }

        public void Say(Player src, string text)
        {
            src.Owner.BroadcastPacket(new TextPacket()
            {
                Name = (src.Client.Account.Admin ? "@" : "") + src.Name,
                ObjectId = src.Id,
                Stars = src.Stars,
                BubbleTime = 5,
                Recipient = "",
                Text = text,
                CleanText = text
            }, null);
            log.InfoFormat("[{0}({1})] <{2}> {3}", src.Owner.Name, src.Owner.Id, src.Name, text);
        }

        public void Announce(string text)
        {
            manager.InterServer.Publish(ISManager.CHAT, new Message()
            {
                Type = ANNOUNCE,
                Inst = manager.InstanceId,
                Text = text
            });
        }

        public void Oryx(World world, string text)
        {
            world.BroadcastPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "#Oryx the Mad God",
                Text = text
            }, null);
            log.InfoFormat("[{0}({1})] <Oryx the Mad God> {2}", world.Name, world.Id, text);
        }

        public bool Tell(Player src, string target, string text)
        {
            int id = manager.Database.ResolveId(target);
            if (id == 0) return false;

            int time = manager.Database.GetLockTime(id);
            if (time == -1) return false;

            manager.InterServer.Publish(ISManager.CHAT, new Message()
            {
                Type = TELL,
                Inst = manager.InstanceId,
                ObjId = src.Id,
                Stars = src.Stars,
                From = src.Client.Account.AccountId,
                To = id,
                Text = text
            });
            return true;
        }

        void HandleChat(object sender, InterServerEventArgs<Message> e)
        {
            switch (e.Content.Type)
            {
                case TELL:
                    {
                        string from = manager.Database.ResolveIgn(e.Content.From);
                        string to = manager.Database.ResolveIgn(e.Content.To);
                        foreach (var i in manager.Clients.Values
                            .Where(x => x.Player != null)
                            .Where(x => x.Account.AccountId == e.Content.From ||
                                        x.Account.AccountId == e.Content.To)
                            .Select(x => x.Player))
                        {
                            i.TellReceived(
                                e.Content.Inst == manager.InstanceId ? e.Content.ObjId : -1,
                                e.Content.Stars, from, to, e.Content.Text);
                        }
                    } break;
                case GUILD:
                    {
                        string from = manager.Database.ResolveIgn(e.Content.From);
                        foreach (var i in manager.Clients.Values
                            .Where(x => x.Player != null)
                            .Where(x => x.Account.GuildId == e.Content.To)
                            .Select(x => x.Player))
                        {
                            i.GuildReceived(
                                e.Content.Inst == manager.InstanceId ? e.Content.ObjId : -1,
                                e.Content.Stars, from, e.Content.Text);
                        }
                    } break;
                case ANNOUNCE:
                    {
                        foreach (var i in manager.Clients.Values
                            .Where(x => x.Player != null)
                            .Select(x => x.Player))
                        {
                            i.AnnouncementReceived(e.Content.Text);
                        }
                    } break;
            }
        }
    }
}
