using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteLogToDb
{
    public interface IClientWriteLog
    {
        bool WriteLog(string test);
    }
}
