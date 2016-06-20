using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latkowska.Testy.Interfaces
{
    public interface IDAO
    {
        string DatabaseConnectionString { get; }

        ITest createTestInstance();
        ITest createTestInstance(string testName, int maxTime);
        IQuestion createQuestionInstance(int testID);
        IQuestion createQuestionInstance(int testID, string questionName, bool multipleChoice, int points);
        IAnswer createAnswerInstance(int questionID);
        IAnswer createAnswerInstance(int questionID, string content, bool correct);

        IEnumerable<IUser> GetAllUsers();
        IEnumerable<ITest> GetAllTests();
        IEnumerable<IQuestion> GetAllQuestionsForTest(int testID);
        IEnumerable<IAnswer> GetAllAnswersForQuestion(int questionID);

        int getNewID(string type);

        void AddNewRecordToDB(string type, object item);
        void UpdateRecordToDB(string type, object item);
        void DeleteRecordFromDB(string type, object item);
    }
}
