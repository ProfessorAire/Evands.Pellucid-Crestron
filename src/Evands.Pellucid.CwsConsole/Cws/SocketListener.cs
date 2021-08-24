using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronSockets;
using System.Text.RegularExpressions;
using Evands.Pellucid.Diagnostics;

namespace Evands.Pellucid.Cws
{
    internal class SocketListener
    {
        private SecureTCPServer server;

        public SocketListener()
        {
        }

        public event Action<string, uint> DataReceived;

        public void Start(int port)
        {
            if (port < 1 || port > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException("port", "Must declare a valid port number.");
            }

            if (server != null)
            {
                server.DisconnectAll();
                server = null;
            }

            Debug.WriteDebugLine(this, "Starting server.");

            server = new Crestron.SimplSharp.CrestronSockets.SecureTCPServer(IPAddress.Any.ToString(), port, 1024, EthernetAdapterType.EthernetUnknownAdapter, 10);
            server.WaitForConnectionsAlways(ClientConnectionCallback);
        }

        public void Send(byte[] data, uint clientIndex, bool createFrame)
        {
            if (server != null
                && server.ClientConnected(clientIndex))
            {
                if (createFrame)
                {
                    data = CreateFrame(data);
                }

                server.SendData(clientIndex, data, data.Length);
                Debug.WriteDebugLine(this, "Data sent.");
            }
        }

        public void Send(string data, uint clientIndex, bool createFrame)
        {
            if (createFrame)
            {
                data = data.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            }

            Send(Encoding.UTF8.GetBytes(data), clientIndex, createFrame);
        }

        public void Send(byte[] data, bool createFrame)
        {
            if (server != null &&
                server.State == ServerState.SERVER_CONNECTED)
            {
                if (createFrame)
                {
                    data = CreateFrame(data);
                }

                server.SendData(data, data.Length);
                Debug.WriteDebugLine(this, "Data sent.");
            }
        }

        public void Send(string data, bool createFrame)
        {
            if (createFrame)
            {
                data = data.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
            }

            Send(Encoding.UTF8.GetBytes(data), createFrame);
        }

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
                Debug.WriteDebugLine(this, "Frame length is 4");
                Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data.Length)), 2, frame, 2, 2);
            }
            else if (frameLength == 10)
            {
                Debug.WriteDebugLine(this, "Frame length is 10");
                Array.Copy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)data.Length)), 0, frame, 2, 8);
            }

            Debug.WriteDebugLine(this, "Frame content length: '{0}'.", data.Length);

            return frame;
        }

        private void ClientConnectionCallback(SecureTCPServer server, uint clientIndex)
        {
            Debug.WriteDebugLine(this, "Client connected at index '{0}'.", clientIndex);
            if (server.GetIfDataAvailableForSpecificClient(clientIndex))
            {
                this.ReceiveDataCallback(server, clientIndex, server.GetIncomingDataBufferForSpecificClient(clientIndex).Length);
            }

            server.ReceiveDataAsync(clientIndex, ReceiveDataCallback);
        }

        private void ReceiveDataCallback(SecureTCPServer server, uint clientIndex, int count)
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
                Debug.WriteDebugLine(this, "Received '{0}' bytes.", dataBuffer.Length);
                bool fin = (dataBuffer[0] & 128) != 0;
                bool mask = (dataBuffer[1] & 128) != 0;
                int opcode = dataBuffer[0] & 15;
                ulong dataLength = (ulong)(dataBuffer[1] - 128);
                ulong offset = 2;

                if (opcode == 0x0)
                {
                    Debug.WriteDebugLine(this, "Received continuation frame opcode.");
                }
                else if (opcode == 0x1)
                {
                    Debug.WriteDebugLine(this, "Received text frame opcode.");
                }
                else if (opcode == 0x2)
                {
                    Debug.WriteDebugLine(this, "Received binary frame opcode.");
                }
                else if (opcode >= 0x3 && opcode <= 0x7)
                {
                    Debug.WriteDebugLine(this, "Received reserved non-control opcode.");
                }
                else if (opcode == 0x8)
                {
                    server.Disconnect(clientIndex);
                    this.Send(new byte[]
                    {
                        BitConverter.GetBytes(0x88)[0],
                        BitConverter.GetBytes(0x00)[0]
                    }, clientIndex, false);
                    Debug.WriteDebugLine(this, "Received disconnect OpCode. Disconnected.");
                    return;
                }
                else if (opcode == 0x9)
                {
                    this.Send(new byte[]
                    {
                        BitConverter.GetBytes(0x8A)[0],
                        BitConverter.GetBytes(0x00)[0]
                    }, clientIndex, false);
                    Debug.WriteDebugLine(this, "Received Ping and sent a Pong.");

                    if (server.ClientConnected(clientIndex))
                    {
                        server.ReceiveDataAsync(clientIndex, ReceiveDataCallback);
                    }

                    return;
                }
                else if (opcode == 0xA)
                {
                    Debug.WriteDebugLine(this, "Received Pong.");
                    if (server.ClientConnected(clientIndex))
                    {
                        server.ReceiveDataAsync(clientIndex, ReceiveDataCallback);
                    }

                    return;
                }
                else if (opcode >= 0xB && opcode <= 0xF)
                {
                    Debug.WriteDebugLine(this, "Received reserved control frame opcode.");
                }

                if (dataLength == 126)
                {
                    Debug.WriteDebugLine(this, "Data length is 126.");
                    dataLength = BitConverter.ToUInt16(new byte[] { dataBuffer[3], dataBuffer[2] }, 0);
                    offset = 4;
                }
                else if (dataLength == 127)
                {
                    Debug.WriteDebugLine(this, "Data length is 127.");
                    dataLength = BitConverter.ToUInt64(new byte[] { dataBuffer[9], dataBuffer[8], dataBuffer[7], dataBuffer[6], dataBuffer[5], dataBuffer[4], dataBuffer[3], dataBuffer[2] }, 0);
                    offset = 10;
                }

                if (dataLength == 0)
                {
                    Debug.WriteDebugLine(this, "Data length was zero.");
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
                    Debug.WriteDebugLine(this, "Mask bit not set. Invalid message received.");
                }
            }

            if (server.ClientConnected(clientIndex))
            {
                server.ReceiveDataAsync(clientIndex, ReceiveDataCallback);
            }
        }
    }
}