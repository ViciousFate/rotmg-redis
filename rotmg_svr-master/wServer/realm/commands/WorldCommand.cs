using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using common;

namespace wServer.realm.commands
{
    class TutorialCommand : Command
    {
        public TutorialCommand() : base("tutorial") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.Client.Reconnect(new ReconnectPacket()
            {
                Host = "",
                Port = 2050,
                GameId = World.TUT_ID,
                Name = "Tutorial",
                Key = Empty<byte>.Array,
            });
            return true;
        }
    }

    class WhoCommand : Command
    {
        public WhoCommand() : base("who") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            StringBuilder sb = new StringBuilder("Players online: ");
            var copy = player.Owner.Players.Values.ToArray();
            if (copy.Length == 0)
                player.SendInfo("Nobody else is online");
            else
            {
                for (int i = 0; i < copy.Length; i++)
                {
                    if (i != 0) sb.Append(", ");
                    sb.Append(copy[i].Name);
                }

                player.SendInfo(sb.ToString());
            }
            return true;
        }
    }

    class ServerCommand : Command
    {
        public ServerCommand() : base("server") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            player.SendInfo(player.Owner.Name);
            return true;
        }
    }

    class PauseCommand : Command
    {
        public PauseCommand() : base("pause") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.HasConditionEffect(ConditionEffects.Paused))
            {
                player.ApplyConditionEffect(new ConditionEffect()
                {
                    Effect = ConditionEffectIndex.Paused,
                    DurationMS = 0
                });
                player.SendInfo("Game resumed.");
                return true;
            }
            else
            {
                if (player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>().Any())
                {
                    player.SendError("Not safe to pause.");
                    return false;
                }
                else
                {
                    player.ApplyConditionEffect(new ConditionEffect()
                    {
                        Effect = ConditionEffectIndex.Paused,
                        DurationMS = -1
                    });
                    player.SendInfo("Game paused.");
                    return true;
                }
            }
        }
    }

    /// <summary>
    /// This introduces a subtle bug, since the client UI is not notified when a /teleport is typed, it's cooldown does not reset.
    /// This leads to the unfortunate situation where the cooldown has been not been reached, but the UI doesn't know. The graphical TP will fail
    /// and cause it's timer to reset. NB: typing /teleport will workaround this timeout issue.
    /// </summary>
    class TeleportCommand : Command
    {
        public TeleportCommand() : base("teleport") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (player.Name.EqualsIgnoreCase(args))
            {
                player.SendInfo("You are already at yourself, and always will be!");
                return false;
            }

            foreach (var i in player.Owner.Players)
            {
                if (i.Value.Name.EqualsIgnoreCase(args))
                {
                    player.Teleport(time, i.Value.Id);
                    return true;
                }
            }
            player.SendInfo(string.Format("Unable to find player: {0}", args));
            return false;
        }
    }

    class TellCommand : Command
    {
        public TellCommand() : base("tell") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }
            int index = args.IndexOf(' ');
            if (index == -1)
            {
                player.SendError("Usage: /tell <player name> <text>");
                return false;
            }

            string playername = args.Substring(0, index);
            string msg = args.Substring(index + 1);

            if (player.Name.ToLower() == playername.ToLower())
            {
                player.SendInfo("Quit telling yourself!");
                return false;
            }

            if (!player.Manager.Chat.Tell(player, playername, msg))
                player.SendError(string.Format("{0} not found.", playername));
            return false;
        }
    }

    class HelpCommand : Command
    {
        //actually the command is 'help', but /help is intercepted by client
        public HelpCommand() : base("commands") { }

        protected override bool Process(Player player, RealmTime time, string args)
        {
            StringBuilder sb = new StringBuilder("Available commands: ");
            var cmds = player.Manager.Commands.Commands.Values
                .Where(x => x.HasPermission(player))
                .ToArray();
            for (int i = 0; i < cmds.Length; i++)
            {
                if (i != 0) sb.Append(", ");
                sb.Append(cmds[i].CommandName);
            }

            player.SendInfo(sb.ToString());
            return true;
        }
    }
}
