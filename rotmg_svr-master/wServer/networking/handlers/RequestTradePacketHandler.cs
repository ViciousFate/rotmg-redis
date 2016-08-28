using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.cliPackets;
using wServer.realm;
using common;
using wServer.networking.svrPackets;
using wServer.realm.entities;

namespace wServer.networking.handlers
{
    class RequestTradePacketHandler : PacketHandlerBase<RequestTradePacket>
    {
        public override PacketID ID { get { return PacketID.RequestTrade; } }

        protected override void HandlePacket(Client client, RequestTradePacket packet)
        {
            client.Player.RequestTrade(packet.Name);
        }
    }
}
