using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading;
using log4net;

namespace wServer.realm
{
    public class LogicTicker
    {
        static ILog log = LogManager.GetLogger(typeof(LogicTicker));

        public int TPS;
        public int MsPT;

        public RealmManager Manager { get; private set; }
        public LogicTicker(RealmManager manager)
        {
            this.Manager = manager;
            pendings = new ConcurrentQueue<Action<RealmTime>>[5];
            for (int i = 0; i < 5; i++)
                pendings[i] = new ConcurrentQueue<Action<RealmTime>>();

            TPS = manager.TPS;
            MsPT = 1000 / TPS;
        }

        public void AddPendingAction(Action<RealmTime> callback)
        {
            AddPendingAction(callback, PendingPriority.Normal);
        }
        public void AddPendingAction(Action<RealmTime> callback, PendingPriority priority)
        {
            pendings[(int)priority].Enqueue(callback);
        }
        readonly ConcurrentQueue<Action<RealmTime>>[] pendings;


        public static RealmTime CurrentTime;
        public void TickLoop()
        {
            log.Info("Logic loop started.");
            Stopwatch watch = new Stopwatch();
            long dt = 0;
            long count = 0;

            watch.Start();
            RealmTime t = new RealmTime();
            long xa = 0;
            do
            {
                if (Manager.Terminating) break;

                long times = dt / MsPT;
                dt -= times * MsPT;
                times++;

                long b = watch.ElapsedMilliseconds;

                count += times;
                if (times > 3)
                    log.Warn("LAGGED!| time:" + times + " dt:" + dt + " count:" + count + " time:" + b + " tps:" + count / (b / 1000.0));

                t.tickTimes = b;
                t.tickCount = count;
                t.thisTickCounts = (int)times;
                t.thisTickTimes = (int)(times * MsPT);
                xa += t.thisTickTimes;

                foreach (var i in pendings)
                {
                    Action<RealmTime> callback;
                    while (i.TryDequeue(out callback))
                    {
                        try
                        {
                            callback(t);
                        }
                        catch { }
                    }
                }
                TickWorlds1(t);
                Manager.InterServer.Tick(t);

                Thread.Sleep(MsPT);
                dt += Math.Max(0, watch.ElapsedMilliseconds - b - MsPT);

            } while (true);
            log.Info("Logic loop stopped.");
        }

        void TickWorlds1(RealmTime t)    //Continous simulation
        {
            CurrentTime = t;
            foreach (var i in Manager.Worlds.Values.Distinct())
                i.Tick(t);
            //if (EnableMonitor)
            //    svrMonitor.Mon.Tick(t);
        }

     /*   void TickWorlds2(RealmTime t)    //Discrete simulation
        {
            long counter = t.thisTickTimes;
            long c = t.tickCount - t.thisTickCounts;
            long x = t.tickTimes - t.thisTickTimes;
            while (counter >= MsPT)
            {
                c++; x += MsPT;
                TickWorlds1(new RealmTime()
                {
                    thisTickCounts = 1,
                    thisTickTimes = MsPT,
                    tickCount = c,
                    tickTimes = x
                });
                counter -= MsPT;
            }
        }*/
    }
}
