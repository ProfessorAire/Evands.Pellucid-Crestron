using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crestron.SimplSharp
{
    public static class InitialParametersClass
    {
        static InitialParametersClass()
        {
            ApplicationNumber = 1;
        }

        public static int ApplicationNumber { get; set; }
    }
}
