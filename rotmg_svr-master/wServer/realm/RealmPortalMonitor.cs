using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.worlds;
using wServer.realm.entities;
using wServer.realm.terrain;
using log4net;

namespace wServer.realm
{
    public class RealmPortalMonitor
    {
        static ILog log = LogManager.GetLogger(typeof(RealmPortalMonitor));

        Nexus nexus;
        RealmManager manager;
        object worldLock = new object();
        Dictionary<World, Portal> portals = new Dictionary<World, Portal>();

        public RealmPortalMonitor(RealmManager manager)
        {
            log.Info("Initalizing Portal Monitor...");
            this.manager = manager;
            this.nexus = manager.Worlds[World.NEXUS_ID] as Nexus;
            lock (worldLock)
                foreach (var i in manager.Worlds)
                {
                    if (i.Value is GameWorld)
                        WorldAdded(i.Value);
                }
            log.Info("Portal Monitor initialized.");
        }

        Random rand = new Random();
        Position GetRandPosition()
        {
            int x, y;
            do
            {
                x = rand.Next(0, nexus.Map.Width);
                y = rand.Next(0, nexus.Map.Height);
            } while (
                portals.Values.Any(_ => _.X == x && _.Y == y) ||
                nexus.Map[x, y].Region != TileRegion.Realm_Portals);
            return new Position() { X = x, Y = y };
        }

        public void WorldAdded(World world)
        {
            lock (worldLock)
            {
                var pos = GetRandPosition();
                var portal = new Portal(manager, 0x0712, null)
                {
                    Size = 80,
                    WorldInstance = world,
                    Name = world.Name
                };
                portal.Move(pos.X + 0.5f, pos.Y + 0.5f);
                nexus.EnterWorld(portal);
                portals.Add(world, portal);
                log.InfoFormat("World {0}({1}) added.", world.Id, world.Name);
            }
        }
        public void WorldRemoved(World world)
        {
            lock (worldLock)
            {
                var portal = portals[world];
                nexus.LeaveWorld(portal);
                portals.Remove(world);
                log.InfoFormat("World {0}({1}) removed.", world.Id, world.Name);
            }
        }
        public void WorldClosed(World world)
        {
            lock (worldLock)
            {
                var portal = portals[world];
                nexus.LeaveWorld(portal);
                portals.Remove(world);
                log.InfoFormat("World {0}({1}) closed.", world.Id, world.Name);
            }
        }
        public void WorldOpened(World world)
        {
            lock (worldLock)
            {
                var pos = GetRandPosition();
                var portal = new Portal(manager, 0x71c, null)
                {
                    Size = 150,
                    WorldInstance = world,
                    Name = world.Name
                };
                portal.Move(pos.X, pos.Y);
                nexus.EnterWorld(portal);
                portals.Add(world, portal);
                log.InfoFormat("World {0}({1}) opened.", world.Id, world.Name);
            }
        }

        public World GetRandomRealm()
        {
            lock (worldLock)
            {
                var worlds = portals.Keys.ToArray();
                if (worlds.Length == 0)
                    return manager.Worlds[World.NEXUS_ID];
                else
                    return worlds[Environment.TickCount % worlds.Length];
            }
        }
    }
}
