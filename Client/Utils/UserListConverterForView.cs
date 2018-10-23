using Client.Models;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utils
{
    class UserListConverterForView
    {

        public static ObservableCollection<UserForContact> ConvertUser(Dictionary<string, UserState> users)
        {
            ObservableCollection<UserForContact> tmp = new ObservableCollection<UserForContact>();
            
            foreach (var item in users)
            {
                UserForContact tmpUser = new UserForContact();
                tmpUser.UserName = item.Key;
                tmpUser.State = item.Value;
                tmp.Add(tmpUser);               
            }

            return tmp;
        }
    }
}
