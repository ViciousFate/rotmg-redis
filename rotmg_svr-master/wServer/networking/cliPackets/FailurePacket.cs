using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using common;

namespace wServer.networking.cliPackets
{
    public class FailurePacket : ClientPacket
    {
        public string Message { get; set; }

        public override PacketID ID { get { return PacketID.Failure; } }
        public override Packet CreateInstance() { return new FailurePacket(); }

        protected override void Read(NReader rdr)
        {
            Message = rdr.ReadUTF();
        }

        protected override void Write(NWriter wtr)
        {
            wtr.WriteUTF(Message);
        }
    }
}
