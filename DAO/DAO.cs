using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
//using System.Data.SQLite;
using Latkowska.Testy.Interfaces;
using Latkowska.Tests.DAO.BO;
using System.Data.SQLite;
using DAO.Properties;

namespace Latkowska.Tests.DAO
{
    public class DAO : IDAO
    {
        public List<IUser> Users
        {
            get
            {
                return _users;
            }

            set
            {
                _users = value;
            }
        }
        private List<IUser> _users;

        private List<ITest> _tests;
        private string _databasePath;
        private string _databaseConnectionString;
        public string DatabaseConnectionString
        {
            get { return _databaseConnectionString; }
        }


        public ITest createTestInstance()
        {
            return createTestInstance("", 10);
        }

        public ITest createTestInstance(string testName, int maxTime)
        {
            return new BO.Test(getNewID("Test"), testName, maxTime);
        }

        public IQuestion createQuestionInstance(int testID)
        {
            return createQuestionInstance(testID, "", false, 1);
        }

        public IQuestion createQuestionInstance(int testID, string questionName, bool multipleChoice, int points)
        {
            return new BO.Question(getNewID("Question"), testID, questionName, multipleChoice, points);
        }

        public IAnswer createAnswerInstance(int questionID)
        {
            return createAnswerInstance(questionID, "", false);
        }

        public IAnswer createAnswerInstance(int questionID, string content, bool correct)
        {
            return new BO.Answer(getNewID("Answer"), questionID, content, correct);
        }


        public IEnumerable<IUser> GetAllUsers()
        {
            if (_users == null)
                loadUsersFromDB();
            return _users;
        }

        public IEnumerable<ITest> GetAllTests()
        {
            //Trace.WriteLine("Ładuje testyyy");
            if (_tests == null)
                loadTestsFromDB();
            return _tests;
        }

