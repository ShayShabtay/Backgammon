using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class UserForContact
    {
        public string UserName { get; set; }
        public UserState State { get; set; }
        public string ConnectionID { get; set; }
    }
}
