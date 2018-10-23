
using Server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Server.Dal
{
    public class Db : DbContext
    {

        public DbSet<User> UserTable { get; set; }

        public Db()
        {
          
        }
    }
}