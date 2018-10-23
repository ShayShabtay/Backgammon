using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.BL
{
    class InitializeProxy
    {
        public IHubProxy Proxy { get; set; }
        public HubConnection Connection { get; set; }

        private static InitializeProxy _instance;

        public static InitializeProxy Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InitializeProxy();
                }
                return _instance;
            }
        }

        private InitializeProxy()
        {
            Connection = new HubConnection("http://localhost:50206/");
            Proxy = Connection.CreateHubProxy("MainHub");
        }
    }
}
