using Latkowska.Testy.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestsApp.ViewModels;

namespace Latkowska.Testy.ViewModels
{
    public class SolveQuestionViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public TestViewModel TestVM { get; set; }
        public ITest Test { get; set; }

        private int _currentQuestionIndex;
        private QuestionViewModel _currentQuestion;
        public QuestionViewModel CurrentQuestion 
        {
            get { return _currentQuestion; }
            set
            {
                _currentQuestion = value;
                RaisePropertyChanged("CurrentQuestion");
            }     
        }

        private RelayCommand _nextQuestionCommand;
        public ICommand NextQuestionCommand
        {
            get { return _nextQuestionCommand; }
        }

        public SolveQuestionViewModel(MainViewModel mainVM, TestViewModel testVM) : base (mainVM)
        {
            TestVM = testVM;
            Test = testVM.Test;

            _currentQuestionIndex = 0;
            _currentQuestion = TestVM.Questions.First();

            _nextQuestionCommand = new RelayCommand((param) => this.NextQuestion());
        }

        private void NextQuestion()
        {
            _currentQuestionIndex++;
            if (_currentQuestionIndex < TestVM.Questions.Count)
            {
                Trace.WriteLine("Następne pytanie");
                CurrentQuestion = TestVM.Questions.ElementAt(_currentQuestionIndex);
            }
            else
            {
                Trace.WriteLine("Koniec pytań - koniec testu");
                //jak zdążę to najpierw okienko podumowujące test
                Parent.CurrentView = new UserTestsListViewModel(Parent);
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
