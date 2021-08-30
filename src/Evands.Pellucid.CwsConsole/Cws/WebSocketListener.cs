#region copyright
// <copyright file="WebSocketListener.cs" company="Christopher McNeely">
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;
using Evands.Pellucid.Diagnostics;

namespace Evands.Pellucid.Cws
{
    /// <summary>
    /// WebSocket server for CWS console writer.
    /// <para>This does not attempt to implement the full websocket spec, so
    /// it should not be expected to function for any websocket implementation.</para>
    /// </summary>
    internal class WebSocketListener : IDisposable
    {
        /// <summary>
        /// The server used for secure implementations.
        /// </summary>        
        private SecureTCPServer secureTcpServer;

        /// <summary>
        /// The server used for insecure implementations.
        /// </summary>        
        private TCPServer tcpServer;

        /// <summary>
        /// Indicates whether or not the secure TCP server should be used.
        /// </summary>        
        private bool useSecureServer = false;

        /// <summary>
        /// Indicates whether or not the server has been requested to start.
        /// </summary>        
        private bool startRequested;

        /// <summary>
        /// A list for maintaining knowledge of the currently connected sockets.
        /// </summary>        
        private List<uint> connectedSocketIndexes = new List<uint>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketListener"/> class.
        /// </summary>
        /// <param name="useSecureServer"></param>        
        public WebSocketListener(bool useSecureServer)
        {
            this.useSecureServer = useSecureServer;
        }

        /// <summary>
        /// Raised when data is received on the websocket.
        /// <para>The <see langword="string"/> parameter is the data received.</para>
        /// <para>The <see langword="uint"/> parameter is the index of the client the data was received from.</para>
        /// </summary>        
        public event Action<string, uint> DataReceived;

        /// <summary>
        /// Attempts to start the websocket server.
        /// </summary>
        /// <param name="port">The port the websocket server should be started using.</param>        
        public void Start(int port)
        {
            if (port < 1 || port > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException("port", "Must declare a valid port number.");
            }

            startRequested = false;

            if (useSecureServer)
            {
                this.StartSecureServer(port);
            }
            else
            {
                this.StartInsecureServer(port);
            }

            startRequested = true;
        }

        /// <summary>
        /// Attempts to send the specified data to the client at the provided index.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="clientIndex">The index of the client to send the data to.</param>
        /// <param name="createFrame">A value indicating whether to create a websocket frame to wrap the data in.</param>        
        public void Send(byte[] data, uint clientIndex, bool createFrame)
        {
            if (!useSecureServer)
            {
                if (tcpServer != null &&
                    (tcpServer.State & ServerState.SERVER_CONNECTED) == ServerState.SERVER_CONNECTED)
                {
                    if (createFrame)
                    {
                        data = CreateFrame(data);
                    }

                    tcpServer.SendData(clientIndex, data, data.Length);
                }
            }
            else
            {
                if (secureTcpServer != null &&
                    (secureTcpServer.State & ServerState.SERVER_CONNECTED) == ServerState.SERVER_CONNECTED)
                {
                    if (createFrame)
                    {
                        data = CreateFrame(data);
                    }

                    secureTcpServer.SendData(clientIndex, data, data.Length);
                }
            }
        }

        /// <summary>
        /// Attempts to send the specified data to the client at the provided index.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="clientIndex">The index of the client to send the data to.</param>
        /// <param name="createFrame">A value indicating whether to create a websocket frame to wrap the data in.   
        public void Send(string data, uint clientIndex, bool createFrame)
        {
            if (createFrame)
            {
                data = data.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            }

            Send(Encoding.UTF8.GetBytes(data), clientIndex, createFrame);
        }

