using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroCore
{
    public class MicroClientInfo
    {
        public string ClientToken { get; set; }
        public byte[] ClientKey { get; set; }
        public bool IsActive { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public DateTime LastKeepAlive { get; set; }
    }
}
