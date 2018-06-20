using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalServices
{
    public interface IExamples
    {
        NumbersResultsAndInfo ComputeNumber(int a, int b,string sign);
        bool TestBD(string text);
        long StressTestSystem();
        long StressTestSystemBD();
        long StressTestSystemPDF();
    }
}
