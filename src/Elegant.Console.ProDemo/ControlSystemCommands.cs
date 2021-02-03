using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Elegant.Terminal.Commands.Attributes;
using Elegant.Terminal.Commands;
using Elegant.Diagnostics;
using Elegant.Terminal.Formatting;
using Elegant.Terminal;

namespace Elegant.Console.ProDemo
{
    [Command("cs", "Control system commands.")]
    public class ControlSystemCommands : TerminalCommandBase
    {
        [Verb("info", "Lists the details of the control system.")]
        public void Details()
        {
            Debug.WriteDebugLine(this, "Supports Audio = '{0}'", ControlSystem.Instance.SupportsAudio);
            Debug.WriteDebugLine(this, "Supports BACNet = '{0}'", ControlSystem.Instance.SupportsBACNet);
            Debug.WriteDebugLine(this, "Supports Bluetooth = '{0}'", ControlSystem.Instance.SupportsBluetooth);
            Debug.WriteDebugLine(this, "Supports Changing Video Resolution = '{0}'", ControlSystem.Instance.SupportsChangingVideoResolution);
            Debug.WriteDebugLine(this, "Supports ComPorts = '{0}'", ControlSystem.Instance.SupportsComPort);
            Debug.WriteDebugLine(this, "Supports Connect It Devices = '{0}'", ControlSystem.Instance.SupportsConnectItDevices);
            Debug.WriteDebugLine(this, "Supports Cresnet = '{0}'", ControlSystem.Instance.SupportsCresnet);
            Debug.WriteDebugLine(this, "Supports Digital Input = '{0}'", ControlSystem.Instance.SupportsDigitalInput);
            Debug.WriteDebugLine(this, "Supports Display Slot = '{0}'", ControlSystem.Instance.SupportsDisplaySlot);
            Debug.WriteDebugLine(this, "Supports ER Radio = '{0}'", ControlSystem.Instance.SupportsERRadio);
            Debug.WriteDebugLine(this, "Supports Ethernet = '{0}'", ControlSystem.Instance.SupportsEthernet);
            Debug.WriteDebugLine(this, "Supports Flash Projects = '{0}'", ControlSystem.Instance.SupportsFlashProjects);
            Debug.WriteDebugLine(this, "Supports Internal AirMedia = '{0}'", ControlSystem.Instance.SupportsInternalAirMedia);
            Debug.WriteDebugLine(this, "Supports Internal RF Gateway = '{0}'", ControlSystem.Instance.SupportsInternalRFGateway);
            Debug.WriteDebugLine(this, "Supports Internal Streaming = '{0}'", ControlSystem.Instance.SupportsInternalStreaming);
            Debug.WriteDebugLine(this, "Supports IR In = '{0}'", ControlSystem.Instance.SupportsIRIn);
            Debug.WriteDebugLine(this, "Supports IR Out = '{0}'", ControlSystem.Instance.SupportsIROut);
            Debug.WriteDebugLine(this, "Supports Microphones = '{0}'", ControlSystem.Instance.SupportsMicrophones);
            Debug.WriteDebugLine(this, "Supports Relay = '{0}'", ControlSystem.Instance.SupportsRelay);
            Debug.WriteDebugLine(this, "Supports SNMP = '{0}'", ControlSystem.Instance.SupportsSNMP);
            Debug.WriteDebugLine(this, "Supports Switcher Inputs'{0}'", ControlSystem.Instance.SupportsSwitcherInputs);
            Debug.WriteDebugLine(this, "Supports Switcher Outputs'{0}'", ControlSystem.Instance.SupportsSwitcherOutputs);
            Debug.WriteDebugLine(this, "Supports System Monitor'{0}'", ControlSystem.Instance.SupportsSystemMonitor);
            Debug.WriteDebugLine(this, "Supports 3-Series Plug-In Cards = '{0}'", ControlSystem.Instance.SupportsThreeSeriesPlugInCards);
            Debug.WriteDebugLine(this, "Supports USB HID = '{0}'", ControlSystem.Instance.SupportsUsbHid);
            Debug.WriteDebugLine(this, "Supports Versiport = '{0}'", ControlSystem.Instance.SupportsVersiport);
            Debug.WriteDebugLine(this, "Supports Switcher Inputs = '{0}'", ControlSystem.Instance.SwitcherInputs);
            Debug.WriteDebugLine(this, "Supports Switcher Outputs = '{0}'", ControlSystem.Instance.SwitcherOutputs);

            Debug.WriteLine(this, new ColorFormat(new RgbColor(255, 0x77, 0), new StandardColor()), "Testing RGB Colors");
        }
    }
}