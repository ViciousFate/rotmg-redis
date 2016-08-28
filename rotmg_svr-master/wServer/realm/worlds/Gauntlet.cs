using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using wServer.networking;

namespace wServer.realm.worlds
{
    public class GauntletMap : World
    {
        public GauntletMap()
        {
            Id = GAUNTLET;
            Name = "The Gauntlet";
            Background = 0;
            AllowTeleport = false;
        }

        protected override void Init()
        {
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.gauntlet.wmap"));
        }

        public override World GetInstance(Client client)
        {
            return Manager.AddWorld(new GauntletMap());
        }
    }
}
