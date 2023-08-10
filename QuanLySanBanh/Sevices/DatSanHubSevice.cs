using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLySanBanh.SignalRSevices
{
    [HubName("datSan")]
    public class DatSanHubSevice : Hub
    {
        public void SendData(object message)
        {
            Clients.All.sendData(message);
        }
    }
}