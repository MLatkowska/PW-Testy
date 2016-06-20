using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latkowska.Testy.Interfaces
{
    public interface ITest
    {
        int TestID { get; set; }
        string TestName { get; set; }
        int MaxTime { get; set; }

        int QuestionsNumber { get; set; }
        List<IQuestion> Questions { get; set; }
        int TotalPoints { get; set; }
    }
}
