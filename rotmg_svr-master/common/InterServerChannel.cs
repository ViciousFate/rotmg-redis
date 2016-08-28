using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookSleeve;
using Newtonsoft.Json;

namespace common
{
    public class InterServerEventArgs<T> : EventArgs
    {
        public InterServerEventArgs(string instId, T val)
        {
            InstanceId = instId;
            Content = val;
        }
        public string InstanceId { get; private set; }
        public T Content { get; private set; }
    }

    public class InterServerChannel
    {
        public string InstanceId { get; private set; }
        public Database Database { get; private set; }

        RedisSubscriberConnection conn;
        public InterServerChannel(Database db, string instId)
        {
            Database = db;
            conn = db.GetOpenSubscriberChannel();
            InstanceId = instId;
        }

        struct Message<T> where T : struct
        {
            public string InstId;
            public string TargetInst;
            public T Content;
        }

        public void Publish<T>(string channel, T val, string target = null) where T : struct
        {
            Message<T> message = new Message<T>()
            {
                InstId = InstanceId,
                TargetInst = target,
                Content = val
            };
            Database.Publish(channel, JsonConvert.SerializeObject(message));
        }

        public void AddHandler<T>(string channel, EventHandler<InterServerEventArgs<T>> handler) where T : struct
        {
            conn.Subscribe(channel, (s, buff) =>
            {
                Message<T> message = JsonConvert.DeserializeObject<Message<T>>(
                    Encoding.UTF8.GetString(buff));
                if (message.TargetInst != null &&
                    message.TargetInst != InstanceId)
                    return;
                handler(this, new InterServerEventArgs<T>(message.InstId, message.Content));
            });
        }
    }
}
