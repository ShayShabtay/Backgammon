using Common.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.BL
{
    class ClientGameManager
    {
        private InitializeProxy _proxy = InitializeProxy.Instance;

        public static bool isMyTurn;

        public ClientGameManager()
        {
            //_proxy.Proxy.On

            _proxy.Connection.Start().Wait();
        }

        internal Cube RollCubes()
        {
                Task<Cube> task = Task.Run(async () =>
                {
                    return await _proxy.Proxy.Invoke<Cube>("RollCube", ClientUserManager.UserToChat);
                });

                return task.Result;
        }

    }
}
