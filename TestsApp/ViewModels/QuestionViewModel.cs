using Latkowska.Tests.DAO.BO;
using Latkowska.Testy.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsApp.ViewModels
{
    public class QuestionViewModel : INotifyPropertyChanged
    {
        private IQuestion _question;
        public IQuestion Question
        {
            get { return _question; }
        }

        public QuestionViewModel(IQuestion question)
        {
            _question = question;
            _answers = new ObservableCollection<AnswerViewModel>();
            _correctAnswers = new ObservableCollection<AnswerViewModel>();
            if (question != null)
                foreach (IAnswer a in question.Answers)
                {
                    _answers.Add(new AnswerViewModel(a));
                    if(a.Correct)
                        _correctAnswers.Add(new AnswerViewModel(a));
                }

        }

        public int QuestionID
        {
            get { return _question.QuestionID; }
            set
            {
                _question.QuestionID = value;
                RaisePropertyChanged("QuestionID");
            }
        }

        public int TestID
        {
            get { return _question.TestID; }
            set
            {
                _question.TestID = value;
                RaisePropertyChanged("TestID");
            }
        }

        public string QuestionName
        {
            get { return _question.QuestionName; }
            set
            {
                _question.QuestionName = value;
                RaisePropertyChanged("QuestionName");
            }
        }

        public bool MultipleChoice
        {
            get { return _question.MultipleChoice; }
            set
            {
                _question.MultipleChoice = value;
                RaisePropertyChanged("MultipleChoice");
            }
        }

        public int Points
        {
            get { return _question.Points; }
            set
            {
                _question.Points = value;
                RaisePropertyChanged("Points");
            }
        }

        public int AnswersNumber
        {
            get { return _question.AnswersNumber; }
            set
            {
                _question.AnswersNumber = value;
                RaisePropertyChanged("AnswersNumber");
            }
        }

        private ObservableCollection<AnswerViewModel> _answers;
        public ObservableCollection<AnswerViewModel> Answers
        {
            get { return _answers; }
            set
            {
                _answers = value;
                //zmieniac tez w IAnswer?
                RaisePropertyChanged("Answers");
            }
        }

        private ObservableCollection<AnswerViewModel> _correctAnswers;
        public ObservableCollection<AnswerViewModel> CorrectAnswers 
        {
            get { return _correctAnswers; }
            set
            {
                _correctAnswers = value;
                RaisePropertyChanged("CorrectAnswers");
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
