using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latkowska.Testy.Interfaces
{
    public interface IUser
    {
        int UserID { get; set; }
        string Login { get; set; }
        string Password { get; set; }
        bool Editor { get; set; }
    }
}
