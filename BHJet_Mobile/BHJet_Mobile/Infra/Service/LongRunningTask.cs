using System;
using System.Collections.Generic;
using System.Text;

namespace BHJet_Mobile.Infra.Service
{
    public class StartLongRunningTaskMessage { }

    public class StopLongRunningTaskMessage { }

    public class TickedMessage
    {
        public string Message { get; set; }
    }
}
