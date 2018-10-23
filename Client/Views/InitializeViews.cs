using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Client.Views
{
    public class InitializeViews
    {
        public static Page ChatViewPage { get; set; }
        public static Page ChatListViewPage { get; set; }
        public static Page RegisterViewPage { get; set; }

         static InitializeViews()
        {
            ChatViewPage = new ChatView();
            ChatListViewPage = new ChatListView();
            RegisterViewPage = new RegisterView();
        }
    }
}
