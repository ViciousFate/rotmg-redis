using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using common;

namespace wServer.networking.cliPackets
{
    public class CancelTradePacket : ClientPacket
    {
        public override PacketID ID { get { return PacketID.CancelTrade; } }
        public override Packet CreateInstance() { return new CancelTradePacket(); }

        protected override void Read(NReader rdr) { }

        protected override void Write(NWriter wtr) { }
    }
}
