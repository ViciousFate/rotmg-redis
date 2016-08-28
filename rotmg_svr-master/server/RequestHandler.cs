using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using common;

namespace server
{
    abstract class RequestHandler
    {
        public abstract void HandleRequest(HttpListenerContext context);
        protected Database Database { get { return Program.Database; } }
        protected void Write(HttpListenerContext txt, string val)
        {
            var buff = Encoding.UTF8.GetBytes(val);
            txt.Response.OutputStream.Write(buff, 0, buff.Length);
        }
    }

    static class RequestHandlers
    {
        public static readonly Dictionary<string, RequestHandler> Handlers = new Dictionary<string, RequestHandler>()
        {
            { "/crossdomain.xml", new crossdomain() },
            { "/char/list", new @char.list() },
            { "/char/delete", new @char.delete() },
            { "/char/fame", new @char.fame() },
            { "/account/register", new account.register() },
            { "/account/verify", new account.verify() },
            { "/account/forgotPassword", new account.forgotPassword() },
            { "/account/sendVerifyEmail", new account.sendVerifyEmail() },
            { "/account/changePassword", new account.changePassword() },
            { "/account/purchaseCharSlot", new account.purchaseCharSlot() },
            { "/account/setName", new account.setName() },
            { "/credits/getoffers", new credits.getoffers() },
            { "/credits/add", new credits.add() },
            { "/fame/list", new fame.list() },
            { "/picture/get", new picture.get() },
        };
    }
}
