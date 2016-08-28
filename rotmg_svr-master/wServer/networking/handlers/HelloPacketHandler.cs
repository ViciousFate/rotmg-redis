using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.cliPackets;
using wServer.realm;
using common;
using wServer.networking.svrPackets;

namespace wServer.networking.handlers
{
    class HelloPacketHandler : PacketHandlerBase<HelloPacket>
    {
        public override PacketID ID { get { return PacketID.Hello; } }

        protected override void HandlePacket(Client client, HelloPacket packet)
        {
            DbAccount acc;
            var s1 = client.Manager.Database.Verify(packet.GUID, packet.Password, out acc);
            if (s1 == LoginStatus.AccountNotExists)
            {
                var s2 = client.Manager.Database.Register(packet.GUID, packet.Password, true, out acc);
                if (s2 != RegisterStatus.OK)
                {
                    SendFailure(client, "Bad Login");
                    client.Disconnect();
                    return;
                }
            }
            else if (s1 == LoginStatus.InvalidCredentials)
                SendFailure(client, "Invalid Credentials");

            if (!client.Manager.TryConnect(client))
            {
                client.Account = null;
                SendFailure(client, "Failed to connect");
                client.Disconnect();
                return;
            }

            if (!client.Manager.Database.AcquireLock(acc))
            {
                SendFailure(client, "Account in Use (" + 
                    client.Manager.Database.GetLockTime(acc) + " seconds until timeout)");
                client.Disconnect();
                return;
            }

            World world = client.Manager.GetWorld(packet.GameId);
            if (world == null)
            {
                SendFailure(client, "Invalid world");
                client.Disconnect();
                return;
            }

            if (world.Id == -6) //Test World
                (world as realm.worlds.Test).LoadJson(packet.MapInfo);
            else if (world.IsLimbo)
                world = world.GetInstance(client);

            client.Account = acc;

            var seed = (uint)((long)Environment.TickCount * packet.GUID.GetHashCode()) % uint.MaxValue;
            client.Random = new wRandom(seed);
            client.targetWorld = world.Id;
            client.SendPacket(new MapInfoPacket()
            {
                Width = world.Map.Width,
                Height = world.Map.Height,
                Name = world.Name,
                Seed = seed,
                Background = world.Background,
                AllowTeleport = world.AllowTeleport,
                ShowDisplays = world.ShowDisplays,
                ClientXML = client.Manager.GameData.AdditionXml,
                ExtraXML = world.ExtraXML
            });
            client.Stage = ProtocalStage.Handshaked;

        }

    }
}
