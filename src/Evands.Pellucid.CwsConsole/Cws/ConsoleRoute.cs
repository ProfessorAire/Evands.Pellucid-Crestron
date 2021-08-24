using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.WebScripting;
using Crestron.SimplSharp.CrestronIO;
using Evands.Pellucid.Terminal;
using Evands.Pellucid.Diagnostics;

namespace Evands.Pellucid.Cws
{
    internal class ConsoleRoute : IHttpCwsHandler, IConsoleWriter, IDebugData
    {
        private SocketListener server;

        private string socketPort = string.Empty;

        public ConsoleRoute(int port)
        {
            server = new SocketListener();
            server.DataReceived += HandleDataReceived;
            socketPort = port.ToString();
            server.Start(port);
        }

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
                html = html.Replace("{{ CSS }}", css).Replace("{{ PORT }}", socketPort).Replace("{{ HOST }}", host);
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

        private string GetStreamContents(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private bool Validate()
        {
            return server != null;
        }

        private string prompt = InitialParametersClass.ControllerPromptName + ">";

        public void Write(string message, params object[] args)
        {
            if (Validate())
            {
                server.Send(string.Format(message.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"), args), true);
            }
        }

        public void Write(string message)
        {
            if (Validate())
            {
                server.Send(message.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"), true);
            }
        }

        public void WriteLine(string message, params object[] args)
        {
            if (Validate())
            {
                server.Send(string.Format(message.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>") + "<br/>", args), true);
                server.Send(prompt + "<br/>", true);
            }
        }

        public void WriteLine(string message)
        {
            if (Validate())
            {
                server.Send(message.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>") + "<br/>", true);
                server.Send(prompt + "<br/>", true);
            }
        }

        public void WriteLine()
        {
            if (Validate())
            {
                server.Send("<br/>" + prompt + "<br/>", true);
            }
        }

        public void WriteCommandResponse(string message, params object[] args)
        {
            this.Write(message, args);
        }

        public void WriteCommandResponse(string message)
        {
            this.Write(message);
        }

        public string Header
        {
            get
            {
                return "ConsoleRoute";
            }
        }

        public Evands.Pellucid.Terminal.Formatting.ColorFormat HeaderColor
        {
            get { return ConsoleBase.Colors.BrightCyan; }
        }
    }
}