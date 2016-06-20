using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latkowska.Testy.Interfaces;

namespace Latkowska.Tests.DAO.BO
{
    public class Answer : IAnswer
    {
        private int _answerID;
        private int _questionID;
        private string _content;
        private bool _correct;

        public Answer(int answerID, int questionID, string content, bool correct)
        {
            _answerID = answerID;
            _questionID = questionID;
            _content = content;
            _correct = correct;
        }

        public int AnswerID
        {
            get
            {
                return _answerID;
            }
            set
            {
                _answerID = value;
            }
        }

        public int QuestionID
        {
            get { return _questionID; }
            set { _questionID = value; }
        }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }

        public bool Correct
        {
            get
            {
                return _correct;
            }
            set
            {
                _correct = value;
            }
        }
    }
}
