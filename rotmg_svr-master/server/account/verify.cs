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

namespace server.account
{
    class verify : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            DbAccount acc;
            var status = Database.Verify(query["guid"], query["password"], out acc);
            if (status == LoginStatus.OK)
                Write(context, Account.FromDb(acc).ToXml().ToString());
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
