using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using common;
using System.Xml.Serialization;
using System.IO;
using System.Web;
using System.Collections.Specialized;

namespace server.@char
{
    class list : RequestHandler
    {
        Lazy<List<ServerItem>> svrList;
        public list()
        {
            svrList = new Lazy<List<ServerItem>>(GetServerList, true);
        }
        List<ServerItem> GetServerList()
        {
            var ret = new List<ServerItem>();
            int num = Program.Settings.GetValue<int>("svrNum");
            for (int i = 0; i < num; i++)
                ret.Add(new ServerItem()
                {
                    Name = Program.Settings.GetValue("svr" + i + "Name"),
                    Lat = Program.Settings.GetValue<int>("svr" + i + "Lat", "0"),
                    Long = Program.Settings.GetValue<int>("svr" + i + "Long", "0"),
                    DNS = Program.Settings.GetValue("svr" + i + "Adr", "127.0.0.1"),
                    Usage = 0.2,
                    AdminOnly = Program.Settings.GetValue<bool>("svr" + i + "Admin", "false")
                });
            return ret;
        }

        public override void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            DbAccount acc;
            var status = Database.Verify(query["guid"], query["password"], out acc);
            if (status == LoginStatus.OK||status== LoginStatus.AccountNotExists)
            {
                if (status == LoginStatus.AccountNotExists)
                    acc = Database.CreateGuestAccount(query["guid"]);
                var list = CharList.FromDb(Database, acc);
                list.Servers = GetServerList();
                Write(context, list.ToXml().ToString());
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
