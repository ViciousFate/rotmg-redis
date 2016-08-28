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

namespace server.fame
{
    class list : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            DbChar character = null;
            if (query["accountId"] != null)
            {
                character = Database.LoadCharacter(
                    int.Parse(query["accountId"]),
                    int.Parse(query["charId"])
                );
            }
            FameList list = FameList.FromDb(Database, query["timespan"], character);
            Write(context, list.ToXml().ToString());
        }
    }
}
