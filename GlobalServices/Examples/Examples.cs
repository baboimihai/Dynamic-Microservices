using ClassLibraryMath;
using LibraryMasterOfNumbers;
using PdfDiploma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniverseOfMath;
using WriteLogToDb;

namespace GlobalServices
{
    public class Examples : IExamples
    {
        private readonly ILibraryMath mathLib;
        private readonly IMasterOfNumbers numbersInfo;
        private readonly IUniverseOfSuperMath universeOfMath;
        private readonly IClientWriteLog clientWriteLog;
        private readonly IGenerateDiploma generateDiploma;
        public Examples(ILibraryMath mathLib, IMasterOfNumbers numbersInfo,
            IUniverseOfSuperMath universeOfMath, IClientWriteLog clientWriteLog, IGenerateDiploma generateDiploma)
        {
            this.mathLib = mathLib;
            this.numbersInfo = numbersInfo;
            this.universeOfMath = universeOfMath;
            this.generateDiploma = generateDiploma;
            this.clientWriteLog = clientWriteLog;
        }
        public long StressTestSystem()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < 10; i++)
            {
                mathLib.SimpleMath(214, 1421, "*");

            }
            watch.Stop();
            return watch.ElapsedMilliseconds;

        }
        public long StressTestSystemBD()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < 10; i++)
            {

                clientWriteLog.WriteLog("mihai");

            }
            watch.Stop();
            return watch.ElapsedMilliseconds;

        }
        public long StressTestSystemPDF()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < 10; i++)
            {
                generateDiploma.WriteDiploma("mihai");
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;

        }
        public NumbersResultsAndInfo ComputeNumber(int a, int b, string sign)
        {
            var result = universeOfMath.ComputeNumber(a, b, sign);
            return new NumbersResultsAndInfo
            {
                Result = result.Result,
                IsEven = result.IsEven,
                IsPositive = result.IsPositive
            };
            //var result= mathLib.SumNumbers(a, b);
            //var info = numbersInfo.GetNumberInfo(result);
            //return new NumbersResultsAndInfo
            //{
            //    Result = result,
            //    IsEven=info.IsEven,
            //    IsPositive=info.IsPositive
            //};
        }
        public bool TestBD(string text)
        {
            return clientWriteLog.WriteLog(text);
        }
    }
    public class NumbersResultsAndInfo
    {
        public int Result { get; set; }
        public bool IsPositive { get; set; }
        public bool IsEven { get; set; }
    }
}
