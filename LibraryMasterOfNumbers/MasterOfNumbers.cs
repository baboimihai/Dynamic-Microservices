using Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMasterOfNumbers
{
    public class MasterOfNumbers : MicroCore<double, NumberInfo>, IMasterOfNumbers
    {
        public NumberInfo GetNumberInfo(double number)
        {
            return Run(number);
        }

        protected override NumberInfo ProcessTask(double objReceived)
        {
            return new NumberInfo
            {
                IsEven = objReceived % 2 == 0,
                IsPositive = objReceived >= 0
            };
        }
    }
    public class NumberInfo
    {
        public bool IsPositive { get; set; }
        public bool IsEven { get; set; }
    }
}
