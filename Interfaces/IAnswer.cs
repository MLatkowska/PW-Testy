using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latkowska.Testy.Interfaces
{
    public interface IAnswer
    {
        int AnswerID { get; set; }
        int QuestionID { get; set; }
        string Content { get; set; }
        bool Correct { get; set; }
    }
}
