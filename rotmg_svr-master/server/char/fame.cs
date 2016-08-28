using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using common;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml;

namespace server.@char
{
    class fame : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            DbChar character = Database.LoadCharacter(int.Parse(query["accountId"]), int.Parse(query["charId"]));
            if (character == null)
            {
                Write(context, "<Error>Invalid character</Error>");
                return;
            }

            Fame fame = Fame.FromDb(character);
            if (fame == null)
            {
                Write(context, "<Error>Character not dead</Error>");
                return;
            }
            Write(context, fame.ToXml().ToString());
        }
    }
}
