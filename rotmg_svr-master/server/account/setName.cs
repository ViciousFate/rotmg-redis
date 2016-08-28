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
    class setName : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            string name = query["name"];
            if (name.Length < 3 || name.Length > 15 || !name.All(x => char.IsLetter(x) || char.IsNumber(x)))
                Write(context, "<Error>Invalid name</Error>");
            else
            {
                string key = Database.NAME_LOCK;
                string lockToken = null;
                try
                {
                    while ((lockToken = Database.AcquireLock(key)) == null) ;

                    if (Database.Hashes.Exists(0, "names", name.ToUpperInvariant()).Exec())
                    {
                        Write(context, "<Error>Duplicated name</Error>");
                        return;
                    }

                    DbAccount acc;
                    var status = Database.Verify(query["guid"], query["password"], out acc);
                    if (status == LoginStatus.OK)
                    {
                        using (var l = Database.Lock(acc))
                            if (Database.LockOk(l))
                            {
                                if (acc.NameChosen && acc.Credits < 1000)
                                    Write(context, "<Error>Not enough credits</Error>");
                                else
                                {
                                    if (acc.NameChosen)
                                        Database.UpdateCredit(acc, -1000);
                                    while (!Database.RenameIGN(acc, name, lockToken)) ;
                                        Write(context, "<Success />");
                                }
                            }
                            else
                                Write(context, "<Error>Account in Use</Error>");
                    }
                    else
                        Write(context, "<Error>" + status.GetInfo() + "</Error>");
                }
                finally
                {
                    if (lockToken != null)
                        Database.ReleaseLock(key, lockToken);
                }
            }
        }
    }
}
