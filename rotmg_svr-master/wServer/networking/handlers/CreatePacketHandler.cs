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
    class CreatePacketHandler : PacketHandlerBase<CreatePacket>
    {
        public override PacketID ID { get { return PacketID.Create; } }

        protected override void HandlePacket(Client client, CreatePacket packet)
        {
            var db = client.Manager.Database;

            DbChar character;
            var status = client.Manager.Database.CreateCharacter(
                client.Manager.GameData, client.Account, packet.ObjectType, out character);

            if (status == CreateStatus.ReachCharLimit)
            {
                SendFailure(client, "Too many characters");
                client.Disconnect();
                return;
            }

            client.Character = character;

            var target = client.Manager.Worlds[client.targetWorld];
                client.SendPacket(new CreateSuccessPacket()
                {
                    CharacterID = client.Character.CharId,
                    ObjectID = target.EnterWorld(client.Player = new Player(client))
                });
            client.Stage = ProtocalStage.Ready;
        }
    }
}