        public IEnumerable<IQuestion> GetAllQuestionsForTest(int testID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IAnswer> GetAllAnswersForQuestion(int questionID)
        {
            throw new NotImplementedException();
        }

        public DAO()
        {
            _databasePath = Settings.Default.Database;
            _databaseConnectionString = "Data Source=" + _databasePath + ";Version=3;";
            _users = null;
            _tests = null;
        }

        private void loadUsersFromDB(SQLiteConnection dbConnection)
        {
            string sql = "select * from USERS";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            _users = new List<IUser>();
            while (reader.Read())
                _users.Add(new User(Convert.ToInt32(reader["USER_ID"]), reader["LOGIN"].ToString(), reader["PASSWORD"].ToString(), Convert.ToBoolean(reader["EDITOR"])));
        }

        private void loadTestsFromDB(SQLiteConnection dbConnection)
        {
            string sql = "select * from TESTS";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            _tests = new List<ITest>();
            while (reader.Read())
                _tests.Add(new Test(Convert.ToInt32(reader["TEST_ID"]), reader["TEST_NAME"].ToString(), Convert.ToInt32(reader["TEST_MAX_TIME"])));

            foreach (ITest test in _tests)
            {
                loadQuestionsForTestFromDB(test, dbConnection);
            }
        
        }

        private void loadQuestionsForTestFromDB(ITest test, SQLiteConnection dbConnection)
        {
            string testID = test.TestID.ToString();
            string sql = "select * from QUESTIONS where TEST_ID = " + testID;
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            int totalPoints = 0, questionsNumber = 0;
            while (reader.Read())
            {
                test.Questions.Add(new Question(Convert.ToInt32(reader["QUESTION_ID"]), Convert.ToInt32(reader["TEST_ID"]), reader["QUESTION_NAME"].ToString(), Convert.ToBoolean(reader["MULTIPLECHOICE"]), Convert.ToInt32(reader["QUESTION_POINTS"])));
                totalPoints += test.Questions.Last().Points;
                questionsNumber++;
            }
            test.TotalPoints = totalPoints;
            test.QuestionsNumber = questionsNumber;

            foreach (IQuestion question in test.Questions)
            {
                loadAnswersForQuestionFromDB(question, dbConnection);
            }
        }

        private void loadAnswersForQuestionFromDB(IQuestion question, SQLiteConnection dbConnection)
        {
            string questionID = question.QuestionID.ToString();
            string sql = "select * from ANSWERS where QUESTION_ID = " + questionID;
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            int answersNumber = 0;
            while (reader.Read())
            {
                Answer answer = new Answer(Convert.ToInt32(reader["ANSWER_ID"]), Convert.ToInt32(reader["QUESTION_ID"]), reader["ANSWER_CONTENT"].ToString(), Convert.ToBoolean(reader["ANSWER_CORRECT"]));
                question.Answers.Add(answer);
                if (answer.Correct == true)
                    question.CorrectAnswers.Add(answer);
                answersNumber++;
            }
            question.AnswersNumber = answersNumber;
        }

        private void loadUsersFromDB()
        {
            SQLiteConnection dbConnection = new SQLiteConnection(_databaseConnectionString);
            dbConnection.Open();
            loadUsersFromDB(dbConnection);
            dbConnection.Close();
        }

        private void loadTestsFromDB()
        {
            SQLiteConnection dbConnection = new SQLiteConnection(_databaseConnectionString);
            dbConnection.Open();
            loadTestsFromDB(dbConnection);
            dbConnection.Close();
        }

        private int getNewID(SQLiteConnection dbConnection, string type)
        {
            string sql = "SELECT MAX("+type+"_ID) FROM "+type+"S";
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            int id = (int)(long)command.ExecuteScalar();
            return id + 1;
        }

        public int getNewID(string type)
        {
            SQLiteConnection dbConnection = new SQLiteConnection(_databaseConnectionString);
            dbConnection.Open();
            int id = getNewID(dbConnection, type.ToUpper());
            dbConnection.Close();
            return id;
        }

        private int getTestIDForQuestion(SQLiteConnection dbConnection, int questionID)
        {
            string sql = "SELECT TEST_ID FROM QUESTIONS WHERE QUESTION_ID = " + questionID;
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            int? id = (int?)(long?)command.ExecuteScalar();
            return id ?? -1;
        }

        private void loadTest(SQLiteConnection dbConnection, int testID)
        {
            if (testID < 0) return;

            string sql = "select * from TESTS WHERE TEST_ID = " + testID;
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                _tests.Add(new Test(Convert.ToInt32(reader["TEST_ID"]), reader["TEST_NAME"].ToString(), Convert.ToInt32(reader["TEST_MAX_TIME"])));
           loadQuestionsForTestFromDB(_tests.Last(), dbConnection);
        }

        private void updateTest(SQLiteConnection dbConnection, int testID)
        {
            if (testID < 0) return;

            _tests.Remove(_tests.Find(t => t.TestID == testID));

            loadTest(dbConnection, testID);
        }

        public void AddNewRecordToDB(string type, object item)
        {
            string table = type.ToUpper() + "S";
            string sql = "INSERT INTO "+table+" VALUES("+getNewID(type)+",";
            int testID = -1;

            SQLiteConnection dbConnection = new SQLiteConnection(_databaseConnectionString);
            dbConnection.Open();

            switch (type.ToUpper())
            {
                case "TEST":
                    ITest test = (ITest) item;
                    sql += "'" + test.TestName + "',";
                    sql += test.MaxTime;
                    _tests.Add(test);
                    break;
                case "QUESTION":
                    IQuestion question = (IQuestion)item;
                    sql += question.TestID + ",";
                    sql += "'" + question.QuestionName + "',";
                    sql += "'" + question.MultipleChoice + "',";
                    sql += question.Points;
                    testID = question.TestID;
                    break;
                case "ANSWER":
                    IAnswer answer = (IAnswer)item;
                    sql += "'" + answer.Content + "',";
                    sql += answer.QuestionID + ",";
                    sql += "'" + answer.Correct + "'";
                    testID = getTestIDForQuestion(dbConnection, answer.QuestionID);
                    break;
            }
            sql += ")";

            Trace.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();

            if (testID != -1)
                updateTest(dbConnection, testID);

            dbConnection.Close();
        }

        public void UpdateRecordToDB(string type, object item)
        {
            string table = type.ToUpper() + "S";
            string sql = "UPDATE " + table + " SET ";
            int testID = -1;

            SQLiteConnection dbConnection = new SQLiteConnection(_databaseConnectionString);
            dbConnection.Open();

            switch (type.ToUpper())
            {
                case "TEST":
                    ITest test = (ITest)item;
                    sql += "TEST_NAME='" + test.TestName + "',";
                    sql += "TEST_MAX_TIME="+test.MaxTime+" ";
                    sql += "WHERE TEST_ID="+test.TestID;
                    testID = test.TestID;
                    break;
                case "QUESTION":
                    IQuestion question = (IQuestion)item;
                    sql += "TEST_ID="+question.TestID + ",";
                    sql += "QUESTION_NAME='" + question.QuestionName + "',";
                    sql += "MULTIPLECHOICE='"+question.MultipleChoice + "',";
                    sql += "QUESTION_POINTS="+question.Points+" ";
                    sql += "WHERE QUESTION_ID=" + question.QuestionID;
                    testID = question.TestID;
                    break;
                case "ANSWER":
                    IAnswer answer = (IAnswer)item;
                    sql += "ANSWER_CONTENT='" + answer.Content + "',";
                    sql += "QUESTION_ID="+answer.QuestionID + ",";
                    sql += "ANSWER_CORRECT='"+ answer.Correct + "' ";
                    sql += "WHERE ANSWER_ID=" + answer.AnswerID;
                    testID = getTestIDForQuestion(dbConnection, answer.QuestionID);
                    break;
            }
            sql += ";";

            Trace.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();

            updateTest(dbConnection, testID);

            dbConnection.Close();
        }

        public void DeleteRecordFromDB(string type, object item)
        {
            string table = type.ToUpper() + "S";
            string sql = "DELETE FROM " + table + " WHERE "+type.ToUpper()+"_ID = ";
            int testID = -1;

            SQLiteConnection dbConnection = new SQLiteConnection(_databaseConnectionString);
            dbConnection.Open();

            switch (type.ToUpper())
            {
                case "TEST":
                    ITest test = (ITest)item;
                    sql += test.TestID;
                    _tests.Remove(test);
                    break;
                case "QUESTION":
                    IQuestion question = (IQuestion)item;
                    sql += question.QuestionID;
                    testID = question.TestID;
                    break;
                case "ANSWER":
                    IAnswer answer = (IAnswer)item;
                    sql +=  answer.AnswerID;
                    testID = getTestIDForQuestion(dbConnection, answer.QuestionID);
                    break;
            }
            sql += ";";

            Trace.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
            command.ExecuteNonQuery();

            if (testID != -1)
                updateTest(dbConnection, testID);

            dbConnection.Close();
        }
    }

}
