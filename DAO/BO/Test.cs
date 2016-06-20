using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latkowska.Testy.Interfaces;

namespace Latkowska.Tests.DAO.BO
{
    public class Test : ITest
    {
        private int _testID;
        private string _testName;
        private int _maxTime;

        private int _questionsNumber;
        private List<IQuestion> _questions;
        private int _totalPoints;

        public Test(int testID, string testName, int maxTime)
        {
            _testID = testID;
            _testName = testName;
            _maxTime = maxTime;
            _questions = new List<IQuestion>();
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

        public string TestName
        {
            get
            {
                return _testName;
            }
            set
            {
                _testName = value;
            }
        }

        public int MaxTime
        {
            get
            {
                return _maxTime;
            }
            set
            {
                _maxTime = value;
            }
        }

        public int QuestionsNumber
        {
            get
            {
                return _questionsNumber;
            }
            set
            {
                _questionsNumber = value;
            }
        }

        public List<IQuestion> Questions
        {
            get
            {
                return _questions;
            }
            set
            {
                _questions = value;
            }
        }

        public int TotalPoints
        {
            get
            {
                return _totalPoints;
            }
            set
            {
                _totalPoints = value;
            }
        }
    }
}
