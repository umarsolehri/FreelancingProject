using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace FFYP
{
    [HubName("notificationHub")]
    public class NotificationHub : Hub
    {
        //notificationHub.client.notify
        public void Hello()
        {
            Clients.All.Notify();
        }
        [HubMethodName("notifyBid")]
        public static void NotifyBid()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            context.Clients.All.notifyBid();
        }
    }
}