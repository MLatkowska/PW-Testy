using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.SQLite;
using Latkowska.Tests.DAO;
using Latkowska.Tests.DAO.BO;
using Latkowska.Testy.Interfaces;
using Latkowska.Testy.ViewModels;

namespace TestsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IUser user)
        {
            InitializeComponent();
            //DAO dao = new DAO();
            MainViewModel vm = new MainViewModel(user);
            DataContext = vm;
            vm.LogOutAndSwitchToLoginWindowAction = new Action(() => { LoginWindow login = new LoginWindow(); login.Show(); this.Close(); });
        }

        public MainWindow() : this(new User(0, "admin", "admin", true)) { }
    }
}

