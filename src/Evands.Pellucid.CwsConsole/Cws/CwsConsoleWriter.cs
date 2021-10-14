#region copyright
// <copyright file="CwsConsoleWriter.cs" company="Christopher McNeely">
// The MIT License (MIT)
// Copyright (c) Christopher McNeely
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
#endregion

using System;
using Crestron.SimplSharp;
using Crestron.SimplSharp.WebScripting;
using Evands.Pellucid.Terminal;

namespace Evands.Pellucid.Cws
{
    /// <summary>
    /// CWS console writer. Registers with an instance of the <see cref="HttpCwsServer"/> from Crestron.
    /// <para>Implements the <see cref="IConsoleWriter"/> interface.</para>
    /// <para>Implements the <see cref="IDisposable"/> interface.</para>
    /// </summary>
    public class CwsConsoleWriter : IConsoleWriter, IDisposable
    {
        /// <summary>
        /// The <see cref="HttpCwsRoute"/> registered to the server.
        /// </summary>        
        private HttpCwsRoute consoleRoute;

        /// <summary>
        /// The object that handles the route requests.
        /// </summary>        
        private ConsoleRoute consoleRouteHandler;

        /// <summary>
        /// The CWS server the route is registered with.
        /// </summary>        
        private HttpCwsServer server;

