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
using System.Globalization;

namespace server.account
{
    class register : RequestHandler
    {
        public bool IsValidEmail(string strIn)
        {
            var invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            MatchEvaluator DomainMapper = match =>
            {
                // IdnMapping class with default property values.
                IdnMapping idn = new IdnMapping();

                string domainName = match.Groups[2].Value;
                try
                {
                    domainName = idn.GetAscii(domainName);
                }
                catch (ArgumentException)
                {
                    invalid = true;
                }
                return match.Groups[1].Value + domainName;
            };

            // Use IdnMapping class to convert Unicode domain names. 
            strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper);
            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            return Regex.IsMatch(strIn,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                      RegexOptions.IgnoreCase);
        }

        public override void HandleRequest(HttpListenerContext context)
        {
            NameValueCollection query;
            using (StreamReader rdr = new StreamReader(context.Request.InputStream))
                query = HttpUtility.ParseQueryString(rdr.ReadToEnd());

            if (!IsValidEmail(query["newGUID"]))
                Write(context, "<Error>Invalid email</Error>");
            else
            {
                string key = Database.REG_LOCK;
                string lockToken = null;
                try
                {
                    while ((lockToken = Database.AcquireLock(key)) == null) ;

                    DbAccount acc;
                    var status = Database.Verify(query["guid"], "", out acc);
                    if (status == LoginStatus.OK)
                    {
                        //what? can register in game? kill the account lock
                        Database.RenameUUID(acc, query["newGUID"], lockToken);
                        Database.ChangePassword(acc.UUID, query["newPassword"]);
                        Write(context, "<Success />");
                    }
                    else
                    {
                        var s = Database.Register(query["newGUID"], query["newPassword"], false, out acc);
                        if (s == RegisterStatus.OK)
                            Write(context, "<Success />");
                        else
                            Write(context, "<Error>" + s.GetInfo() + "</Error>");
                    }
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
