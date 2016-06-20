using Latkowska.Testy.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Latkowska.Tests.DAO.BO;
using System.ComponentModel;
using TestsApp.ViewModels;

namespace Latkowska.Testy.ViewModels
{
    public class AddTestViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private bool testEdition;
        private bool questionEdition;
        private bool answerEdition;

        public TestViewModel TestVM { get; set; }
        public ITest Test { get; set; }
        private QuestionViewModel _selectedQuestion;
        public QuestionViewModel SelectedQuestion 
        {
            get { return _selectedQuestion; }
            set
            {
                _selectedQuestion = value;
                RaisePropertyChanged("SelectedQuestion");
            }     
        }
        private AnswerViewModel _selectedAnswer;
        public AnswerViewModel SelectedAnswer
        {
            get { return _selectedAnswer; }
            set
            {
                _selectedAnswer = value;
                RaisePropertyChanged("SelectedAnswer");
            }
        }
        private AnswerViewModel _newAnswer;
        public AnswerViewModel NewAnswer
        {
            get { return _newAnswer; }
            set
            {
                _newAnswer = value;
                RaisePropertyChanged("NewAnswer");
            }
        }
        private int _selectedTab;
        public int SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                _selectedTab = value;
                RaisePropertyChanged("SelectedTab");
            }
        }

        private RelayCommand _saveTestCommand;
        private RelayCommand _quitCommand;

        private RelayCommand _addQuestionCommand;
        private RelayCommand _editQuestionCommand;
        private RelayCommand _eraseQuestionCommand;
        private RelayCommand _saveQuestionCommand;
        private RelayCommand _quitQuestionCommand;

        private RelayCommand _editAnswerCommand;
        private RelayCommand _eraseAnswerCommand;
        private RelayCommand _saveAnswerCommand;

        public ICommand SaveTestCommand
        {
            get { return _saveTestCommand; }
        }

        public ICommand QuitCommand
        {
            get { return _quitCommand; }
        }

        public ICommand AddQuestionCommand
        {
            get { return _addQuestionCommand; }
        }

        public ICommand EditQuestionCommand
        {
            get { return _editQuestionCommand; }
        }

        public ICommand EraseQuestionCommand
        {
            get { return _eraseQuestionCommand; }
        }

        public ICommand SaveQuestionCommand
        {
            get { return _saveQuestionCommand; }
        }

        public ICommand QuitQuestionCommand
        {
            get { return _quitQuestionCommand; }
        }

        public ICommand EditAnswerCommand
        {
            get { return _editAnswerCommand; }
        }

        public ICommand EraseAnswerCommand
        {
            get { return _eraseAnswerCommand; }
        }

        public ICommand SaveAnswerCommand
        {
            get { return _saveAnswerCommand; }
        }

        public AddTestViewModel(MainViewModel mainVM, TestViewModel testVM) : base (mainVM)
        {
            TestVM = testVM;
            if (testVM.Test == null)
            {
                testEdition = false;
                Test = Parent.DAO.createTestInstance();
            }
            else
            {
                testEdition = true;
                Test = testVM.Test;
            }

            _selectedQuestion = null;
            _selectedAnswer = null;

            _saveTestCommand = new RelayCommand((param) => this.SaveTest());
            _quitCommand = new RelayCommand((param) => this.Quit());

            _addQuestionCommand = new RelayCommand((param) => this.AddQuestion());
            _editQuestionCommand = new RelayCommand((param) => this.EditQuestion());
            _eraseQuestionCommand = new RelayCommand((param) => this.EraseQuestion());
            _saveQuestionCommand = new RelayCommand((param) => this.SaveQuestion());
            _quitQuestionCommand = new RelayCommand((param) => this.QuitQuestion());

            _editAnswerCommand = new RelayCommand((param) => this.EditAnswer());
            _eraseAnswerCommand = new RelayCommand((param) => this.EraseAnswer());
            _saveAnswerCommand = new RelayCommand((param) => this.SaveAnswer());
        }

        private void SaveTest()
        {
            if (testEdition)
                Parent.DAO.UpdateRecordToDB("Test", Test);
            else
                Parent.DAO.AddNewRecordToDB("Test", Test);

            Parent.CurrentView = new TestsListViewModel(Parent);
        }

        private void Quit()
        {
            Parent.CurrentView = new TestsListViewModel(Parent);
        }


        private void AddQuestion()
        {
            SelectedTab = 2;
            questionEdition = false;
            SelectedQuestion = new QuestionViewModel(Parent.DAO.createQuestionInstance(Test.TestID));
            NewAnswer = new AnswerViewModel(Parent.DAO.createAnswerInstance(SelectedQuestion.QuestionID));
            Trace.WriteLine("Dodawanie pytania");
        }

        private void EditQuestion()
        {
            if (_selectedQuestion != null)
            {
                questionEdition = true;
                SelectedTab = 2;
                NewAnswer = new AnswerViewModel(Parent.DAO.createAnswerInstance(SelectedQuestion.QuestionID));
                Trace.WriteLine("Edycja pytania");
            }
        }

        private void EraseQuestion()
        {
            if (_selectedQuestion != null)
            {
                Trace.WriteLine("Usuwanie pytania");
                Parent.DAO.DeleteRecordFromDB("Question", _selectedQuestion.Question);
                foreach(AnswerViewModel answer in _selectedQuestion.Answers)
                    Parent.DAO.DeleteRecordFromDB("Answer", answer.Answer);
                TestVM.Questions.Remove(SelectedQuestion);
            }
        }

        private void SaveQuestion()
        {
            if (questionEdition)
                Parent.DAO.UpdateRecordToDB("Question", SelectedQuestion.Question);
            else
            {
                Parent.DAO.AddNewRecordToDB("Question", SelectedQuestion.Question);
                TestVM.Questions.Add(SelectedQuestion);
                TestVM.QuestionsNumber++;
                TestVM.TotalPoints += SelectedQuestion.Points;
            }
            Trace.WriteLine("Zapis pytania");
            SelectedTab = 1;
        }

        private void QuitQuestion()
        {
            _selectedQuestion = null;
            SelectedTab = 1;
        }


        private void EditAnswer()
        {
            answerEdition = true;
            NewAnswer = SelectedAnswer;
            Trace.WriteLine("Edycja odpowiedzi");
        }

        private void EraseAnswer()
        {
            if (_selectedAnswer != null)
            {
                Trace.WriteLine("Usuwanie odpowiedzi");
                Parent.DAO.DeleteRecordFromDB("Answer", _selectedAnswer.Answer);
                if (_selectedAnswer.Correct)
                {
                    SelectedQuestion.CorrectAnswers.Remove(_selectedAnswer);
                    if (SelectedQuestion.CorrectAnswers.Count <= 1)
                        SelectedQuestion.MultipleChoice = false;
                }
                SelectedQuestion.Answers.Remove(_selectedAnswer);
                SelectedQuestion.AnswersNumber--;
            }
        }

        private void SaveAnswer()
        {
            if (answerEdition)
            {
                Parent.DAO.UpdateRecordToDB("Answer", NewAnswer.Answer);
                if (NewAnswer.Correct && !SelectedQuestion.CorrectAnswers.Contains(NewAnswer))
                {
                    SelectedQuestion.CorrectAnswers.Add(NewAnswer);
                    if (SelectedQuestion.CorrectAnswers.Count > 1)
                        SelectedQuestion.MultipleChoice = true;
                }
            }
            else
            {
                Parent.DAO.AddNewRecordToDB("Answer", NewAnswer.Answer);
                SelectedQuestion.Answers.Add(NewAnswer);
                SelectedQuestion.AnswersNumber++;
                if (NewAnswer.Correct)
                {
                    SelectedQuestion.CorrectAnswers.Add(NewAnswer);
                    if (SelectedQuestion.CorrectAnswers.Count > 1)
                        SelectedQuestion.MultipleChoice = true;
                }
            }
            NewAnswer = new AnswerViewModel(Parent.DAO.createAnswerInstance(SelectedQuestion.QuestionID));
            answerEdition = false;
            Trace.WriteLine("Zapisanie odpowiedzi");
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
