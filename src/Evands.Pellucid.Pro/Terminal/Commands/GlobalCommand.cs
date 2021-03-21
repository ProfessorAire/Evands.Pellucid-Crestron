#region copyright
// <copyright file="GlobalCommand.cs" company="Christopher McNeely">
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
using Crestron.SimplSharp.Reflection;
using Evands.Pellucid.Terminal.Commands.Attributes;

namespace Evands.Pellucid.Terminal.Commands
{
    /// <summary>
    /// Global console command that is registered with the <see cref="CrestronConsole"/> or other console service on startup.
    /// <para>Without a global command registered with the console no commands will be available to execute.</para>
    /// </summary>
    public class GlobalCommand
    {
        /// <summary>
        /// The regex to use for parsing command details from console text input.
        /// </summary>
        private static Regex commandRegex = new Regex("^(?<name>[\\w-]+){1} ?(?<verb>[^\\r\\n\"\\ ]+)? ?\"?(?<defaultValue>[^\\r\\n\"-]+)?\"? ?(?<parameters>.*)?", RegexOptions.Compiled);

        /// <summary>
        /// The regex to use for parsing parameters from console text commands.
        /// </summary>
        private static Regex parametersRegex = new Regex("(?:-{2}(?:(?<name>\\w+)(?: +\"?(?<value>[^-\\r\\n\"]*))?\"?)){0,1}(?:-{1}(?<shortName>[a-zA-Z]+)){0,1}", RegexOptions.Compiled);

        /// <summary>
        /// Dictionary of <see cref="TerminalCommandBase"/> objects registered with this command.
        /// </summary>
        private Dictionary<string, TerminalCommandBase> commands = new Dictionary<string, TerminalCommandBase>();

        /// <summary>
        /// Backing field for the <see cref="WriteErrorMethod"/> property.
        /// </summary>
        private Action<string> writeErrorMethod;

        /// <summary>
        /// Backing field for the <see cref="WriteHelpMethod"/> property.
        /// </summary>
        private Action<string> writeHelpMethod;

        /// <summary>
        /// Backing field for the <see cref="FormatHelpCommandMethod"/> property.
        /// </summary>
        private Func<string, string> formatHelpCommandMethod;

        /// <summary>
        /// Backing field for the <see cref="FormatHelpVerbMethod"/> property.
        /// </summary>
        private Func<string, string> formatHelpVerbMethod;

        /// <summary>
        /// Backing field for the <see cref="FormatHelpOperandMethod"/> property.
        /// </summary>
        private Func<string, string> formatHelpOperandMethod;

        /// <summary>
        /// Backing field for the <see cref="FormatHelpFlagMethod"/> property.
        /// </summary>
        private Func<string, string> formatHelpFlagMethod;

        /// <summary>
        /// Backing field for the <see cref="FormatHelpSampleMethod"/> property.
        /// </summary>
        private Func<string, string> formatHelpSampleMethod;

