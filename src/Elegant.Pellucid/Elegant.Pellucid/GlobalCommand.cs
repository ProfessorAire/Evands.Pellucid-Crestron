#region copyright
// <copyright file="GlobalCommand.cs" company="Christopher McNeely">
// The MIT License (MIT)
// Copyright (c) Christopher McNeely
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

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
using Crestron.SimplSharp.Reflection;
using Elegant.Pellucid.Attributes;

namespace Elegant.Pellucid
{
    /// <summary>
    /// Global console command that is registered with the CrestronConsole on startup.
    /// <para>Without a global command registered with the console no commands will be available to execute.</para>
    /// </summary>
    public class GlobalCommand
    {
        private static Regex commandRegex = new Regex("^(?<name>\\w+){1} ?(?<verb>[^\\r\\n\"\\- ]+)? ?\"?(?<defaultValue>[^\\r\\n\"-]+)?\"? ?(?<parameters>.*)?");

        private static Regex parametersRegex = new Regex("-{1,2}(?:(?<name>\\w+)(?: +\"?(?<value>[^-\\r\\n\"]*))?\"?)");

        private Dictionary<string, TerminalCommandBase> commands = new Dictionary<string, TerminalCommandBase>();

        private Action<string> writeErrorMethod;

        private Action<string> writeHelpMethod;

        private Func<string, string> formatHelpCommandMethod;

        private Func<string, string> formatHelpVerbMethod;

        private Func<string, string> formatHelpOperandMethod;

        private Func<string, string> formatHelpFlagMethod;

        private Func<string, string> formatHelpSampleMethod;

        private Func<string, string> formatHelpTextMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command, as it should appear in the processor console. Limited to 23 characters.</param>
        /// <param name="help">Help text shown when entering 'help user' on a Crestron processor command prompt. Limited to 79 characters.</param>
        /// <param name="access">The user access level the command should be given.</param>
        public GlobalCommand(string name, string help, CommandAccess access)
        {
            if (name.Length >= 23)
            {
                throw new ArgumentOutOfRangeException("name", "The name can be no longer than 23 characters.");
            }

            Name = name;
            Help = help.Substring(0, Math.Min(help.Length, 76));
            if (Help.Length == 76)
            {
                Help = Help + "...";
            }

            CommandAccess = access;
            Action<string> writer = (msg) => CrestronConsole.ConsoleCommandResponse(msg);
            Func<string, string> formatter = (msg) => msg;
            WriteErrorMethod = writer;
            WriteHelpMethod = writer;
            FormatHelpCommandMethod = formatter;
            FormatHelpVerbMethod = formatter;
            FormatHelpOperandMethod = formatter;
            FormatHelpFlagMethod = formatter;
            FormatHelpSampleMethod = formatter;
            FormatHelpTextMethod = formatter;
        }

        /// <summary>
        /// Raised when an exception is encountered by a command.
        /// <para>If this is null when an exception is encountered the exception will be rethrown.</para>
        /// </summary>
        public event EventHandler<TerminalCommandExceptionEventArgs> CommandExceptionEncountered;

        /// <summary>
        /// Gets the name associated with the command.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the help associated with the command.
        /// </summary>
        public string Help { get; private set; }

        /// <summary>
        /// Gets or sets the method used to write error messages to the console.
        /// </summary>
        public Action<string> WriteErrorMethod
        {
            get { return writeErrorMethod; }

            set { writeErrorMethod = ValidateWriter(value); }
        }

        /// <summary>
        /// Gets or sets the method used to write help messages to the console.
        /// </summary>
        public Action<string> WriteHelpMethod
        {
            get { return writeHelpMethod; }

            set { writeHelpMethod = ValidateWriter(value); }
        }

        /// <summary>
        /// Gets or sets the method used to format the names of commands in help text.
        /// </summary>
        public Func<string, string> FormatHelpCommandMethod
        {
            get { return formatHelpCommandMethod; }

            set { formatHelpCommandMethod = ValidateFormatter(value); }
        }

        /// <summary>
        /// Gets or sets the method used to format the names of verbs in help text.
        /// </summary>
        public Func<string, string> FormatHelpVerbMethod
        {
            get { return formatHelpVerbMethod; }

            set { formatHelpVerbMethod = ValidateFormatter(value); }
        }

