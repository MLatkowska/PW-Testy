using Latkowska.Testy.Interfaces;
using Latkowska.Testy.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Latkowska.Testy.ViewModels
{
    public class UserTestsListViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ListCollectionView _view;
        private ObservableCollection<TestViewModel> _tests;
        private TestViewModel _selectedTest;
        private IDAO _dao;

        public ObservableCollection<TestViewModel> Tests
        {
            get { return _tests; }
            set
            {
                _tests = value;
                RaisePropertyChanged("Tests");
            }
        }

        public TestViewModel SelectedTest
        {
            get { return _selectedTest; }
            set
            {
                _selectedTest = value;
            }
        }

        private RelayCommand _solveTestCommand;
        public ICommand SolveTestCommand
        {
            get { return _solveTestCommand; }
        }

        private RelayCommand _showStatisticsCommand;
        public ICommand ShowStatisticsCommand
        {
            get { return _showStatisticsCommand; }
        }

        public UserTestsListViewModel() : this(null) {}

        public UserTestsListViewModel(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
            _tests = new ObservableCollection<TestViewModel>();
            _dao = Parent.DAO;
            _view = (ListCollectionView)CollectionViewSource.GetDefaultView(_tests);

            GetAllTests();

            _solveTestCommand = new RelayCommand((param) => this.SolveTest());
            _showStatisticsCommand = new RelayCommand((param) => this.ShowStatistics());
        }

        private void GetAllTests()
        {
            foreach (var test in _dao.GetAllTests())
            {
                _tests.Add(new TestViewModel(test));
            }
        }

        private void SolveTest()
        {
            if (_selectedTest != null)
            {
                Trace.WriteLine("Rozwiązywanie testu.");
                Parent.CurrentView = new SolveQuestionViewModel(Parent, _selectedTest);
            }
        }

        private void ShowStatistics()
        {
            Trace.WriteLine("Statystyki dla testu.");
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
