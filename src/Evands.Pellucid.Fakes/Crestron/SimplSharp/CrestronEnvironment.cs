using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp
{
    public static class CrestronEnvironment
    {
        public static DateTime GetLocalTime()
        {
            return DateTime.Now;
        }

        public static event ProgramStatusEventHandler ProgramStatusEventHandler;

        public static void RaiseProgramStatusEventHandler(eProgramStatusEventType eventType)
        {
            var pseh = ProgramStatusEventHandler;
            if (pseh != null) { pseh(eventType); }
        }
    }
}
