using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latkowska.Testy.Interfaces;

namespace Latkowska.Tests.DAO.BO
{
    public class Question : IQuestion
    {
        private int _questionID;
        private int _testID;
        private string _questionName;
        private bool _multipleChoice;
        private int _points;

        private int _answersNumber;
        private List<IAnswer> _answers;
        private List<IAnswer> _correctAnswers;

        public Question(int questionID, int testID, string questionName, bool multipleChoice, int points)
        {
            _questionID = questionID;
            _testID = testID;
            _questionName = questionName;
            _multipleChoice = multipleChoice;
            _points = points;

            _answers = new List<IAnswer>();
            _correctAnswers = new List<IAnswer>();
        }

        public int QuestionID
        {
            get
            {
                return _questionID;
            }
            set
            {
                _questionID = value;
            }
        }

        public int TestID
        {
            get
            {
                return _testID;
            }
            set
            {
                _testID = value;
            }
        }

        public string QuestionName
        {
            get
            {
                return _questionName;
            }
            set
            {
                _questionName = value;
            }
        }

        public bool MultipleChoice
        {
            get
            {
                return _multipleChoice;
            }
            set
            {
                _multipleChoice = value;
            }
        }

        public int Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }

        public int AnswersNumber
        {
            get
            {
                return _answersNumber;
            }
            set
            {
                _answersNumber = value;
            }
        }

        public List<IAnswer> Answers
        {
            get
            {
                return _answers;
            }
            set
            {
                _answers = value;
            }
        }

        public List<IAnswer> CorrectAnswers
        {
            get
            {
                return _correctAnswers;
            }
            set
            {
                _correctAnswers = value;
            }
        }

    }
}
