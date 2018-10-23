using Client.BL;
using Client.Models;
using Client.Utils;
using Client.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModels
{
    class RegisterViewModel : ViewModelBase
    {
        //Manager
        private ClientUserManager _userManager;

        //Properties
        public string Name { get; set; }
        public User User { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        private string _errorMessage;
        public string ErrorMessage {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        //Ctor
        public RegisterViewModel()
        {
            _userManager = new ClientUserManager();

            RegisterCommand = new RelayCommand(Registeration);
            LoginCommand = new RelayCommand(Login);
            User = new User();
        }

        //Functions
        private void Registeration()
        {
            if (User == null 
                || string.IsNullOrWhiteSpace(User.UserName) 
                || string.IsNullOrWhiteSpace(User.Password) )
            {
                ErrorMessage = "Please fill username and password";
            }
            else
            {
                if(_userManager.InvokeRegistration(User))
                Application.Current.MainWindow.Content = new ChatListView();
                else
                {
                    ErrorMessage = "username already exsists, please choose another one";
                }
            }
        }

        private void Login()
        {
            if (User == null 
                || string.IsNullOrWhiteSpace(User.UserName) 
                || string.IsNullOrWhiteSpace(User.Password) )
            {
                ErrorMessage = "Please fill username and password";
            }
            else
            {
                if(_userManager.InvokeLogin(User))
                Application.Current.MainWindow.Content = new ChatListView();
                else
                {
                    ErrorMessage = "username or password are incorrect, please try again";
                }
            }
        }
    }
}
