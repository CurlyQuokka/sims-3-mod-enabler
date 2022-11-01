using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sims3_mod_enabler
{
    internal interface IReportProgress : INotifyPropertyChanged
    {
        int GetProgress();
    }
}
