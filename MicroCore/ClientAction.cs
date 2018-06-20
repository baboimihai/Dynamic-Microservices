using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroCore
{
    public enum ClientAction
    {
        [Description("Get")]
        Get,
        [Description("RestartServer")]
        RestartServer,
        [Description("InstallService")]
        InstallService,
        [Description("CheckClient")]
        CheckClient,
        [Description("ExtendedService")]
        ExtendedService
    }
}
