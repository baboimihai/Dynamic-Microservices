using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteLogToDb
{
    public class LogClientCall
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ClientToken { get; set; }
        public string SomeString { get; set; }
    }
}
