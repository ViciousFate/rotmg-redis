using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm.setpieces;
using wServer.realm.commands;

namespace wServer.realm.entities
{
    partial class Player
    {
        public void SendInfo(string text)
        {
            client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "",
                Text = text
            });
        }
        public void SendError(string text)
        {
            client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "*Error*",
                Text = text
            });
        }
        public void SendClientText(string text)
        {
            client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "*Client*",
                Text = text
            });
        }
        public void SendHelp(string text)
        {
            client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "*Help*",
                Text = text
            });
        }
        public void SendEnemy(string name, string text)
        {
            client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = "#" + name,
                Text = text
            });
        }
        public void SendText(string sender, string text)
        {
            client.SendPacket(new TextPacket()
            {
                BubbleTime = 0,
                Stars = -1,
                Name = sender,
                Text = text
            });
        }

        internal void TellReceived(int objId, int stars, string from, string to, string text)
        {
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 10,
                Stars = stars,
                Name = from,
                Recipient = to,
                Text = text
            });
        }

        internal void AnnouncementReceived(string text)
        {
            client.Player.SendText("@Announcement", text);
        }

        internal void GuildReceived(int objId, int stars, string from, string text)
        {
            //untested
            Client.SendPacket(new TextPacket()
            {
                BubbleTime = 10,
                Stars = stars,
                Name = "*Guild*",
                Recipient = from,
                Text = text
            });
        }
    }
}
