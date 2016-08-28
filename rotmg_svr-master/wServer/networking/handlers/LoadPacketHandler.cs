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
    class LoadPacketHandler : PacketHandlerBase<LoadPacket>
    {
        public override PacketID ID { get { return PacketID.Load; } }

        protected override void HandlePacket(Client client, LoadPacket packet)
        {
            client.Character = client.Manager.Database
                                .LoadCharacter(client.Account, packet.CharacterId);
            if (client.Character != null)
            {
                if (client.Character.Dead)
                {
                    SendFailure(client, "Character is dead");
                    client.Disconnect();
                }
                else
                {
                    var target = client.Manager.Worlds[client.targetWorld];
                        client.SendPacket(new CreateSuccessPacket()
                        {
                            CharacterID = client.Character.CharId,
                            ObjectID = target.EnterWorld(client.Player = new Player(client))
                        });
                    client.Stage = ProtocalStage.Ready;
                }
            }
            else
            {
                SendFailure(client, "Failed to load character");
                client.Disconnect();
            }
        }
    }
}
