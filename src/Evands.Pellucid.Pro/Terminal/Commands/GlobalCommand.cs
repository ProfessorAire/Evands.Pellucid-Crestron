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
    public class GlobalCommand : IDisposable
    {
        /// <summary>
        /// The regex to use for parsing command details from console text input.
        /// </summary>
        private static Regex commandRegex = new Regex("^(?!-{1,2})(?<name>[\\w-]+){1} ?(?!-{1,2})(?<verb>[^\\r\\n\"\\ ]+)? ?\"?(?<defaultValue>[^\\r\\n\"-]+)?\"? ?(?<parameters>.*)?", RegexOptions.Compiled);

        /// <summary>
        /// The regex to use for parsing parameters from console text commands.
        /// </summary>
        private static Regex parametersRegex = new Regex("-{2}(?:(?<name>\\w+)(?: +\"(?<value>[^\"]*)\"| +(?<value2>[^-][^ ]*)){0,1})|(?<!-)(?:-{1}(?<shortName>[a-zA-Z0-9]+))", RegexOptions.Compiled);

        /// <summary>
        /// Dictionary of <see cref="TerminalCommandBase"/> objects registered with this command.
        /// </summary>
        private Dictionary<string, TerminalCommandBase> commands = new Dictionary<string, TerminalCommandBase>();

        /// <summary>
        /// Dictionary of <see cref="TerminalCommandBase"/> objects registered with aliases with this command.
        /// </summary>
        private Dictionary<string, TerminalCommandBase> aliasedCommands = new Dictionary<string, TerminalCommandBase>();

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
        /// Synchronization object for command dictionaries.
        /// </summary>
        private CCriticalSection syncRoot = new CCriticalSection();

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalCommand"/> class.
        /// </summary>
        /// <param name="name">The name of the command, as it should appear in the processor console. Limited to 23 characters.</param>
        /// <param name="help">Help text shown when entering 'help user' on a processor command prompt. Limited to 79 characters.</param>
        /// <param name="access">The user access level the command should be given.</param>
        public GlobalCommand(string name, string help, Access access)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentOutOfRangeException("name", "The name must be non-empty and non-null.");
            }

            if (name.Length >= 23)
            {
                throw new ArgumentOutOfRangeException("name", "The name can be no longer than 23 characters.");
            }

            Name = name;
            Help = help.Substring(0, Math.Min(help.Length, 76));
            if (Help.Length == 76 && help.Length != 76)
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
        /// Gets all the registered global commands.
        /// </summary>
        /// <returns>All the registered global commands.</returns>
        public static System.Collections.ObjectModel.ReadOnlyCollection<GlobalCommand> GetAllGlobalCommands()
        {
            return new System.Collections.ObjectModel.ReadOnlyCollection<GlobalCommand>(Manager.GetAllGlobalCommands());
        }

        /// <summary>
        /// Attempts to add the command to the console.
        /// <para>This must be done in the Control System Constructor.</para>
        /// </summary>
        /// <returns>True if it succeeds, false if it fails for any reason.</returns>
        public bool AddToConsole()
        {
            this.syncRoot.Enter();

            try
            {
                ConsoleAccessLevelEnum level = (ConsoleAccessLevelEnum)CommandAccess;

                if (CrestronConsole.AddNewConsoleCommand(ExecuteCommand, Name, Help, level) || CrestronEnvironment.DevicePlatform == eDevicePlatform.Server)
                {
                    return Manager.RegisterCrestronConsoleCommand(this);
                }

                return false;
            }
            finally
            {
                syncRoot.Leave();
            }
        }

        /// <summary>
        /// Attempts to remove the command from the console.
        /// </summary>
        /// <returns>True if it succeeds, false if it fails for any reason.</returns>
        public bool RemoveFromConsole()
        {
            syncRoot.Enter();

            try
            {
                return Manager.RemoveCrestronConsoleCommand(Name);
            }
            finally
            {
                syncRoot.Leave();
            }
        }

        /// <summary>
        /// Add a new terminal command to the global command.
        /// <para>Unlike the Global commands, these can be added at any point in program execution.</para>
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to add to the global command's handler.</param>
        /// <returns><see cref="RegisterResult"/>.</returns>
        public RegisterResult AddCommand(TerminalCommandBase command)
        {
            syncRoot.Enter();

            try
            {
                RegisterResult result;
                var commandName = command.Name.ToLower();

                if (string.IsNullOrEmpty(commandName))
                {
                    return RegisterResult.NoCommandAttributeFound;
                }

                var commandAlias = !string.IsNullOrEmpty(command.Alias) ? command.Alias.ToLower() : string.Empty;

                if (!commands.ContainsKey(commandName) && !aliasedCommands.ContainsKey(commandAlias))
                {
                    commands.Add(commandName, command);
                    if (!string.IsNullOrEmpty(commandAlias))
                    {
                        aliasedCommands.Add(commandAlias, command);
                    }

                    result = RegisterResult.Success;
                }
                else
                {
                    result = RegisterResult.CommandNameAlreadyExists;
                }

                return result;
            }
            finally
            {
                syncRoot.Leave();
            }
        }

        /// <summary>
        /// Removes the specified command from the global command's handler, provided the command is registered.
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to remove.</param>
        /// <returns>True if a command was removed, false if no command existed with that name.</returns>
        public bool RemoveCommand(TerminalCommandBase command)
        {
            syncRoot.Enter();

            try
            {
                if (IsCommandRegistered(command))
                {
                    aliasedCommands.Remove(command.Alias.ToLower());
                    return commands.Remove(command.Name.ToLower());
                }

                return false;
            }
            finally
            {
                syncRoot.Leave();
            }
        }

        /// <summary>
        /// Gets whether the specified command is registered with this global command.
        /// </summary>
        /// <param name="command">The command to check registration of.</param>
        /// <returns><see langword="True"/> if the command is registered.</returns>
        public bool IsCommandRegistered(TerminalCommandBase command)
        {
            syncRoot.Enter();

            try
            {
                return this.commands.ContainsKey(command.Name.ToLower()) && this.commands[command.Name.ToLower()] == command;
            }
            finally
            {
                syncRoot.Leave();
            }
        }

        /// <summary>
        /// Disposes of internal resources.
        /// </summary>
        public void Dispose()
        {
            aliasedCommands.Clear();
            commands.Clear();
            this.RemoveFromConsole();
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
                            var key = result[i].Groups["name"].Value.ToLower().Trim();
                            if (!operands.ContainsKey(key))
                            {
                                if (!string.IsNullOrEmpty(result[i].Groups["value"].Value))
                                {
                                    operands.Add(key, result[i].Groups["value"].Value);
                                }
                                else if (!string.IsNullOrEmpty(result[i].Groups["value2"].Value))
                                {
                                    operands.Add(key, result[i].Groups["value2"].Value);
                                }
                                else
                                {
                                    operands.Add(key, string.Empty);
                                }
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
                                else
                                {
                                    WriteErrorMethod(string.Format("The '{0}' operand or flag was used more than once.\r\n", result[i].Groups["name"].Value));
                                    WriteErrorMethod(string.Format("Duplicate operand or flag names are not allowed!"));
                                    return;
                                }
                            }
                        }
                    }

                    ProcessCommand(cmdName, verb, defaultValue, operands);
                }
                else if (args.ToLower().Contains("--help") || args.ToLower().Contains("-h"))
                {
                    PrintGlobalCommandsHelp();
                }
                else
                {
                    WriteErrorMethod("You must enter the name of a command. Enter '--help' for a list of available commands.");
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

        /// <summary>
        /// Processes the command with the provided values.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="verb">The verb contained in the command.</param>
        /// <param name="defaultValue">The default value of the command.</param>
        /// <param name="operandsAndFlags">The operands and flags of the command.</param>
        private void ProcessCommand(string commandName, string verb, string defaultValue, Dictionary<string, string> operandsAndFlags)
        {
            TerminalCommandBase command = null;

            if (!commands.ContainsKey(commandName))
            {
                if (!aliasedCommands.ContainsKey(commandName))
                {
                    WriteErrorMethod(string.Format("No command with the name '{0}' exists. Enter '--help' to view all available commands.", commandName));
                    return;
                }

                command = aliasedCommands[commandName];
            }
            else
            {
                command = commands[commandName];
            }

            if (string.IsNullOrEmpty(verb) && (operandsAndFlags.ContainsKey("help") || operandsAndFlags.ContainsKey("h")))
            {
                PrintCommandHelp(command);
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
                        WriteErrorMethod(string.Format("No verb with the specified name '{1}' exists. Enter '{0} --help' to view all available verbs.", commandName, verb));
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

                                foreach (var oa in operandAttributes)
                                {
                                    var index = oa.Key.Position;
                                    var type = oa.Key.ParameterType;

                                    var operandName = oa.Value.Name.ToLower();
                                    var operandValue = operandsAndFlags[operandName];
                                    object value = null;
                                    if (TryPrepareParameterValue(type, operandValue, out value))
                                    {
                                        parameterList[index] = value;
                                    }
                                    else
                                    {
                                        WriteErrorMethod(string.Format("Unable to convert the operand '{0}' with the value '{1}' to the expected type value '{2}'.", operandName, operandValue, type.ToString()));
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

                    WriteErrorMethod(string.Format("The verb '{0}' requires a different combination of operands than what was provided. Enter '{1} {0} --help' for more information.", verb, commandName));
                }
            }
        }

        private Action<string> ValidateWriter(Action<string> writer)
        {
            return writer != null ? writer : msg => ConsoleBase.WriteCommandResponse(msg);
        }

        private Func<string, string> ValidateFormatter(Func<string, string> formatter)
        {
            return formatter != null ? formatter : msg => msg;
        }

        private bool TryPrepareParameterValue(CType type, string value, out object result)
        {
            result = null;
            if (type == typeof(bool))
            {
                if (string.IsNullOrEmpty(value))
                {
                    result = false;
                    return true;
                }
                else if (value.Equals("yes", StringComparison.InvariantCultureIgnoreCase) ||
                        value.Equals("on", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = true;
                    return true;
                }
                else if (value.Equals("no", StringComparison.InvariantCultureIgnoreCase) ||
                        value.Equals("off", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = false;
                    return true;
                }
                else
                {
                    try
                    {
                        result = Convert.ToBoolean(value);
                        return true;
                    }
                    catch (FormatException)
                    {
                        result = null;
                        return false;
                    }
                }
            }
            else if (type == typeof(int))
            {
                try
                {
                    result = Convert.ToInt32(value);
                    return true;
                }
                catch (FormatException)
                {
                    result = 0;
                    return false;
                }
            }
            else if (type == typeof(ushort))
            {
                try
                {
                    result = Convert.ToUInt16(value);
                    return true;
                }
                catch (FormatException)
                {
                    result = 0;
                    return false;
                }
            }
            else if (type == typeof(uint))
            {
                try
                {
                    result = Convert.ToUInt32(value);
                    return true;
                }
                catch (FormatException)
                {
                    result = 0;
                    return false;
                }
            }
            else if (type == typeof(double))
            {
                try
                {
                    result = Convert.ToDouble(value);
                    return true;
                }
                catch (FormatException)
                {
                    result = 0;
                    return false;
                }
            }
            else
            {
                try
                {
                    result = Convert.ChangeType(value, type, System.Globalization.CultureInfo.InvariantCulture);
                    return true;
                }
                catch
                {
                    result = null;
                    return false;
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
                sb.Append(ConsoleBase.NewLine);
                sb.Append(FormatHelpCommandMethod(verbs.First().Value.HelpFormattedName));
                sb.Append(' ', 6);
                sb.Append(FormatHelpTextMethod(string.Format("{0}", verbs.First().Value.Help)));
                sb.Append(ConsoleBase.NewLine);
                sb.Append(ConsoleBase.NewLine);

                var examples = Helpers.GetVerbExamples(command, verbName);
                if (examples.Count > 0)
                {
                    sb.Append(FormatHelpTextMethod("Examples"));
                    sb.Append(ConsoleBase.NewLine);
                    foreach (var e in examples)
                    {
                        sb.Append(FormatHelpSampleMethod(string.Format("'{0}'", e.Sample)));
                        sb.Append(ConsoleBase.NewLine);
                        sb.Append(' ', 5);
                        sb.Append(FormatHelpTextMethod(e.Description));
                        sb.Append(ConsoleBase.NewLine);
                        sb.Append(ConsoleBase.NewLine);
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
                sb.Append(FormatHelpFlagMethod("Flags"));
                sb.Append(ConsoleBase.NewLine);
                sb.Append(ConsoleBase.NewLine);

                foreach (var op in operands)
                {
                    if (!string.IsNullOrEmpty(op.Name))
                    {
                        sb.Append(FormatHelpOperandMethod(string.Format("--{0}", op.Name.PadRight(nameWidth - 2))));
                        sb.Append(' ', 6);
                        sb.Append(FormatHelpTextMethod(op.Help));
                        sb.Append(ConsoleBase.NewLine);
                    }
                }

                foreach (var flag in flags)
                {
                    var value = string.Format("--{0}", flag.Name);
                    if (flag.ShortName.HasValue)
                    {
                        value = string.Format("{0}, -{1}{2}", value, flag.ShortName, flag.IsOptional ? " (optional)" : string.Empty);
                    }

                    value = FormatHelpFlagMethod(value.PadRight(nameWidth));
                    sb.Append(value);
                    sb.Append(' ', 6);
                    sb.Append(FormatHelpTextMethod(flag.Help));
                    sb.Append(ConsoleBase.NewLine);
                }
            }

            WriteHelpMethod(sb.ToString());
        }

        /// <summary>
        /// Prints the associated command's help.
        /// </summary>
        /// <param name="command">The command to print help for.</param>
        private void PrintCommandHelp(TerminalCommandBase command)
        {
            var sb = new StringBuilder();
            var help = Helpers.GetCommandHelpFromAttribute(command);
            sb.Append(ConsoleBase.NewLine);
            sb.Append(FormatHelpCommandMethod(Helpers.GetCommandNameHelpFromAttribute(command)));
            if (!string.IsNullOrEmpty(help))
            {
                sb.AppendFormat(" - {0}{1}", FormatHelpTextMethod(help), ConsoleBase.NewLine);
            }

            sb.Append(ConsoleBase.NewLine);
            sb.AppendFormat("{0}{1}", formatHelpVerbMethod("Verbs"), ConsoleBase.NewLine);
            sb.AppendFormat("{0}{1}", formatHelpTextMethod("-----"), ConsoleBase.NewLine);

            var verbs = Helpers.GetVerbs(command);

            var nameWidth = 0;
            if (verbs.Count > 0)
            {
                nameWidth = verbs.Max(v => v.Value.HelpFormattedName.Length);
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
                        sb.Append(FormatHelpVerbMethod(kvp.Value.HelpFormattedName.PadRight(nameWidth)));
                    }

                    sb.Append(' ', 6);
                    sb.Append(FormatHelpTextMethod(kvp.Value.Help));
                    sb.Append(ConsoleBase.NewLine);
                }
            }

            sb.Append(ConsoleBase.NewLine);
            sb.AppendFormat("{0}{1}", FormatHelpFlagMethod("Flags"), ConsoleBase.NewLine);
            sb.AppendFormat("{0}{1}", FormatHelpTextMethod("-----"), ConsoleBase.NewLine);
            sb.Append(FormatHelpFlagMethod("--Help, -h".PadRight(nameWidth)));
            sb.Append(' ', 6);
            sb.Append(FormatHelpTextMethod("Prints help for this command."));
            sb.Append(ConsoleBase.NewLine);

            WriteHelpMethod(sb.ToString());
        }

        private void PrintGlobalCommandsHelp()
        {
            var sb = new StringBuilder();
            sb.Append(FormatHelpTextMethod(string.Format("Listing the commands available for '{0}'{1}", Name, ConsoleBase.NewLine)));
            sb.Append(FormatHelpTextMethod("-----"));
            sb.Append(ConsoleBase.NewLine);

            var nameWidth = commands.Max(c => Helpers.GetCommandNameHelpFromAttribute(c.Value).Length);

            foreach (var command in commands)
            {
                var name = Helpers.GetCommandNameHelpFromAttribute(command.Value);
                sb.Append(FormatHelpCommandMethod(name.PadRight(nameWidth)));
                sb.Append(' ', 6);
                var help = Helpers.GetCommandHelpFromAttribute(command.Value);
                sb.Append(FormatHelpTextMethod(string.IsNullOrEmpty(help) ? "No help is available for this command." : help));
                sb.Append(ConsoleBase.NewLine);
            }

            sb.Append(FormatHelpTextMethod("-----"));
            sb.Append(ConsoleBase.NewLine);

            WriteHelpMethod(sb.ToString());
        }
    }
}