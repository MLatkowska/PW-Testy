using Latkowska.Testy.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsApp.ViewModels
{
    public class AnswerViewModel :INotifyPropertyChanged
    {
        private IAnswer _answer;
        public IAnswer Answer
        {
            get { return _answer; }
        }

        public AnswerViewModel(IAnswer answer)
        {
            _answer = answer;
        }

        public int AnswerID
        {
            get { return _answer.AnswerID; }
            set
            {
                _answer.AnswerID = value;
                RaisePropertyChanged("AnswerID");
            }
        }

        public int QuestionID
        {
            get { return _answer.QuestionID; }
            set
            {
                _answer.QuestionID = value;
                RaisePropertyChanged("QuestionID");
            }
        }

        public string Content
        {
            get { return _answer.Content; }
            set
            {
                _answer.Content = value;
                RaisePropertyChanged("Content");
            }
        }

        public bool Correct
        {
            get { return _answer.Correct; }
            set
            {
                _answer.Correct = value;
                RaisePropertyChanged("Correct");
            }
        }

        private bool _chosen;
        public bool Chosen
        {
            get { return _chosen; }
            set
            {
                _chosen = value;
                RaisePropertyChanged("Chosen");
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
