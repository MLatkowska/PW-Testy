using Latkowska.Tests.DAO;
using Latkowska.Testy.Interfaces;
using Latkowska.Testy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Reflection;
using TestsApp.Properties;

namespace Latkowska.Testy.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        private List<IUser> _users;
        private RelayCommand _loginCommand;

        public Action<IUser> LogInAndSwitchToMainWindowAction { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        private bool _loginSucceded;
        public bool LoginSucceded
        {
            get { return _loginSucceded; }
            set
            {
                _loginSucceded = value;
                RaisePropertyChanged("LoginSucceded");
            }
        }

        private IDAO _dao;
        private Assembly _assembly;

        public LoginViewModel()
        {
            string DAOpath = Settings.Default.DAO;

            Trace.WriteLine(DAOpath);
            _assembly = Assembly.UnsafeLoadFrom(@DAOpath);
            foreach (Type type in _assembly.GetTypes())
            {
                try
                {
                    if (Activator.CreateInstance(type) is IDAO)
                    {
                        _dao = (IDAO)Activator.CreateInstance(type);
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            _users = (List<IUser>) _dao.GetAllUsers();
            _loginSucceded = false;

            _loginCommand = new RelayCommand(param => this.Log());
        }

        public ICommand LoginCommand
        {
            get { return _loginCommand; }
        }

        private bool Log()
        {
            if (_users.Exists(user => user.Login == Login))
            {
                IUser user = _users.Find(u => u.Login == Login);
                if (user.Password == Password)
                { 
                    LoginSucceded = false;
                    LogInAndSwitchToMainWindowAction(user);
                    return true;
                }
            }
            LoginSucceded = true;
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