        /// <summary>
        /// Attempts to send the specified data to all the connected clients.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="createFrame">A value indicating whether to create a websocket frame to wrap the data in.   
        public void Send(byte[] data, bool createFrame)
        {
            if (!useSecureServer)
            {
                if (tcpServer != null &&
                    (tcpServer.State & ServerState.SERVER_CONNECTED) == ServerState.SERVER_CONNECTED)
                {
                    if (createFrame)
                    {
                        data = CreateFrame(data);
                    }

                    var sockets = connectedSocketIndexes.Distinct().ToArray();

                    for (uint i = 0; i < sockets.Length; i++)
                    {
                        CrestronConsole.PrintLine("Sending data to client at index '{0}'", sockets[i]);
                        tcpServer.SendData(sockets[i], data, data.Length);
                    }
                }
            }
            else
            {
                if (secureTcpServer != null &&
                    (secureTcpServer.State & ServerState.SERVER_CONNECTED) == ServerState.SERVER_CONNECTED)
                {
                    if (createFrame)
                    {
                        data = CreateFrame(data);
                    }

                    var sockets = connectedSocketIndexes.Distinct().ToArray();

                    for (uint i = 0; i < sockets.Length; i++)
                    {
                        CrestronConsole.PrintLine("Sending data to client at index '{0}'", sockets[i]);
                        secureTcpServer.SendData(sockets[i], data, data.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to send the specified data to all the connected clients.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <param name="createFrame">A value indicating whether to create a websocket frame to wrap the data in.       
        public void Send(string data, bool createFrame)
        {
            if (createFrame)
            {
                data = data.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            }

            Send(Encoding.UTF8.GetBytes(data), createFrame);
        }

        /// <summary>
        /// Creates a websocket frame and wraps the provided data in it.
        /// </summary>
        /// <param name="data">The data to wrap in the frame.</param>
        /// <returns>An array of <see langword="byte"/> values.</returns>        
        private byte[] CreateFrame(byte[] data)
        {
            var frameLength = data.Length < 126 ? 2 :
                              data.Length <= ushort.MaxValue ? 4 : 10;

            ushort payloadLengthRepresentation = data.Length < 126 ? (ushort)data.Length :
                data.Length <= ushort.MaxValue ? (ushort)126 : (ushort)127;

            var frame = new byte[data.Length + frameLength];
            Array.Copy(data, 0, frame, frameLength, data.Length);
            frame[0] = BitConverter.GetBytes(0x81)[0];
            frame[1] = BitConverter.GetBytes(payloadLengthRepresentation)[0];

            if (frameLength == 4)
            {
                Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length)), 2, frame, 2, 2);
            }
            else if (frameLength == 10)
            {
                Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)data.Length)), 0, frame, 2, 8);
            }

