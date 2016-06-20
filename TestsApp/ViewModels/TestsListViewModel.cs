using Latkowska.Tests.DAO;
using Latkowska.Testy.Interfaces;
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

using Latkowska.Tests.DAO.BO;
using System.Data.SQLite;
using TestsApp.ViewModels;

namespace Latkowska.Testy.ViewModels
{
    class TestsListViewModel : BaseViewModel, INotifyPropertyChanged
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

        private RelayCommand _addTestCommand;
        private RelayCommand _editTestCommand;
        private RelayCommand _eraseTestCommand;
        private RelayCommand _showStatisticsCommand;

        public Action<TestViewModel> SwitchToNewUserControlToEditTest { get; set; }

        public ICommand AddTestCommand
        {
            get { return _addTestCommand; }
        }

        public ICommand EditTestCommand
        {
            get { return _editTestCommand; }
        }

        public ICommand EraseTestCommand
        {
            get { return _eraseTestCommand; }
        }

        public ICommand ShowStatisticsCommand
        {
            get { return _showStatisticsCommand; }
        }

        public TestsListViewModel() : this(null) {}

        public TestsListViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            _tests = new ObservableCollection<TestViewModel>();
            _dao = Parent.DAO;
            _view = (ListCollectionView)CollectionViewSource.GetDefaultView(_tests);

            GetAllTests();

            _addTestCommand = new RelayCommand((param) => this.AddTest());
            _editTestCommand = new RelayCommand((param) => this.EditTest());
            _eraseTestCommand = new RelayCommand((param) => this.EraseTest());
            _showStatisticsCommand = new RelayCommand((param) => this.ShowStatistics());
        }

        private void AddTest()
        {
            Trace.WriteLine("Dodawanie testu.");
            Parent.CurrentView = new AddTestViewModel(Parent, new TestViewModel(null));
        }

        private void EditTest()
        {
            if (_selectedTest == null) return;
            Trace.WriteLine("Edytowanie testu.");
            Parent.CurrentView = new AddTestViewModel(Parent, SelectedTest);
            
        }

        private void EraseTest()
        {
            if (_selectedTest != null)
            {                
                foreach (QuestionViewModel question in _selectedTest.Questions)
                {
                    foreach (AnswerViewModel answer in question.Answers)
                        Parent.DAO.DeleteRecordFromDB("Answer", answer.Answer);
                    Parent.DAO.DeleteRecordFromDB("Question", question.Question);
                    
                }
                Parent.DAO.DeleteRecordFromDB("Test", _selectedTest.Test);
                Tests.Remove(SelectedTest);
            }
        }

        private void ShowStatistics()
        {
            Trace.WriteLine("Statystyki testu.");
        }

        private void GetAllTests()
        {
            foreach (var test in _dao.GetAllTests())
            {
                _tests.Add(new TestViewModel(test));
            }
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
