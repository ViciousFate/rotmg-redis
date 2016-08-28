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
    class ChooseNamePacketHandler : PacketHandlerBase<ChooseNamePacket>
    {
        public override PacketID ID { get { return PacketID.ChooseName; } }

        protected override void HandlePacket(Client client, ChooseNamePacket packet)
        {
            string name = packet.Name;
            if (name.Length < 3 || name.Length > 15 || !name.All(x => char.IsLetter(x) || char.IsNumber(x)))
                client.SendPacket(new NameResultPacket()
                {
                    Success = false,
                    Message = "Invalid name"
                });
            else
            {
                string key = Database.NAME_LOCK;
                string lockToken = null;
                try
                {
                    while ((lockToken = client.Manager.Database.AcquireLock(key)) == null) ;

                    if (client.Manager.Database.Hashes.Exists(0, "names", name.ToUpperInvariant()).Exec())
                    {
                        client.SendPacket(new NameResultPacket()
                        {
                            Success = false,
                            Message = "Duplicated name"
                        });
                        return;
                    }

                    if (client.Account.NameChosen && client.Account.Credits < 1000)
                        client.SendPacket(new NameResultPacket()
                        {
                            Success = false,
                            Message = "Not enough credits"
                        });
                    else
                    {
                        if (client.Account.NameChosen)
                            client.Manager.Database.UpdateCredit(client.Account, -1000);
                        while (!client.Manager.Database.RenameIGN(client.Account, name, lockToken)) ;
                        client.Player.Name = client.Account.Name;
                        client.Player.UpdateCount++;
                        client.SendPacket(new NameResultPacket()
                        {
                            Success = true,
                            Message = ""
                        });
                    }
                }
                finally
                {
                    if (lockToken != null)
                        client.Manager.Database.ReleaseLock(key, lockToken);
                }
            }
        }

        void Handle(Player player)
        {
            player.Credits = player.Client.Account.Credits;
            player.Name = player.Client.Account.Name;
            player.NameChosen = true;
            player.UpdateCount++;
        }
    }
}
