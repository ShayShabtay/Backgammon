using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Server.Dal;

[assembly: OwinStartup(typeof(Server.Startup))]

namespace Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //using (var context = new Db())
            //{
            //    context.UserTable.Add(new User("shay", "123"));
            //    context.SaveChanges();
            //}
            app.MapSignalR();
        }
    }
}
