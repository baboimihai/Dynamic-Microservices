using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroCore.Utils
{
    public enum TaskDistributionStrategy
    {
        FrequentlyFirsts,
        ExpensiveFirsts,
        MixedFrequentlyExpensive,
        Random
    }
}
