using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;

namespace wServer.realm.entities
{
    partial class Player
    {
        //TODO: persistence
        const int LOCKED_LIST_ID = 0;
        const int IGNORED_LIST_ID = 1;
        List<int> locked = new List<int>(6);
        List<int> ignored = new List<int>(6);
        
        void SendAccountList(List<int> list, int id)
        {
            client.SendPacket(new AccountListPacket()
            {
                AccountListId = id,
                AccountIds = list.ToArray()
            });
        }

        //public void EditAccountList(EditAccountListPacket pkt)
        //{
        //    List<int> list;
        //    if (pkt.AccountListId == LOCKED_LIST_ID)
        //        list = locked;
        //    else if (pkt.AccountListId == IGNORED_LIST_ID)
        //        list = ignored;
        //    else return;

        //    Player player = Owner.GetEntity(pkt.ObjectId) as Player;
        //    if (player == null) return;
        //    int accId = player.client.Account.AccountId;

        //    if (pkt.Add && list.Count < 6)
        //        list.Add(accId);
        //    else
        //        list.Remove(accId);

        //    SendAccountList(list, pkt.AccountListId);
        //}
    }
}
