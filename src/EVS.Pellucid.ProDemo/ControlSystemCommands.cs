#region copyright
// <copyright file="ControlSystemCommands.cs" company="Christopher McNeely">
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

using EVS.Pellucid.Diagnostics;
using EVS.Pellucid.Terminal.Commands;
using EVS.Pellucid.Terminal.Commands.Attributes;
using EVS.Pellucid.Terminal.Formatting;

namespace EVS.Pellucid.ProDemo
{
    /// <summary>
    /// Commands for interacting with the control system.
    /// </summary>
    [Command("cs", "Control system commands.")]
    public class ControlSystemCommands : TerminalCommandBase
    {
        /// <summary>
        /// Gets the details of the control system.
        /// </summary>
        [Verb("info", "Lists the details of the control system.")]
        public void Details()
        {
            ProConsole.WriteLine();
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Audio = '{0}'", ControlSystem.Instance.SupportsAudio);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports BACNet = '{0}'", ControlSystem.Instance.SupportsBACNet);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Bluetooth = '{0}'", ControlSystem.Instance.SupportsBluetooth);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Changing Video Resolution = '{0}'", ControlSystem.Instance.SupportsChangingVideoResolution);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports ComPorts = '{0}'", ControlSystem.Instance.SupportsComPort);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Connect It Devices = '{0}'", ControlSystem.Instance.SupportsConnectItDevices);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Cresnet = '{0}'", ControlSystem.Instance.SupportsCresnet);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Digital Input = '{0}'", ControlSystem.Instance.SupportsDigitalInput);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Display Slot = '{0}'", ControlSystem.Instance.SupportsDisplaySlot);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports ER Radio = '{0}'", ControlSystem.Instance.SupportsERRadio);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Ethernet = '{0}'", ControlSystem.Instance.SupportsEthernet);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Flash Projects = '{0}'", ControlSystem.Instance.SupportsFlashProjects);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Internal AirMedia = '{0}'", ControlSystem.Instance.SupportsInternalAirMedia);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Internal RF Gateway = '{0}'", ControlSystem.Instance.SupportsInternalRFGateway);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Internal Streaming = '{0}'", ControlSystem.Instance.SupportsInternalStreaming);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports IR In = '{0}'", ControlSystem.Instance.SupportsIRIn);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports IR Out = '{0}'", ControlSystem.Instance.SupportsIROut);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Microphones = '{0}'", ControlSystem.Instance.SupportsMicrophones);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Relay = '{0}'", ControlSystem.Instance.SupportsRelay);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports SNMP = '{0}'", ControlSystem.Instance.SupportsSNMP);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Switcher Inputs = '{0}'", ControlSystem.Instance.SupportsSwitcherInputs);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Switcher Outputs = '{0}'", ControlSystem.Instance.SupportsSwitcherOutputs);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports System Monitor = '{0}'", ControlSystem.Instance.SupportsSystemMonitor);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports 3-Series Plug-In Cards = '{0}'", ControlSystem.Instance.SupportsThreeSeriesPlugInCards);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports USB HID = '{0}'", ControlSystem.Instance.SupportsUsbHid);
            Debug.WriteDebugLine(ControlSystem.Instance, "Supports Versiport = '{0}'", ControlSystem.Instance.SupportsVersiport);

            Debug.WriteLine(ControlSystem.Instance, new ColorFormat(new RgbColor(255, 0x77, 0), new StandardColor()), "Testing RGB Colors");
        }
    }
}