        /// <summary>
        /// Initializes a new instance of the <see cref="CwsConsoleWriter"/> class.
        /// </summary>
        /// <param name="cwsServer">The <see cref="HttpCwsServer"/> to register with.</param>
        /// <param name="websocketPort">The port the websocket server should listen on.</param>
        public CwsConsoleWriter(HttpCwsServer cwsServer, int websocketPort)
            : this(cwsServer, "console" + InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), false, websocketPort)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CwsConsoleWriter"/> class.
        /// </summary>
        /// <param name="cwsServer">The <see cref="HttpCwsServer"/> to register with.</param>
        /// <param name="secureWebsocket"><see langword="true"/> to indicate the websocket server should use a secure TCP server; otherwise <see langword="false"/>.</param>
        /// <param name="websocketPort">The port the websocket server should listen on.</param>
        public CwsConsoleWriter(HttpCwsServer cwsServer, bool secureWebsocket, int websocketPort)
            : this(cwsServer, "console" + InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), secureWebsocket, websocketPort)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CwsConsoleWriter"/> class.
        /// </summary>
        /// <param name="cwsServer">The <see cref="HttpCwsServer"/> to register with.</param>
        /// <param name="routePath">The name of the CWS route the console should be created at, such as: "/console/program01/".</param>
        /// <param name="websocketPort">The port the websocket server should listen on.</param>
        public CwsConsoleWriter(HttpCwsServer cwsServer, string routePath, int websocketPort)
            : this(cwsServer, routePath, false, websocketPort, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CwsConsoleWriter"/> class.
        /// </summary>
        /// <param name="cwsServer">The <see cref="HttpCwsServer"/> to register with.</param>
        /// <param name="routePath">The name of the CWS route the console should be created at, such as: "/console/program01/".</param>
        /// <param name="websocketPort">The port the websocket server should listen on.</param>
        /// <param name="authenticateRoute"><see langword="true"/> to authenticate the route. Only valid on 4-Series processors.</param>
        public CwsConsoleWriter(HttpCwsServer cwsServer, string routePath, int websocketPort, bool authenticateRoute)
            : this(cwsServer, routePath, false, websocketPort, authenticateRoute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CwsConsoleWriter"/> class.
        /// </summary>
        /// <param name="cwsServer">The <see cref="HttpCwsServer"/> to register with.</param>
        /// <param name="routePath">The name of the CWS route the console should be created at, such as: "/console/program01/".</param>
        /// <param name="secureWebsocket"><see langword="true"/> to indicate the websocket server should use a secure TCP server; otherwise <see langword="false"/>.</param>
        /// <param name="websocketPort">The port the websocket server should listen on.</param>  
        public CwsConsoleWriter(HttpCwsServer cwsServer, string routePath, bool secureWebsocket, int websocketPort)
            : this(cwsServer, routePath, secureWebsocket, websocketPort, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CwsConsoleWriter"/> class.
        /// </summary>
        /// <param name="cwsServer">The <see cref="HttpCwsServer"/> to register with.</param>
        /// <param name="websocketPort">The port the websocket server should listen on.</param>
        /// <param name="authenticateRoute"><see langword="true"/> to authenticate the route. Only valid on 4-Series processors.</param>
        public CwsConsoleWriter(HttpCwsServer cwsServer, int websocketPort, bool authenticateRoute)
            : this(cwsServer, "console" + InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), false, websocketPort, authenticateRoute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CwsConsoleWriter"/> class.
        /// </summary>
        /// <param name="cwsServer">The <see cref="HttpCwsServer"/> to register with.</param>
        /// <param name="secureWebsocket"><see langword="true"/> to indicate the websocket server should use a secure TCP server; otherwise <see langword="false"/>.</param>
        /// <param name="websocketPort">The port the websocket server should listen on.</param>
        /// <param name="authenticateRoute"><see langword="true"/> to authenticate the route. Only valid on 4-Series processors.</param>
        public CwsConsoleWriter(HttpCwsServer cwsServer, bool secureWebsocket, int websocketPort, bool authenticateRoute)
            : this(cwsServer, "console" + InitialParametersClass.ApplicationNumber.ToString().PadLeft(2, '0'), secureWebsocket, websocketPort, authenticateRoute)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CwsConsoleWriter"/> class.
        /// </summary>
        /// <param name="cwsServer">The <see cref="HttpCwsServer"/> to register with.</param>
        /// <param name="routePath">The name of the CWS route the console should be created at, such as: "/console/program01/".</param>
        /// <param name="secureWebsocket"><see langword="true"/> to indicate the websocket server should use a secure TCP server; otherwise <see langword="false"/>.</param>
        /// <param name="websocketPort">The port the websocket server should listen on.</param>
        /// <param name="authenticateRoute"><see langword="true"/> to authenticate the route. Only valid on 4-Series processors.</param>     
        public CwsConsoleWriter(HttpCwsServer cwsServer, string routePath, bool secureWebsocket, int websocketPort, bool authenticateRoute)
        {
            if (cwsServer == null)
            {
                throw new ArgumentNullException("cwsServer");
            }

            this.server = cwsServer;

            try
            {
                if (CrestronEnvironment.ProgramCompatibility == eCrestronSeries.Series3 || CrestronEnvironment.ProgramCompatibility == eCrestronSeries.Unspecified)
                {
                    consoleRoute = new HttpCwsRoute(routePath);
                }
                else
                {
                    consoleRoute = new HttpCwsRoute(routePath, authenticateRoute);
                }
            }
            catch
            {
                consoleRoute = new HttpCwsRoute(routePath);
            }

            consoleRouteHandler = new ConsoleRoute(websocketPort, secureWebsocket);
            consoleRoute.RouteHandler = consoleRouteHandler;
            cwsServer.AddRoute(this.consoleRoute);
        }

        /// <inheritdocs/>
        public void Write(string message, params object[] args)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.Write(message, args);
            }
        }

        /// <inheritdocs/>
        public void Write(string message)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.Write(message);
            }
        }

        /// <inheritdocs/>
        public void WriteLine(string message, params object[] args)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.WriteLine(message, args);
            }
        }

        /// <inheritdocs/>
        public void WriteLine(string message)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.WriteLine(message);
            }
        }

        /// <inheritdocs/>
        public void WriteLine()
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.WriteLine();
            }
        }

        /// <inheritdocs/>
        public void WriteCommandResponse(string message, params object[] args)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.WriteCommandResponse(message, args);
            }
        }

        /// <inheritdocs/>
        public void WriteCommandResponse(string message)
        {
            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.WriteCommandResponse(message);
            }
        }

        /// <summary>
        /// Disposes of resources.
        /// </summary>        
        public void Dispose()
        {
            if (consoleRoute != null)
            {
                if (server != null)
                {
                    server.RemoveRoute(consoleRoute);
                }

                consoleRoute = null;
            }

            if (consoleRouteHandler != null)
            {
                consoleRouteHandler.Dispose();
                consoleRouteHandler = null;
            }

            server = null;
        }
    }
}
