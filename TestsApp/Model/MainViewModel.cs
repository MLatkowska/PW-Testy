using Latkowska.Testy.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestsApp.Properties;

namespace Latkowska.Testy.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private BaseViewModel _currentView;
        public BaseViewModel CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }

        public IUser User { get; set; }
        public ITest CurrentTest { get; set; }

        private RelayCommand _logoutCommand;
        public Action LogOutAndSwitchToLoginWindowAction { get; set; }

        private Assembly _assembly;
        private IDAO _dao;
        public IDAO DAO
        {
            get { return _dao; }
        }

        public MainViewModel() : this(null) { }

        public MainViewModel(IUser user)
        {
            Settings settings = Settings.Default;

            string DAOpath = settings.DAO;

            Trace.WriteLine(DAOpath);
            _assembly = Assembly.UnsafeLoadFrom(@DAOpath);
            foreach (Type type in _assembly.GetTypes())
            {
                try
                {
                    if (Activator.CreateInstance(type) is IDAO)
                    {
                        _dao = (IDAO)Activator.CreateInstance(type);
                        Trace.WriteLine("Utworzono DAO");
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            _dao.GetAllTests();

            User = user;
            if(user.Editor)
                CurrentView = new TestsListViewModel(this);
            else
                CurrentView = new UserTestsListViewModel(this);
            _logoutCommand = new RelayCommand(param => this.Logout());
        }

        public ICommand LogoutCommand
        {
            get { return _logoutCommand; }
        }

        private void Logout()
        {
            LogOutAndSwitchToLoginWindowAction();
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
