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
    class purchaseCharSlot : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            DbAccount acc;
            var status = Database.Verify(query["guid"], query["password"], out acc);
            if (status == LoginStatus.OK)
            {
                using (var l = Database.Lock(acc))
                {
                    if (!Database.LockOk(l))
                    {
                        Write(context, "<Error>Account in Use</Error>");
                        return;
                    }
                    else if (acc.Credits < 100)
                    {
                        Write(context, "<Error>Not enough credits</Error>");
                        return;
                    }
                    Database.UpdateCredit(acc, -100);
                    acc.MaxCharSlot++;
                    acc.Flush();
                }
                Write(context, "<Success />");
            }
            else
                Write(context, "<Error>" + status.GetInfo() + "</Error>");
        }
    }
}
