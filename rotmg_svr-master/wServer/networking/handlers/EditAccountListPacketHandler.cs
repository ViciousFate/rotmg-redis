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
    class EditAccountListPacketHandler : PacketHandlerBase<EditAccountListPacket>
    {
        public override PacketID ID { get { return PacketID.EditAccountList; } }

        protected override void HandlePacket(Client client, EditAccountListPacket packet)
        {
            //TODO: implement something
        }
    }
}