        /// <summary>
        /// Backing field for the <see cref="FormatHelpTextMethod"/> property.
        /// </summary>
        private Func<string, string> formatHelpTextMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command, as it should appear in the processor console. Limited to 23 characters.</param>
        /// <param name="help">Help text shown when entering 'help user' on a processor command prompt. Limited to 79 characters.</param>
        /// <param name="access">The user access level the command should be given.</param>
        public GlobalCommand(string name, string help, Access access)
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
            WriteErrorMethod = (msg) => ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightRed, msg);
            WriteHelpMethod = (msg) => ConsoleBase.WriteCommandResponse(ConsoleBase.Colors.BrightYellow, msg);
            FormatHelpCommandMethod = (msg) => ConsoleBase.Colors.BrightGreen.FormatText(msg);
            FormatHelpVerbMethod = (msg) => ConsoleBase.Colors.BrightMagenta.FormatText(msg);
            FormatHelpOperandMethod = (msg) => ConsoleBase.Colors.BrightCyan.FormatText(msg);
            FormatHelpFlagMethod = (msg) => ConsoleBase.Colors.BrightBlue.FormatText(msg);
            FormatHelpSampleMethod = (msg) => ConsoleBase.Colors.BrightGreen.FormatText(msg);
            FormatHelpTextMethod = (msg) => ConsoleBase.Colors.BrightYellow.FormatText(msg);
        }

        /// <summary>
        /// Raised when an exception is encountered by a command.
        /// <para>If this is null when an exception is encountered the exception will be re-thrown.</para>
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
        public Access CommandAccess { get; private set; }

        /// <summary>
        /// Attempts to add the command to the console.
        /// <para>This must be done in the Control System Constructor.</para>
        /// </summary>
        /// <returns>True if it succeeds, false if it fails for any reason.</returns>
        public bool AddToConsole()
        {
            ConsoleAccessLevelEnum level = (ConsoleAccessLevelEnum)CommandAccess;

            if (CrestronConsole.AddNewConsoleCommand(ExecuteCommand, Name, Help, level))
            {
                return Manager.RegisterCrestronConsoleCommand(this);
            }

            return false;
        }

        /// <summary>
        /// Attempts to remove the command from the console.
        /// </summary>
        /// <returns>True if it succeeds, false if it fails for any reason.</returns>
        public bool RemoveFromConsole()
        {
            return Manager.RemoveCrestronConsoleCommand(Name);
        }

        /// <summary>
        /// Add a new terminal command to the global command.
        /// <para>Unlike the Global commands, these can be added at any point in program execution.</para>
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to add to the global command's handler.</param>
        /// <returns>The result of the registration attempt as a <see cref="RegisterResult"/>.</returns>
        public RegisterResult AddCommand(TerminalCommandBase command)
        {
            var name = Helpers.GetCommandNameFromAttribute(command);

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
        /// <returns><see cref="RegisterResult"/>.</returns>
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
        /// Removes the specified command from the global command's handler, provided the command is registered.
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to remove.</param>
        /// <returns>True if a command was removed, false if no command existed with that name.</returns>
        public bool RemoveCommand(TerminalCommandBase command)
        {
            if (IsCommandRegistered(command))
            {
                return RemoveCommand(command.Name);
            }

            return false;
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
        /// Gets whether the specified command is registered with this global command.
        /// </summary>
        /// <param name="command">The command to check registration of.</param>
        /// <returns><see langword="True"/> if the command is registered.</returns>
        public bool IsCommandRegistered(TerminalCommandBase command)
        {
            return this.commands.ContainsKey(command.Name) && this.commands[command.Name] == command;
        }

        /// <summary>
        /// Attempts to execute a command with the specified argument text.
        /// <para>This text should be the contents of the command line, without this <see cref="GlobalCommand"/>'s command name.</para>
        /// <para>Example: 'sampleCommand verb defaultValue --flag --operand operandValue'</para>
        /// <para>This allows commands to be executed from a source other than the command prompt.</para>
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
                            var val = result[i].Groups["name"].Value.ToLower().Trim();
                            if (!operands.ContainsKey(val))
                            {
                                operands.Add(val, result[i].Groups["value"].Value);
                            }
                            else
                            {
                                WriteErrorMethod(string.Format("The '{0}' operand or flag was used more than once.\r\n", result[i].Groups["name"].Value));
                                WriteErrorMethod(string.Format("Duplicate operand or flag names are not allowed!"));
                                return;
                            }
                        }
                        else if (result[i].Success && result[i].Groups["shortName"].Length > 0)
                        {
                            var values = result[i].Groups["shortName"].Value.ToCharArray();

                            for (var j = 0; j < values.Length; j++)
                            {
                                if (!operands.ContainsKey(values[j].ToString()))
                                {
                                    operands.Add(values[j].ToString(), string.Empty);
                                }
                            }
                        }
                    }

                    ProcessCommand(cmdName, verb, defaultValue, operands);
                }
                else if (args.Contains("--help") || args.Contains("-h"))
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
                    ProConsole.WriteLine();
                    ProConsole.WriteError("Exception encountered while executing Terminal Command: '{0}'.\r\n{1}", args, ex.ToString());
                }
            }
        }

        private void ProcessCommand(string commandName, string verb, string defaultValue, Dictionary<string, string> operandsAndFlags)
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

            if (string.IsNullOrEmpty(verb))
            {
                PrintCommandHelp(command, commandName);
                return;
            }
            else
            {
                var verbEntries = Helpers.GetVerb(command, verb);

                if (verbEntries.Count == 0)
                {
                    if (string.IsNullOrEmpty(verb))
                    {
                        WriteErrorMethod(string.Format("The command '{0}' requires a verb. Enter '{0} --help' to view all available verbs.", commandName));
                    }
                    else
                    {
                        WriteErrorMethod(string.Format("No verb with the specified name '{0}' exists. Enter '{0} --help' to view all available verbs.", commandName));
                    }

                    return;
                }
                else
                {
                    if (operandsAndFlags.ContainsKey("help") || operandsAndFlags.ContainsKey("h"))
                    {
                        PrintVerbHelp(command, verb);
                        return;
                    }

                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        operandsAndFlags.Add(string.Empty, defaultValue);
                    }

                    foreach (var entry in verbEntries)
                    {
                        var operandAttributes = Helpers.GetOperands(entry.Key);
                        var flagAttributes = Helpers.GetFlags(entry.Key);

                        var names = operandAttributes.Where((a) => operandsAndFlags.ContainsKey(a.Value.Name.ToLower())).Select(n => n.Value.Name.ToLower()).ToList();
                        names.AddRange(flagAttributes.Where(a => operandsAndFlags.ContainsKey(a.Value.Name.ToLower()) || operandsAndFlags.ContainsKey(a.Value.ShortName.ToString().ToLower()) || a.Value.IsOptional).Select(n => n.Value.Name.ToLower()));

                        var optional = flagAttributes.Where(a => a.Value.IsOptional).Count();
                        var parameterCount = entry.Key.GetParameters().Length;

                        var success = true;
                        
                        foreach (var oaf in operandsAndFlags.Keys)
                        {
                            if (operandAttributes.Values.Any(oa => oa.Name.ToLower() == oaf))
                            {
                                continue;
                            }

                            if (flagAttributes.Values.Any(fa => fa.Name.ToLower() == oaf || (fa.ShortName.HasValue && fa.ShortName.Value.ToString() == oaf)))
                            {
                                continue;
                            }

                            success = false;
                        }

                        if (!success)
                        {
                            continue;
                        }

                        if ((names.Count == operandsAndFlags.Count && operandsAndFlags.Count == parameterCount) ||
                            (parameterCount - names.Count <= optional && optional > 0))
                        {
                            if (parameterCount > 0)
                            {
                                var parameterList = new object[parameterCount];

                                try
                                {
                                    foreach (var oa in operandAttributes)
                                    {
                                        var index = oa.Key.Position;
                                        var type = oa.Key.ParameterType;

                                        var operandName = operandsAndFlags[oa.Value.Name.ToLower()];
                                        var value = PrepareParameterValue(type, operandName);
                                        if (value != null)
                                        {
                                            parameterList[index] = value;
                                        }
                                        else
                                        {
                                            WriteErrorMethod(string.Format("Unable to convert the operand '{0}' with the value '{1}' to the expected type value '{2}'.", operandName, operandsAndFlags[operandName], type.ToString()));
                                            return;
                                        }
                                    }

                                    foreach (var fa in flagAttributes)
                                    {
                                        if (operandsAndFlags.ContainsKey(fa.Value.Name.ToLower()) || (fa.Value.ShortName.HasValue && operandsAndFlags.ContainsKey(fa.Value.ShortName.ToString().ToLower())))
                                        {
                                            parameterList[fa.Key.Position] = true;
                                        }
                                        else
                                        {
                                            parameterList[fa.Key.Position] = false;
                                        }
                                    }
                                }
                                catch
                                {
                                    success = false;
                                }

                                if (success)
                                {
                                    entry.Key.Invoke(command, parameterList);
                                }
                            }
                            else
                            {
                                entry.Key.Invoke(command, null);
                            }

                            if (success)
                            {
                                return;
                            }
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
            else if (type == typeof(int))
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
            else if (type == typeof(uint))
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

        /// <summary>
        /// Prints the associated verb's help.
        /// </summary>
        /// <param name="command">The command to check for verbs.</param>
        /// <param name="verbName">The name of the verb to print help for.</param>
        private void PrintVerbHelp(TerminalCommandBase command, string verbName)
        {
            var sb = new StringBuilder();

            var verbs = Helpers.GetVerb(command, verbName);

            if (verbs.Count > 0)
            {
                sb.AppendLine();
                sb.Append(string.Format("{0}", FormatHelpVerbMethod(verbs.First().Value.Name)));
                sb.Append(' ', 6);
                sb.AppendLine(FormatHelpTextMethod(string.Format("{0}", verbs.First().Value.Help)));
                sb.AppendLine();

                var examples = Helpers.GetVerbExamples(command, verbName);
                if (examples.Count > 0)
                {
                    sb.AppendLine(FormatHelpTextMethod("Examples"));
                    foreach (var e in examples)
                    {
                        sb.AppendLine(FormatHelpSampleMethod(string.Format("'{0}'", e.Sample)));
                        sb.Append(' ', 5);
                        sb.AppendLine(FormatHelpTextMethod(e.Description));
                        sb.AppendLine();
                    }
                }

                List<OperandAttribute> operands = new List<OperandAttribute>();
                List<FlagAttribute> flags = new List<FlagAttribute>();

                foreach (var v in verbs)
                {
                    var ec = EqualityComparer<KeyValuePair<ParameterInfo, OperandAttribute>>.Default;
                                        
                    var ops = Helpers.GetOperands(v.Key);
                    if (ops.Count > 0)
                    {
                        foreach (var val in ops.Values)
                        {
                            if (!operands.Any(o => o.Name == val.Name))
                            {
                                operands.Add(val);
                            }
                        }
                    }

                    var fl = Helpers.GetFlags(v.Key);
                    if (fl.Count > 0)
                    {
                        foreach (var val in fl.Values)
                        {
                            if (!flags.Any(f => f.Name == val.Name))
                            {
                                flags.Add(val);
                            }
                        }
                    }
                }

                var nameWidth = 0;
                if (operands.Count > 0)
                {
                    nameWidth = operands.Max(op => op.Name.Length);
                }

                if (flags.Count > 0)
                {
                    nameWidth = Math.Max(nameWidth, flags.Max(fa => fa.Name.Length + 2 + (fa.ShortName.HasValue ? 4 : 0)));
                }

                sb.Append(FormatHelpOperandMethod("Operands"));
                sb.Append(FormatHelpTextMethod("/"));
                sb.AppendLine(FormatHelpFlagMethod("Flags"));
                sb.AppendLine();

                foreach (var op in operands)
                {
                    if (!string.IsNullOrEmpty(op.Name))
                    {
                        sb.Append(FormatHelpOperandMethod(string.Format("--{0}", op.Name.PadRight(nameWidth - 2))));
                        sb.Append(' ', 6);
                        sb.AppendLine(FormatHelpTextMethod(op.Help));
                    }
                }

                foreach (var flag in flags)
                {
                    var value = string.Format("--{0}", flag.Name);
                    if (flag.ShortName.HasValue)
                    {
                        value = string.Format("{0}, -{1}", value, flag.ShortName);
                    }

                    value = FormatHelpFlagMethod(value.PadRight(nameWidth));
                    sb.Append(value);
                    sb.Append(' ', 6);
                    sb.AppendLine(FormatHelpTextMethod(flag.Help));
                }
            }
            else
            {
                sb.Append(FormatHelpTextMethod(string.Format("There is no help available for '{0}'", verbName)));
            }

            WriteHelpMethod(sb.ToString());
        }

        /// <summary>
        /// Prints the associated command's help.
        /// </summary>
        /// <param name="command">The command to print help for.</param>
        /// <param name="name">The name of the name of the command.</param>
        private void PrintCommandHelp(TerminalCommandBase command, string name)
        {
            var sb = new StringBuilder();
            var help = Helpers.GetCommandHelpFromAttribute(command);
            sb.AppendLine();
            sb.AppendLine(FormatHelpCommandMethod(name));
            if (!string.IsNullOrEmpty(help))
            {
                sb.AppendLine(FormatHelpTextMethod(help));
            }

            sb.AppendLine();
            sb.AppendLine(formatHelpVerbMethod("Verbs"));
            sb.AppendLine(formatHelpTextMethod("-----"));

            var verbs = Helpers.GetVerbs(command);

            var nameWidth = 0;
            if (verbs.Count > 0)
            {
                nameWidth = verbs.Max(v => v.Value.Name.Length);
            }

            nameWidth = Math.Max(nameWidth, 10);

            var details = new List<VerbAttribute>();

            foreach (var kvp in verbs.OrderBy(t => t.Value.Name))
            {
                if (details.Where(d => d.Name == kvp.Value.Name).Count() == 0)
                {
                    details.Add(kvp.Value);

                    if (string.IsNullOrEmpty(kvp.Value.Name))
                    {
                        sb.Append(FormatHelpVerbMethod("<none>".PadRight(nameWidth)));
                    }
                    else
                    {
                        sb.Append(FormatHelpVerbMethod(kvp.Value.Name.PadRight(nameWidth)));
                    }

                    sb.Append(' ', 6);
                    sb.Append(FormatHelpTextMethod(kvp.Value.Help));
                    sb.AppendLine();
                }
            }

            sb.AppendLine();
            sb.AppendLine(FormatHelpFlagMethod("Flags"));
            sb.AppendLine(FormatHelpTextMethod("-----"));
            sb.Append(FormatHelpFlagMethod("--Help, -h".PadRight(nameWidth)));
            sb.Append(' ', 6);
            sb.Append(FormatHelpTextMethod("Prints help for this command."));
            sb.AppendLine();

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
                sb.Append(' ', 6);
                var help = Helpers.GetCommandHelpFromAttribute(command.Value);
                sb.Append(FormatHelpTextMethod(string.IsNullOrEmpty(help) ? "No help is available for this command." : help));
                sb.AppendLine();
            }

            sb.AppendLine(FormatHelpTextMethod("-----"));

            WriteHelpMethod(sb.ToString());
        }
    }
}