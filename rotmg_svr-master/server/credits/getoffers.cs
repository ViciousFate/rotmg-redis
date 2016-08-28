using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace server.credits
{
    class getoffers : RequestHandler
    {
        public override void HandleRequest(HttpListenerContext context)
        {
            Write(context,
"<Offers><Tok>WUT</Tok><Exp>STH</Exp><Offer><Id>0</Id><Price>0</Price><RealmGold>1000</RealmGold><CheckoutJWT>1000</CheckoutJWT><Data>YO</Data><Currency>HKD</Currency></Offer></Offers>");
        }
    }
}