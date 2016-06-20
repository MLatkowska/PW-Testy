using Latkowska.Testy.Interfaces;
using Latkowska.Testy.ViewModels;
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
using System.Windows.Shapes;

namespace TestsApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            LoginViewModel vm = new LoginViewModel(); 
            this.DataContext = vm; 
            if (vm.LogInAndSwitchToMainWindowAction == null)
                vm.LogInAndSwitchToMainWindowAction = new Action<IUser>((user) => { MainWindow main = new MainWindow(user); main.Show(); this.Close(); });
        }
    }
}
