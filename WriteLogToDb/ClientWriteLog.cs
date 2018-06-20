using Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WriteLogToDb
{
    public class ClientWriteLog : MicroCore<string, bool>, IClientWriteLog
    {
        private WriteLogToDb context;
        public ClientWriteLog()
        {
            context = new WriteLogToDb();
        }

        public bool WriteLog(string test)
        {
            return Run(test);
        }

        protected override bool ProcessTask(string objReceived)
        {
            try
            {
                context.LogClientCallTests.Add(new LogClientCall
                {
                    Date = DateTime.Now,
                    ClientToken = Guid.NewGuid().ToString().Substring(0, 10),
                    SomeString = objReceived
                });
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