            return frame;
        }

        /// <summary>
        /// The client connection callback for the secure server.
        /// </summary>
        /// <param name="server">The server the callback is executed on.</param>
        /// <param name="clientIndex">The index of the connecting client.</param>        
        private void SecureClientConnectionCallback(SecureTCPServer server, uint clientIndex)
        {
            if (clientIndex > 0)
            {
                CrestronConsole.PrintLine("Client connected at index '{0}'.", clientIndex);

                if (!connectedSocketIndexes.Contains(clientIndex))
                {
                    connectedSocketIndexes.Add(clientIndex);
                }

                server.ReceiveDataAsync(clientIndex, SecureReceiveDataCallback);
            }
        }

        /// <summary>
        /// The callback for receiving data from the secure server client.
        /// </summary>
        /// <param name="server">The server the callback is executed on.</param>
        /// <param name="clientIndex">The index of the client data is received from.</param>
        /// <param name="count">The number of bytes of data received.</param>        
        private void SecureReceiveDataCallback(SecureTCPServer server, uint clientIndex, int count)
        {
            if (count <= 0)
            {
                if (server.ClientConnected(clientIndex))
                {
                    server.ReceiveDataAsync(clientIndex, SecureReceiveDataCallback);
                }

                return;
            }

            var dataBuffer = server.GetIncomingDataBufferForSpecificClient(clientIndex);
            var data = Encoding.UTF8.GetString(dataBuffer, 0, count);
            if (Regex.IsMatch(data, "^GET", RegexOptions.IgnoreCase))
            {
                var key = Regex.Match(data, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
                key = key + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                byte[] keySha1 = Crestron.SimplSharp.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
                string sha64 = Convert.ToBase64String(keySha1);

                var responseMessage = "HTTP/1.1 101 Switching Protocols\r\n" +
                "Connection: Upgrade\r\n" +
                "Upgrade: websocket\r\n" +
                "Sec-WebSocket-Accept: " + sha64 + "\r\n\r\n";

                this.Send(responseMessage, clientIndex, false);
            }
            else
            {
                CrestronConsole.PrintLine("Received '{0}' bytes.", count);
                bool fin = (dataBuffer[0] & 128) != 0;
                bool mask = (dataBuffer[1] & 128) != 0;
                int opcode = dataBuffer[0] & 15;
                ulong dataLength = (ulong)(dataBuffer[1] - 128);
                ulong offset = 2;

                if (opcode == 0x0)
                {
                    CrestronConsole.PrintLine("Received continuation frame opcode.");
                }
                else if (opcode == 0x1)
                {
                    CrestronConsole.PrintLine("Received text frame opcode.");
                }
                else if (opcode == 0x2)
                {
                    CrestronConsole.PrintLine("Received binary frame opcode.");
                }
                else if (opcode >= 0x3 && opcode <= 0x7)
                {
                    CrestronConsole.PrintLine("Received reserved non-control opcode.");
                }
                else if (opcode == 0x8)
                {
                    this.Send(new byte[]
                    {
                        BitConverter.GetBytes(0x88)[0],
                        BitConverter.GetBytes(0x00)[0]
                    }, clientIndex, false);
                    server.Disconnect(clientIndex);
                    CrestronConsole.PrintLine("Received disconnect OpCode. Disconnected.");
                    return;
                }
                else if (opcode == 0x9)
                {
                    this.Send(new byte[]
                    {
                        BitConverter.GetBytes(0x8A)[0],
                        BitConverter.GetBytes(0x00)[0]
                    }, clientIndex, false);
                    CrestronConsole.PrintLine("Received Ping and sent a Pong.");

                    if (server.ClientConnected(clientIndex))
                    {
                        server.ReceiveDataAsync(clientIndex, SecureReceiveDataCallback);
                    }

                    return;
                }
                else if (opcode == 0xA)
                {
                    CrestronConsole.PrintLine("Received Pong.");
                    if (server.ClientConnected(clientIndex))
                    {
                        server.ReceiveDataAsync(clientIndex, SecureReceiveDataCallback);
                    }

                    return;
                }
                else if (opcode >= 0xB && opcode <= 0xF)
                {
                    CrestronConsole.PrintLine("Received reserved control frame opcode.");
                }

                if (dataLength == 126)
                {
                    CrestronConsole.PrintLine("Data length is 126.");
                    dataLength = BitConverter.ToUInt16(new byte[] { dataBuffer[3], dataBuffer[2] }, 0);
                    offset = 4;
                }
                else if (dataLength == 127)
                {
                    CrestronConsole.PrintLine("Data length is 127.");
                    dataLength = BitConverter.ToUInt64(new byte[] { dataBuffer[9], dataBuffer[8], dataBuffer[7], dataBuffer[6], dataBuffer[5], dataBuffer[4], dataBuffer[3], dataBuffer[2] }, 0);
                    offset = 10;
                }

                if (dataLength == 0)
                {
                    CrestronConsole.PrintLine("Data length was zero.");
                    this.Send(string.Format("{0}>", InitialParametersClass.ControllerPromptName), clientIndex, true);
                }
                else if (mask)
                {
                    byte[] de = new byte[dataLength];
                    byte[] mk = new byte[4] { dataBuffer[offset], dataBuffer[offset + 1], dataBuffer[offset + 2], dataBuffer[offset + 3] };
                    offset += 4;

                    for (ulong i = 0; i < dataLength; i++)
                    {
                        de[i] = (byte)(dataBuffer[offset + i] ^ mk[i % 4]);
                    }

                    var text = Encoding.UTF8.GetString(de, 0, de.Length);
                    var dr = DataReceived;
                    if (dr != null)
                    {
                        dr.Invoke(text, clientIndex);
                    }
                }
                else
                {
                    CrestronConsole.PrintLine("Data length was '{0}', but mask bit not set. Invalid message received.", dataLength);
                    var dbg = string.Empty;
                    for (var i = 0; i < count; i++)
                    {
                        dbg += string.Format("[{0}]", Convert.ToString(dataBuffer[i], 16));
                    }

                    Debug.WriteDebugLine(this, dbg);
                }
            }

            if (server.ClientConnected(clientIndex))
            {
                server.ReceiveDataAsync(clientIndex, SecureReceiveDataCallback);
            }
        }

        /// <summary>
        /// The client connection callback for the insecure server.
        /// </summary>
        /// <param name="server">The server the callback is executed on.</param>
        /// <param name="clientIndex">The index of the connecting client.</param>  
        private void ClientConnectionCallback(TCPServer server, uint clientIndex)
        {
            if (clientIndex > 0)
            {
                CrestronConsole.PrintLine("Client connected at index '{0}'.", clientIndex);
                if (!connectedSocketIndexes.Contains(clientIndex))
                {
                    connectedSocketIndexes.Add(clientIndex);
                }

                if (server.GetIfDataAvailableForSpecificClient(clientIndex))
                {
                    ReceiveDataCallback(server, clientIndex, server.GetIncomingDataBufferForSpecificClient(clientIndex).Length);
                }
                else
                {
                    server.ReceiveDataAsync(clientIndex, ReceiveDataCallback);
                }
            }
        }

        /// <summary>
        /// The callback for receiving data from the insecure server client.
        /// </summary>
        /// <param name="server">The server the callback is executed on.</param>
        /// <param name="clientIndex">The index of the client data is received from.</param>
        /// <param name="count">The number of bytes of data received.</param>
        private void ReceiveDataCallback(TCPServer server, uint clientIndex, int count)
        {
            if (count <= 0)
            {
                if (server.ClientConnected(clientIndex))
                {
                    server.ReceiveDataAsync(clientIndex, ReceiveDataCallback);
                }

                return;
            }

            var dataBuffer = server.GetIncomingDataBufferForSpecificClient(clientIndex);
            var data = Encoding.UTF8.GetString(dataBuffer, 0, count);
            if (Regex.IsMatch(data, "^GET", RegexOptions.IgnoreCase))
            {
                var key = Regex.Match(data, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
                key = key + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                byte[] keySha1 = Crestron.SimplSharp.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
                string sha64 = Convert.ToBase64String(keySha1);

                var responseMessage = "HTTP/1.1 101 Switching Protocols\r\n" +
                "Connection: Upgrade\r\n" +
                "Upgrade: websocket\r\n" +
                "Sec-WebSocket-Accept: " + sha64 + "\r\n\r\n";

                this.Send(responseMessage, clientIndex, false);
            }
            else
            {
                CrestronConsole.PrintLine("Received '{0}' bytes.", count);
                bool fin = (dataBuffer[0] & 128) != 0;
                bool mask = (dataBuffer[1] & 128) != 0;
                int opcode = dataBuffer[0] & 15;
                ulong dataLength = (ulong)(dataBuffer[1] - 128);
                ulong offset = 2;

                if (opcode == 0x0)
                {
                    CrestronConsole.PrintLine("Received continuation frame opcode.");
                }
                else if (opcode == 0x1)
                {
                    CrestronConsole.PrintLine("Received text frame opcode.");
                }
                else if (opcode == 0x2)
                {
                    CrestronConsole.PrintLine("Received binary frame opcode.");
                }
                else if (opcode >= 0x3 && opcode <= 0x7)
                {
                    CrestronConsole.PrintLine("Received reserved non-control opcode.");
                }
                else if (opcode == 0x8)
                {
                    this.Send(new byte[]
                    {
                        BitConverter.GetBytes(0x88)[0],
                        BitConverter.GetBytes(0x00)[0]
                    }, clientIndex, false);
                    server.Disconnect(clientIndex);
                    CrestronConsole.PrintLine("Received disconnect OpCode. Disconnected.");
                    return;
                }
                else if (opcode == 0x9)
                {
                    this.Send(new byte[]
                    {
                        BitConverter.GetBytes(0x8A)[0],
                        BitConverter.GetBytes(0x00)[0]
                    }, clientIndex, false);
                    CrestronConsole.PrintLine("Received Ping and sent a Pong.");

                    if (server.ClientConnected(clientIndex))
                    {
                        server.ReceiveDataAsync(clientIndex, ReceiveDataCallback);
                    }

                    return;
                }
                else if (opcode == 0xA)
                {
                    CrestronConsole.PrintLine("Received Pong.");
                    if (server.ClientConnected(clientIndex))
                    {
                        server.ReceiveDataAsync(clientIndex, ReceiveDataCallback);
                    }

                    return;
                }
                else if (opcode >= 0xB && opcode <= 0xF)
                {
                    CrestronConsole.PrintLine("Received reserved control frame opcode.");
                }

                if (dataLength == 126)
                {
                    CrestronConsole.PrintLine("Data length is 126.");
                    dataLength = BitConverter.ToUInt16(new byte[] { dataBuffer[3], dataBuffer[2] }, 0);
                    offset = 4;
                }
                else if (dataLength == 127)
                {
                    CrestronConsole.PrintLine("Data length is 127.");
                    dataLength = BitConverter.ToUInt64(new byte[] { dataBuffer[9], dataBuffer[8], dataBuffer[7], dataBuffer[6], dataBuffer[5], dataBuffer[4], dataBuffer[3], dataBuffer[2] }, 0);
                    offset = 10;
                }

                if (dataLength == 0)
                {
                    CrestronConsole.PrintLine("Data length was zero.");
                    this.Send(string.Format("{0}>", InitialParametersClass.ControllerPromptName), clientIndex, true);
                }
                else if (mask)
                {
                    byte[] de = new byte[dataLength];
                    byte[] mk = new byte[4] { dataBuffer[offset], dataBuffer[offset + 1], dataBuffer[offset + 2], dataBuffer[offset + 3] };
                    offset += 4;

                    for (ulong i = 0; i < dataLength; i++)
                    {
                        de[i] = (byte)(dataBuffer[offset + i] ^ mk[i % 4]);
                    }

                    var text = Encoding.UTF8.GetString(de, 0, de.Length);
                    var dr = DataReceived;
                    if (dr != null)
                    {
                        dr.Invoke(text, clientIndex);
                    }
                }
                else
                {
                    CrestronConsole.PrintLine("Data length was '{0}', but mask bit not set. Invalid message received.", dataLength);
                }
            }

            if (server.ClientConnected(clientIndex))
            {
                server.ReceiveDataAsync(clientIndex, ReceiveDataCallback);
            }
        }

        /// <summary>
        /// Starts the insecure server.
        /// </summary>
        /// <param name="port">The port the server should be started on.</param>        
        private void StartInsecureServer(int port)
        {
            if (tcpServer != null)
            {
                tcpServer.SocketStatusChange -= InsecureSocketStatusChange;
                tcpServer.DisconnectAll();
                connectedSocketIndexes.Clear();
                tcpServer = null;
            }

            CrestronConsole.PrintLine("Starting server.");

            tcpServer = new TCPServer(IPAddress.Any.ToString(), port, 1024, EthernetAdapterType.EthernetUnknownAdapter, 10);
            tcpServer.SocketStatusChange += InsecureSocketStatusChange;
            tcpServer.WaitForConnectionsAlways(ClientConnectionCallback);
        }

        /// <summary>
        /// Starts the secure server.
        /// </summary>
        /// <param name="port">The port the server should be started on.</param>        
        private void StartSecureServer(int port)
        {
            if (secureTcpServer != null)
            {
                secureTcpServer.SocketStatusChange -= SecureSocketStatusChange;
                secureTcpServer.DisconnectAll();
                connectedSocketIndexes.Clear();
                secureTcpServer = null;
            }

            CrestronConsole.PrintLine("Starting secure server.");

            secureTcpServer = new SecureTCPServer(IPAddress.Any.ToString(), port, 1024, EthernetAdapterType.EthernetUnknownAdapter, 10);
            secureTcpServer.SocketStatusChange += SecureSocketStatusChange;
            secureTcpServer.WaitForConnectionsAlways(SecureClientConnectionCallback);
        }

        /// <summary>
        /// Handles socket status changes for the secure server.
        /// </summary>
        /// <param name="server">The server that had a socket change event.</param>
        /// <param name="clientIndex">The index of the client who's socket connection changed.</param>
        /// <param name="serverSocketStatus">The new socket status.</param>        
        void SecureSocketStatusChange(SecureTCPServer server, uint clientIndex, SocketStatus serverSocketStatus)
        {
            if (serverSocketStatus != SocketStatus.SOCKET_STATUS_CONNECTED)
            {
                connectedSocketIndexes.Remove(clientIndex);
            }

            if (serverSocketStatus == SocketStatus.SOCKET_STATUS_BROKEN_LOCALLY ||
                serverSocketStatus == SocketStatus.SOCKET_STATUS_BROKEN_REMOTELY)
            {
                if (startRequested)
                {
                    server.WaitForConnectionsAlways(SecureClientConnectionCallback);
                }
            }
        }

        /// <summary>
        /// Handles socket status changes for the insecure server.
        /// </summary>
        /// <param name="server">The server that had a socket change event.</param>
        /// <param name="clientIndex">The index of the client who's socket connection changed.</param>
        /// <param name="serverSocketStatus">The new socket status.</param> 
        void InsecureSocketStatusChange(TCPServer server, uint clientIndex, SocketStatus serverSocketStatus)
        {
            if (serverSocketStatus == SocketStatus.SOCKET_STATUS_BROKEN_LOCALLY ||
                serverSocketStatus == SocketStatus.SOCKET_STATUS_BROKEN_REMOTELY)
            {
                if (startRequested)
                {
                    server.WaitForConnectionsAlways(ClientConnectionCallback);
                }
            }
        }

        /// <summary>
        /// Disposes of resources.
        /// </summary>        
        public void Dispose()
        {
            if (tcpServer != null)
            {
                tcpServer.Stop();
                tcpServer.DisconnectAll();
                tcpServer = null;
            }

            if (secureTcpServer != null)
            {
                secureTcpServer.Stop();
                secureTcpServer.DisconnectAll();
                secureTcpServer = null;
            }
        }
    }
}