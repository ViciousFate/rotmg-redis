using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using common;

namespace wServer.networking.cliPackets
{
    public class TeleportPacket : ClientPacket
    {
        public int ObjectId { get; set; }

        public override PacketID ID { get { return PacketID.Teleport; } }
        public override Packet CreateInstance() { return new TeleportPacket(); }

        protected override void Read(NReader rdr)
        {
            ObjectId = rdr.ReadInt32();
        }
        protected override void Write(NWriter wtr)
        {
            wtr.Write(ObjectId);
        }
    }
}