        /// <summary>
        /// Gets or sets the method used to format the names of operands in help text.
        /// </summary>
        public Func<string, string> FormatHelpOperandMethod
        {
            get { return formatHelpOperandMethod; }

            set { formatHelpOperandMethod = ValidateFormatter(value); }
        }

        /// <summary>
        /// Gets or sets the method used to format the names of flags in help text.
        /// </summary>
        public Func<string, string> FormatHelpFlagMethod
        {
            get { return formatHelpFlagMethod; }

            set { formatHelpFlagMethod = ValidateFormatter(value); }
        }

        /// <summary>
        /// Gets or sets the method used to format samples in help text.
        /// </summary>
        public Func<string, string> FormatHelpSampleMethod
        {
            get { return formatHelpSampleMethod; }

            set { formatHelpSampleMethod = ValidateFormatter(value); }
        }

        /// <summary>
        /// Gets or sets the method used to format text with no other formatter in help text.
        /// </summary>
        public Func<string, string> FormatHelpTextMethod
        {
            get { return formatHelpTextMethod; }

            set { formatHelpTextMethod = ValidateFormatter(value); }
        }

        /// <summary>
        /// Gets the user access level this command will have on the console.
        /// </summary>
        public CommandAccess CommandAccess { get; private set; }

        /// <summary>
        /// Attempts to add the command to the console.
        /// <para>This must be done in the Control System Constructor.</para>
        /// </summary>
        /// <returns>True if it succeeds, false if it fails for any reason.</returns>
        public bool AddToConsole()
        {
            ConsoleAccessLevelEnum level = (ConsoleAccessLevelEnum)CommandAccess;
            return CrestronConsole.AddNewConsoleCommand(ExecuteCommand, Name, Help, level);
        }

        /// <summary>
        /// Attempts to remove the command from the console.
        /// </summary>
        /// <returns>True if it succeeds, false if it fails for any reason.</returns>
        public bool RemoveFromConsole()
        {
            return CrestronConsole.RemoveConsoleCommand(Name);
        }

        /// <summary>
        /// Add a new terminal command to the global command.
        /// <para>Unlike the Global commands, these can be added at any point in program execution.</para>
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to add to the global command's handler.</param>
        /// <returns>The result of the registration attempt as a <see cref="RegisterResult"/>.</returns>
        public RegisterResult AddCommand(TerminalCommandBase command)
        {
            var name = CommandHelpers.GetCommandNameFromAttribute(command);

            if (string.IsNullOrEmpty(name))
            {
                return RegisterResult.NoCommandAttributeFound;
            }

            return AddCommand(command, name);
        }

        /// <summary>
        /// Add a new terminal command to the global command.
        /// <para>Unlike the Global commands, these can be added at any point in program execution.</para>
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to add to the global command's handler.</param>
        /// <param name="name">The name to add the command using. If this is specified the class's <see cref="Attributes.CommandAttribute.Name"/> value will not be used.</param>
        public RegisterResult AddCommand(TerminalCommandBase command, string name)
        {
            RegisterResult result;

            if (!commands.ContainsKey(name.ToLower()))
            {
                commands.Add(name.ToLower(), command);
                result = RegisterResult.Success;
            }
            else
            {
                result = RegisterResult.CommandNameAlreadyExists;
            }

            return result;
        }

        /// <summary>
        /// Removes the command with the specified name from the global command's handler.
        /// </summary>
        /// <param name="name">The name of the command to try to remove.</param>
        /// <returns>True if a command was removed, false if no command existed with that name.</returns>
        public bool RemoveCommand(string name)
        {
            return commands.Remove(name);
        }

