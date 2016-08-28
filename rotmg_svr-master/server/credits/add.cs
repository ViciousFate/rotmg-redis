using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using common;
using System.Web;

namespace server.credits
{
    class add : RequestHandler
    {
        //Obviously a place holder
        public override void HandleRequest(HttpListenerContext context)
        {
            var query = HttpUtility.ParseQueryString(context.Request.Url.Query);

            string ret;
            DbAccount acc = Database.GetAccount(query["guid"]);
            if (acc != null)
            {
                int amount = int.Parse(query["jwt"]);
                Database.UpdateCredit(acc, amount);
                ret = "Done ya!";
            }
            else
                ret = "Account not exists!";

            Write(context, @"<html>
    <head>
        <title>Ya...</title>
    </head>
    <body style='background: #333333'>
        <h1 style='color: #EEEEEE; text-align: center'>
            " + ret + @"
        </h1>
    </body>
</html>");
        }
    }
}