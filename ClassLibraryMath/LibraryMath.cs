using Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClassLibraryMath.LibraryMath;

namespace ClassLibraryMath
{

    public class LibraryMath : MicroCore<dynamic, OutputTestProcess>, ILibraryMath
    {
        public int SimpleMath(int a, int b, string sign)
        {
            var obj = Run(new { A = a, B = b, Sign = sign });
            return obj.C;
        }

        protected override OutputTestProcess ProcessTask(dynamic objReceived)
        {
            int result=0;
            switch ((string)objReceived.Sign)
            {
                case "+":
                    result = objReceived.A + objReceived.B;
                    break;
                case "-":
                    result = objReceived.A - objReceived.B;
                    break;
                case "*":
                    result = objReceived.A * objReceived.B;
                    break;
                case "/":
                    result = objReceived.A / objReceived.B;
                    break;
            }
            return new OutputTestProcess
            {
                C = result
            };
        }
        public class InputTestProcess
        {
            public int A { get; set; }
            public int B { get; set; }
        }
        public class OutputTestProcess
        {
            public int C { get; set; }
        }
    }
}