        /// <summary>
        /// Attempts to execute a command with the specified argument text.
        /// <para>This text should be the contents of the command line, without this <see cref="GlobalCommand"/>'s command name.</para>
        /// <para>eg: 'sampleCommand verb defaultValue --flag --operand operandValue'</para>
        /// <para>This allows commands to be executed from a source other than the Crestron command prompt.</para>
        /// </summary>
        /// <param name="args">The command line arguments, including the command, verb, defaultValue, operands, and flags.</param>
        public void ExecuteCommand(string args)
        {
            try
            {
                var command = commandRegex.Match(args);

                if (command.Success)
                {
                    var cmdName = command.Groups["name"].Value.ToLower(System.Globalization.CultureInfo.InvariantCulture).Trim();
                    var verb = command.Groups["verb"].Value.ToLower(System.Globalization.CultureInfo.InvariantCulture).Trim();
                    var defaultValue = command.Groups["defaultValue"].Value.Trim();

                    var result = parametersRegex.Matches(command.Groups["parameters"].Value.Trim());
                    var operands = new Dictionary<string, string>();

                    for (var i = 0; i < result.Count; i++)
                    {
                        if (result[i].Success && result[i].Groups["name"].Length > 0)
                        {
                            if (!operands.ContainsKey(result[i].Groups["name"].Value))
                            {
                                operands.Add(result[i].Groups["name"].Value.ToLower(), result[i].Groups["value"].Value);
                            }
                            else
                            {
                                WriteErrorMethod(string.Format("The '{0}' operand or flag was used more than once.\r\n", result[i].Groups["name"].Value));
                                WriteErrorMethod(string.Format("Duplicate operand or flag names are not allowed!"));
                                return;
                            }
                        }
                    }

                    ProcessCommand(cmdName, verb, defaultValue, operands);
                }
                else if (args.Contains("--help"))
                {
                    PrintGlobalCommandsHelp();
                }
                else
                {
                    WriteErrorMethod("Invalid command syntax. Enter '--help' for help.");
                }
            }
            catch (TerminalCommandException e)
            {
                var cee = CommandExceptionEncountered;
                if (cee != null)
                {
                    cee.Invoke(this, new TerminalCommandExceptionEventArgs(e, args));
                }
                else
                {
                    throw e;
                }
            }
            catch (Exception ex)
            {
                var e = new TerminalCommandException(ex, string.Format("Exception encountered while executing Terminal Command: '{0}'.", args));
                var cee = CommandExceptionEncountered;
                if (cee != null)
                {
                    cee.Invoke(this, new TerminalCommandExceptionEventArgs(e, args));
                }
                else
                {
                    throw e;
                }
            }
        }

        private void ProcessCommand(string commandName, string verb, string defaultValue, Dictionary<string, string> operands)
        {
            TerminalCommandBase command = null;

            if (!commands.ContainsKey(commandName))
            {
                WriteErrorMethod(string.Format("No command with the name '{0}' exists. Enter '--help' to view all available commands.", commandName));
                return;
            }
            else
            {
                command = commands[commandName];
            }

            if (string.IsNullOrEmpty(verb) && operands.ContainsKey("help"))
            {
                PrintCommandHelp(command, commandName);
                return;
            }
            else
            {
                if (operands.ContainsKey("help"))
                {
                    PrintVerbHelp(command, verb);
                    return;
                }

                var verbEntries = CommandHelpers.GetVerb(command, verb);

                if (verbEntries.Count == 0)
                {
                    if (string.IsNullOrEmpty(verb))
                    {
                        WriteErrorMethod(string.Format("The command '{0}' requires a verb. Enter '{0} --help' to view all available verbs.", commandName));
                    }
                    else
                    {
                        WriteErrorMethod(string.Format("No verb with the specified name '{0}' exists. Enter '{1} --help' to view all available verbs.", commandName, verb));
                    }
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        operands.Add("", defaultValue);
                    }

                    foreach (var entry in verbEntries)
                    {
                        var operandAttributes = CommandHelpers.GetOperands(entry.Key);
                        var flagAttributes = CommandHelpers.GetFlags(entry.Key);

                        var names = operandAttributes.Where((a) => operands.ContainsKey(a.Value.Name.ToLower())).Select(n => n.Value.Name.ToLower()).ToList();
                        names.AddRange(flagAttributes.Where(a => operands.ContainsKey(a.Value.Name.ToLower()) || a.Value.IsOptional).Select(n => n.Value.Name.ToLower()));

                        var optional = flagAttributes.Where(a => a.Value.IsOptional).Count();
                        var parameterCount = entry.Key.GetParameters().Length;

                        if ((names.Count == operands.Count && operands.Count == parameterCount) ||
                            (parameterCount - names.Count <= optional && optional > 0))
                        {
                            if (parameterCount > 0)
                            {
                                var parameterList = new object[parameterCount];

                                foreach (var oa in operandAttributes)
                                {
                                    var index = oa.Key.Position;
                                    var type = oa.Key.ParameterType;

                                    var operandName = operands[oa.Value.Name.ToLower()];
                                    var value = PrepareParameterValue(type, operandName);
                                    if (value != null)
                                    {
                                        parameterList[index] = value;
                                    }
                                    else
                                    {
                                        WriteErrorMethod(string.Format("Unable to convert the operand '{0}' with the value '{1}' to the expected type value '{2}'.", operandName, operands[operandName], type.ToString()));
                                        return;
                                    }
                                }

                                foreach (var fa in flagAttributes)
                                {
                                    if (operands.ContainsKey(fa.Value.Name.ToLower()))
                                    {
                                        parameterList[fa.Key.Position] = true;
                                    }
                                    else
                                    {
                                        parameterList[fa.Key.Position] = false;
                                    }
                                }

                                entry.Key.Invoke(command, parameterList);
                            }
                            else
                            {
                                entry.Key.Invoke(command, null);
                            }

                            return;
                        }
                    }

                    WriteErrorMethod(string.Format("The verb '{0}' requires a different combination of operands than what was provided.", verb));
                }
            }
        }

