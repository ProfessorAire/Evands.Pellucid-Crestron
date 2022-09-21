using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Crestron.SimplSharp
{
    public class CCriticalSection
    {
        private object syncRoot = new object();

        public void Enter()
        {
            Monitor.Enter(syncRoot);
        }

        public void Leave()
        {
            Monitor.Exit(syncRoot);
        }
    }
}
