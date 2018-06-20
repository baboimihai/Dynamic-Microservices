using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroCore
{
    public class MicroTaskInfo
    {
        public Type Type { get; set; }
        public string SThis { get; set; }
        public List<TaskLogRecord> Logs { get; set; }
    }
    public class TaskLogRecord
    {
        public DateTime Date { get; set; }
        public long TimeExecution { get; set; }
    }
}
