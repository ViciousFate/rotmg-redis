using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.cliPackets;
using wServer.realm;
using common;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using wServer.realm.worlds;

namespace wServer.networking.handlers
{
    class UsePortalPacketHandler : PacketHandlerBase<UsePortalPacket>
    {
        public override PacketID ID { get { return PacketID.UsePortal; } }

        protected override void HandlePacket(Client client, UsePortalPacket packet)
        {
            client.Manager.Logic.AddPendingAction(t => Handle(client.Player, packet));
        }

        void Handle(Player player, UsePortalPacket packet)
        {
            Portal entity = player.Owner.GetEntity(packet.ObjectId) as Portal;
            if (entity == null || !entity.Usable) return;
            World world = entity.WorldInstance;
            if (world == null)
            {
                switch (entity.ObjectType) //handling default case for not found. Add more as implemented
                {
                    case 0x0703: //portal of cowardice
                        {
                            if (player.Manager.PlayerWorldMapping.ContainsKey(player.AccountId))  //may not be valid, realm recycled?
                                world = player.Manager.PlayerWorldMapping[player.AccountId];  //also reconnecting to vault is a little unexpected
                            else
                                world = player.Manager.GetWorld(World.NEXUS_ID);
                        } break;
                    case 0x0712:
                        world = player.Manager.GetWorld(World.VAULT_ID); break;
                    case 0x071d:
                        world = player.Manager.GetWorld(World.NEXUS_ID); break;
                    case 0x071c:
                        world = player.Manager.Monitor.GetRandomRealm(); break;
                    case 0x0720:
                        world = player.Manager.GetWorld(World.VAULT_ID); break;
                    case 0x071e:
                        world = player.Manager.AddWorld(new Kitchen()); break;
                    case 0x071f: //these need to match IDs
                        //world = RealmManager.GetWorld(World.GauntletMap); break; //this creates a singleton dungeon
                        world = player.Manager.AddWorld(new GauntletMap()); break; //this allows each dungeon to be unique
                    default: player.SendError("Portal Not Implemented!"); break;
                    //case 1795
                    /*case 0x0712:
                        world = RealmManager.GetWorld(World.NEXUS_ID); break;*/
                }

                entity.WorldInstance = world;
            }

            //used to match up player to last realm they were in, to return them to it. Sometimes is odd, like from Vault back to Vault...
            if (player.Manager.PlayerWorldMapping.ContainsKey(player.AccountId))
            {
                World tempWorld;
                player.Manager.PlayerWorldMapping.TryRemove(player.AccountId, out tempWorld);
            }
            player.Manager.PlayerWorldMapping.TryAdd(player.AccountId, player.Owner);
            player.Client.Reconnect(new ReconnectPacket()
            {
                Host = "",
                Port = 2050,
                GameId = world.Id,
                Name = world.Name,
                Key = Empty<byte>.Array,
            });
        }
    }
}
