using ClassLibraryMath;
using LibraryMasterOfNumbers;
using Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseOfMath
{
    public class UniverseOfSuperMath : MicroCore<dynamic, NumbersResultsAndInfo>, IUniverseOfSuperMath
    {
        public NumbersResultsAndInfo ComputeNumber(int a, int b, string sign)
        {
            return Run(new { A = a, B = b, Sign = sign });
        }

        protected override NumbersResultsAndInfo ProcessTask(dynamic obj)
        {
            LibraryMath mathLib = new LibraryMath();
            MasterOfNumbers numbersInfo = new MasterOfNumbers();
            var result = mathLib.SimpleMath((int)obj.A, (int)obj.B, (string)obj.Sign);
            var info = numbersInfo.GetNumberInfo(result);
            return new NumbersResultsAndInfo
            {
                Result = result,
                IsEven = info.IsEven,
                IsPositive = info.IsPositive
            };
        }

    }
    public class NumbersResultsAndInfo
    {
        public int Result { get; set; }
        public bool IsPositive { get; set; }
        public bool IsEven { get; set; }
    }
}
