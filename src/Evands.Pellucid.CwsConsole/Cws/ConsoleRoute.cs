#region copyright
// <copyright file="ConsoleRoute.cs" company="Christopher McNeely">
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
using System.Linq;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharp.WebScripting;
using Evands.Pellucid;
using Evands.Pellucid.Diagnostics;
using Evands.Pellucid.Terminal;

namespace Evands.Pellucid.Cws
{
    /// <summary>
    /// CWS route handler for <see cref="CwsConsoleWriter"/>.
    /// </summary>
    internal class ConsoleRoute : IHttpCwsHandler, IConsoleWriter, IDebugData, IDisposable
    {
        /// <summary>
        /// The websocket server.
        /// </summary>        
        private WebSocketListener server;

        /// <summary>
        /// The socket the websocket server should connect on.
        /// </summary>        
        private string socketPort = string.Empty;

        /// <summary>
        /// Indicates whether the websocket server should use a secure server.
        /// </summary>        
        private bool useSecureWebsocket = false;

        /// <summary>
        /// The prompt to write to the console.
        /// </summary>        
        private string prompt = InitialParametersClass.ControllerPromptName + ">";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleRoute"/> class.
        /// </summary>
        /// <param name="port">The port the websocket should connect on.</param>
        /// <param name="secure">Set to <see langword="true"/> to use a secure websocket server.</param>
        public ConsoleRoute(int port, bool secure)
        {
            server = new WebSocketListener(secure);
            server.DataReceived += HandleDataReceived;
            socketPort = port.ToString();
            useSecureWebsocket = secure;
            server.Start(port);
        }

        /// <summary>
        /// Disposes of resources.
        /// </summary>        
        public void Dispose()
        {
            if (server != null)
            {
                server.Dispose();
                server = null;
            }
        }

        /// <inheritdoc/>     
        string IDebugData.Header
        {
            get
            {
                return "ConsoleRoute";
            }
        }

        /// <inheritdoc/>       
        Evands.Pellucid.Terminal.Formatting.ColorFormat IDebugData.HeaderColor
        {
            get { return ConsoleBase.Colors.BrightCyan; }
        }

        /// <inheritdoc/>
        public void Write(string message, params object[] args)
        {
            if (Validate())
            {
                server.Send(string.Format(message.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>").Replace(" ", "&nbsp;"), args), true);
            }
        }

        /// <inheritdoc/>
        public void Write(string message)
        {
            if (Validate())
            {
                server.Send(message.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>").Replace(" ", "&nbsp;"), true);
            }
        }

        /// <inheritdoc/>
        public void WriteLine(string message, params object[] args)
        {
            if (Validate())
            {
                server.Send(string.Format(message.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>").Replace(" ", "&nbsp;") + "<br/>", args), true);
                server.Send(prompt + "<br/>", true);
            }
        }

        /// <inheritdoc/>
        public void WriteLine(string message)
        {
            if (Validate())
            {
                server.Send(message.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>").Replace(" ", "&nbsp;") + "<br/>", true);
                server.Send(prompt + "<br/>", true);
            }
        }

        /// <inheritdoc/>
        public void WriteLine()
        {
            if (Validate())
            {
                server.Send("<br/>" + prompt + "<br/>", true);
            }
        }

        /// <inheritdoc/>      
        public void WriteCommandResponse(string message, params object[] args)
        {
            if (Validate())
            {
                server.Send(string.Format(message.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>").Replace(" ", "&nbsp;"), args), true);
            }
        }

        /// <inheritdoc/>       
        public void WriteCommandResponse(string message)
        {
            if (Validate())
            {
                server.Send(message.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>").Replace(" ", "&nbsp;"), true);
            }
        }

        /// <summary>
        /// Processes the CWS request.
        /// </summary>
        /// <param name="context">The context of the request.</param>
        void IHttpCwsHandler.ProcessRequest(HttpCwsContext context)
        {
            switch (context.Request.HttpMethod.ToUpper())
            {
                case "GET":
                    ProcessGet(context);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Processes a GET request.
        /// </summary>
        /// <param name="context">The context of the request.</param>        
        private void ProcessGet(HttpCwsContext context)
        {
            Debug.WriteLine("Processing GET.");

            try
            {
                var html = string.Empty;
                var css = string.Empty;
                using (var stream = Crestron.SimplSharp.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Evands.Pellucid.Resources.CwsConsoleTemplate.html"))
                {
                    using (var reader = new Crestron.SimplSharp.CrestronIO.StreamReader(stream, true))
                    {
                        html = reader.ReadToEnd();
                    }
                }
                using (var stream = Crestron.SimplSharp.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Evands.Pellucid.Resources.default.css"))
                {
                    using (var reader = new Crestron.SimplSharp.CrestronIO.StreamReader(stream, true))
                    {
                        css = reader.ReadToEnd();
                    }
                }

                context.Response.StatusCode = 200;
                var host = context.Request.Url.Host;
                var protocol = useSecureWebsocket ? "wss://" : "ws://";
                html = html.Replace("{{ CSS }}", css).Replace("{{ PROTOCOL }}", protocol).Replace("{{ PORT }}", socketPort).Replace("{{ HOST }}", host);
                context.Response.Write(html, true);
            }
            catch (Exception ex)
            {
                Debug.WriteException(this, ex, "Exception while processing GET.");
                context.Response.StatusCode = 500;
                context.Response.StatusDescription = string.Format("Internal Server Error - Exception encountered while retrieving Console Endpoint.<br/><br/>{0}", ex.ToString());
                context.Response.Write(context.Response.StatusDescription, true);
            }
        }

        /// <summary>
        /// Handles receiving data from the websocket server.
        /// </summary>
        /// <param name="content">The content received.</param>
        /// <param name="clientIndex">The index of the client the data was received from.</param>
        private void HandleDataReceived(string content, uint clientIndex)
        {
            Debug.WriteDebugLine(this, "Received: '{0}'", content);
            var cmds = Evands.Pellucid.Terminal.Commands.GlobalCommand.GetAllGlobalCommands();

            var command = cmds.FirstOrDefault(cmd => content.ToLower().StartsWith(cmd.Name.ToLower()));
            if (command != null)
            {
                Debug.WriteDebugLine(this, "Processing console command.");
                command.ExecuteCommand(content.Remove(0, command.Name.Length).Trim());
            }
            else
            {
                if (Validate())
                {
                    var response = string.Empty;
                    Crestron.SimplSharp.CrestronConsole.SendControlSystemCommand(content, ref response);
                    server.Send(!string.IsNullOrEmpty(response) ? response + "<br/>" : "Command Not Understood<br/>", clientIndex, true);
                }
            }
        }

        /// <summary>
        /// Gets the contents of a stream.
        /// </summary>
        /// <param name="stream">The stream to get the contents of.</param>
        /// <returns>The contents of the stream as a <see langword="string"/>.</returns>        
        private string GetStreamContents(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Validates that the route is still valid.
        /// </summary>
        /// <returns></returns>        
        private bool Validate()
        {
            return server != null;
        }
    }
}