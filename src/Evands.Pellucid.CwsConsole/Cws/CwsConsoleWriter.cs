using System;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.WebScripting;
using Evands.Pellucid.Terminal;

namespace Evands.Pellucid.Cws
{
    public class CwsConsoleWriter : IConsoleWriter
    {
        private HttpCwsRoute consoleRoute;

        private ConsoleRoute consoleRouteHandler;

        public CwsConsoleWriter(HttpCwsServer cwsServer, int websocketPort)
            : this(cwsServer, "console" + InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), websocketPort)
        {
        }

        public CwsConsoleWriter(HttpCwsServer cwsServer, string routePath, int websocketPort)
            : this(cwsServer, routePath, websocketPort, false)
        {
        }

        public CwsConsoleWriter(HttpCwsServer cwsServer, int websocketPort, bool authenticateRoute)
            : this(cwsServer, "console" + InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), websocketPort, authenticateRoute)
        {
        }

        public CwsConsoleWriter(HttpCwsServer cwsServer, string routePath, int websocketPort, bool authenticateRoute)
        {
            if (cwsServer == null)
            {
                throw new ArgumentNullException("cwsServer");
            }

            if (CrestronEnvironment.ProgramCompatibility == eCrestronSeries.Series3 || CrestronEnvironment.ProgramCompatibility == eCrestronSeries.Unspecified)
            {
                consoleRoute = new HttpCwsRoute(routePath);
            }
            else
            {
                consoleRoute = new HttpCwsRoute(routePath, authenticateRoute);
            }

            consoleRouteHandler = new ConsoleRoute(websocketPort);
            consoleRoute.RouteHandler = consoleRouteHandler;
            cwsServer.AddRoute(this.consoleRoute);
        }

        public void Write(string message, params object[] args)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.Write(message, args);
            }
        }

        public void Write(string message)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.Write(message);
            }
        }

        public void WriteLine(string message, params object[] args)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.WriteLine(message, args);
            }
        }

        public void WriteLine(string message)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.WriteLine(message);
            }
        }

        public void WriteLine()
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.WriteLine();
            }
        }

        public void WriteCommandResponse(string message, params object[] args)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.WriteCommandResponse(message, args);
            }
        }

        public void WriteCommandResponse(string message)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.WriteCommandResponse(message);
            }
        }
    }
}
