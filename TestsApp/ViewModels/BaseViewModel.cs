using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latkowska.Testy.ViewModels
{
    public class BaseViewModel
    {
        public MainViewModel Parent { get; set; }

        public BaseViewModel(MainViewModel mainvm)
        {
            Parent = mainvm;
        }
    }
}
