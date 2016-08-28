using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace server.account
{
    class forgotPassword : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            Write(context, "<Error>Nope.</Error>");
        }
    }
}