        private Action<string> ValidateWriter(Action<string> writer)
        {
            return writer != null ? writer : msg => CrestronConsole.ConsoleCommandResponse(msg);
        }

        private Func<string, string> ValidateFormatter(Func<string, string> formatter)
        {
            return formatter != null ? formatter : msg => msg;
        }

        private object PrepareParameterValue(CType type, string value)
        {
            if (type == typeof(bool))
            {
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }
                else if (value.Equals("yes", StringComparison.InvariantCultureIgnoreCase) ||
                        value.Equals("on", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
                else if (value.Equals("no", StringComparison.InvariantCultureIgnoreCase) ||
                        value.Equals("off", StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
                else
                {
                    try
                    {
                        return Convert.ToBoolean(value);
                    }
                    catch (FormatException)
                    {
                        return false;
                    }
                }
            }
            else if (type == typeof(Int32))
            {
                try
                {
                    return Convert.ToInt32(value);
                }
                catch (FormatException)
                {
                    return 0;
                }
            }
            else if (type == typeof(ushort))
            {
                try
                {
                    return Convert.ToUInt16(value);
                }
                catch (FormatException)
                {
                    return (ushort)0;
                }
            }
            else if (type == typeof(UInt32))
            {
                try
                {
                    return Convert.ToUInt32(value);
                }
                catch (FormatException)
                {
                    return (uint)0;
                }
            }
            else if (type == typeof(double))
            {
                try
                {
                    return Convert.ToDouble(value);
                }
                catch (FormatException)
                {
                    return (double)0;
                }
            }
            else
            {
                try
                {
                    return Convert.ChangeType(value, type, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    return null;
                }
            }
        }

        private void PrintVerbHelp(TerminalCommandBase command, string verbName)
        {
            var sb = new StringBuilder();

            var verbs = CommandHelpers.GetVerb(command, verbName);

            if (verbs.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine(FormatHelpTextMethod(string.Format("{0} - {1}", verbs.First().Value.Name, verbs.First().Value.Help)));
                sb.AppendLine();
                var examples = CommandHelpers.GetVerbExamples(command, verbName);

                if (examples.Count > 0)
                {
                    sb.AppendLine("Examples");
                    foreach (var e in examples)
                    {
                        sb.AppendLine(FormatHelpSampleMethod(string.Format("'{0}'", e.Sample)));
                        sb.Append('\t');
                        sb.AppendLine(FormatHelpTextMethod(e.Description));
                        sb.AppendLine();
                    }
                    sb.AppendLine();
                }

                List<OperandAttribute> operands = new List<OperandAttribute>();
                List<FlagAttribute> flags = new List<FlagAttribute>();

                foreach (var v in verbs)
                {
                    var ops = CommandHelpers.GetOperands(v.Key);
                    if (ops.Count > 0)
                    {
                        operands.AddRange(ops.Values);
                    }

                    var fl = CommandHelpers.GetFlags(v.Key);
                    if (fl.Count > 0)
                    {
                        flags.AddRange(fl.Values);
                    }
                }

                var nameWidth = 0;
                if (operands.Count > 0)
                {
                    nameWidth = operands.Max(op => op.Name.Length);
                }

                if (flags.Count > 0)
                {
                    nameWidth = Math.Max(nameWidth, flags.Max(fa => fa.Name.Length));
                }

                foreach (var op in operands)
                {
                    if (!string.IsNullOrEmpty(op.Name))
                    {
                        sb.Append(FormatHelpTextMethod("--"));
                        sb.Append(FormatHelpOperandMethod(op.Name.PadRight(nameWidth)));
                        sb.Append('\t', 2);
                        sb.AppendLine(FormatHelpTextMethod(op.Help));
                    }
                }

                foreach (var flag in flags)
                {
                    sb.Append(FormatHelpTextMethod("--"));
                    sb.Append(FormatHelpFlagMethod(flag.Name.PadRight(nameWidth)));
                    sb.Append('\t', 2);
                    sb.AppendLine(FormatHelpTextMethod(flag.Help));
                }
            }
            else
            {
                sb.Append(FormatHelpTextMethod(string.Format("There is no help available for '{0}'", verbName)));
            }

            WriteHelpMethod(sb.ToString());
        }

        private void PrintCommandHelp(TerminalCommandBase command, string name)
        {
            var sb = new StringBuilder();
            var help = CommandHelpers.GetCommandHelpFromAttribute(command);
            sb.AppendLine();
            sb.AppendLine(FormatHelpCommandMethod(name));
            if (!string.IsNullOrEmpty("help"))
            {
                sb.AppendLine(FormatHelpTextMethod(help));
            }

            sb.AppendLine(FormatHelpTextMethod("-----"));
            sb.AppendLine(FormatHelpTextMethod("Available Verbs"));
            sb.AppendLine();

            var verbs = CommandHelpers.GetVerbs(command);

            var nameWidth = 0;
            if (verbs.Count > 0)
            {
                nameWidth = verbs.Max(v => v.Value.Name.Length);
            }

            nameWidth = Math.Max(nameWidth, 4);

            var details = new List<VerbAttribute>();

            foreach (var kvp in verbs)
            {
                if (details.Where(d => d.Name == kvp.Value.Name).Count() == 0)
                {
                    details.Add(kvp.Value);

                    sb.Append(kvp.Value.Name.PadRight(nameWidth));
                    sb.Append('\t', 2);
                    sb.Append(kvp.Value.Help);
                    sb.AppendLine();
                }
            }

            sb.Append(FormatHelpFlagMethod("Help".PadRight(nameWidth)));
            sb.Append('\t', 2);
            sb.Append(FormatHelpTextMethod("Prints help for this command."));
            sb.AppendLine();

            sb.AppendLine(FormatHelpTextMethod("-----"));

            WriteHelpMethod(sb.ToString());
        }

        private void PrintGlobalCommandsHelp()
        {
            var sb = new StringBuilder();
            sb.AppendLine(FormatHelpTextMethod(string.Format("Listing the commands available for '{0}'", Name)));
            sb.AppendLine(FormatHelpTextMethod("-----"));

            var nameWidth = commands.Max(c => c.Key.Length);

            foreach (var command in commands)
            {
                sb.Append(FormatHelpCommandMethod(command.Key.PadRight(nameWidth)));
                sb.Append('\t', 2);
                var help = CommandHelpers.GetCommandHelpFromAttribute(command.Value);
                sb.Append(FormatHelpTextMethod(string.IsNullOrEmpty(help) ? "No help is available for this command." : help));
                sb.AppendLine();
            }

            sb.AppendLine(FormatHelpTextMethod("-----"));

            WriteHelpMethod(sb.ToString());
        }

    }
}