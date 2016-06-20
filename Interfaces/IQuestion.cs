using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latkowska.Testy.Interfaces
{
    public interface IQuestion
    {
        int QuestionID { get; set; }
        int TestID { get; set; }
        string QuestionName { get; set; }
        bool MultipleChoice { get; set; }
        int Points { get; set; }

        int AnswersNumber { get; set; }
        List<IAnswer> Answers { get; set; }
        List<IAnswer> CorrectAnswers { get; set; }
    }
}
