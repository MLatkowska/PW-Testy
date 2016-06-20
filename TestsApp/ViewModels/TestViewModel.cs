using Latkowska.Testy.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsApp.ViewModels;

namespace Latkowska.Testy.ViewModels
{
    public class TestViewModel : INotifyPropertyChanged
    {
        private ITest _test;

        public ITest Test 
        { 
            get { return _test; } 
        }

        public TestViewModel(ITest test)
        {
            _test = test;

            _questions = new ObservableCollection<QuestionViewModel>();
            if(test != null)
                foreach (var q in test.Questions)
                    _questions.Add(new QuestionViewModel(q));
        }

        public int TestID
        {
            get { return _test.TestID; }
            set
            {
                _test.TestID = value;
                RaisePropertyChanged("TestID");
            }
        }

        public string TestName
        {
            get { return _test.TestName; }
            set
            {
                _test.TestName= value;
                RaisePropertyChanged("TestName");
            }
        }

        public int MaxTime
        {
            get { return _test.MaxTime; }
            set
            {
                _test.MaxTime = value;
                RaisePropertyChanged("MaxTime");
            }
        }

        public int QuestionsNumber
        {
            get { return _test.QuestionsNumber; }
            set
            {
                _test.QuestionsNumber = value;
                RaisePropertyChanged("QuestionsNumber");
            }
        }

        private ObservableCollection<QuestionViewModel> _questions;
        public ObservableCollection<QuestionViewModel> Questions
        {
            get { return _questions; }
            set
            {
                _questions = value;
                RaisePropertyChanged("Questions");
            }
        }

        /*public ObservableCollection<IQuestion> Questions
        {
            get { return _test.Questions; }
            set
            {
                _test.Questions = value;
                RaisePropertyChanged("Questions");
            }
        }*/

        public int TotalPoints
        {
            get { return _test.TotalPoints; }
            set
            {
                _test.TotalPoints = value;
                RaisePropertyChanged("TotalPoints");
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
