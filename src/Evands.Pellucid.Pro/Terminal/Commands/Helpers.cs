#region copyright
// <copyright file="Helpers.cs" company="Christopher McNeely">
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
using System.ComponentModel;
using System.Linq;
using Crestron.SimplSharp.Reflection;
using Evands.Pellucid.Terminal.Commands.Attributes;

namespace Evands.Pellucid.Terminal.Commands
{
    /// <summary>
    /// Contains various methods for quickly retrieving information from commands.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class Helpers
    {
        /// <summary>
        /// Gets the name of a command from the <see cref="CommandAttribute"/> associated with a <see cref="TerminalCommandBase"/> object.
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to get a name for.</param>
        /// <returns>A <see langword="string"/> with the name retrieved, or <see cref="String.Empty"/> if no name was found.</returns>        
        internal static string GetCommandNameFromAttribute(TerminalCommandBase command)
        {
            var name = string.Empty;
            var ca = command.GetType().GetCType().GetCustomAttributes(false).OfType<CommandAttribute>().FirstOrDefault();

            if (ca != null)
            {
                name = ca.Name;
            }

            return name;
        }

        /// <summary>
        /// Gets the help text from a <see cref="CommandAttribute"/> associated with a <see cref="TerminalCommandBase"/> object.
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to get the help text for.</param>
        /// <returns>A <see langword="string"/> with the help text, or <see cref="String.Empty"/> if no help text was found.</returns>        
        internal static string GetCommandHelpFromAttribute(TerminalCommandBase command)
        {
            var help = string.Empty;
            var ca = command.GetType().GetCType().GetCustomAttributes(false).OfType<CommandAttribute>().FirstOrDefault();

            if (ca != null)
            {
                help = ca.Help;
            }

            return help;
        }

        /// <summary>
        /// Gets the verbs associated with a specific <see cref="TerminalCommandBase"/> object.
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> object to get the verbs of.</param>
        /// <returns>A <see cref="Dictionary{MethodInfo, VerbAttribute}"/> with any verbs found.</returns>        
        internal static Dictionary<MethodInfo, VerbAttribute> GetVerbs(TerminalCommandBase command)
        {
            var result = new Dictionary<MethodInfo, VerbAttribute>();

            var methods = command.GetType().GetCType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            foreach (var m in methods)
            {
                var verb = m.GetCustomAttributes(false).OfType<VerbAttribute>().FirstOrDefault();
                if (verb != null)
                {
                    result.Add(m, verb);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a specific verb from a <see cref="TerminalCommandBase"/> by the verb's name.
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to retrieve the verb from.</param>
        /// <param name="verbName">The name of the verb to retrieve.</param>
        /// <returns>A <see cref="Dictionary{MethodInfo, VerbAttribute}"/> containing all the <see cref="MethodInfo"/> objects with the specified verb name.</returns>        
        internal static Dictionary<MethodInfo, VerbAttribute> GetVerb(TerminalCommandBase command, string verbName)
        {
            var result = new Dictionary<MethodInfo, VerbAttribute>();

            var methods = command.GetType().GetCType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            foreach (var m in methods)
            {
                var verb = m.GetCustomAttributes(false).OfType<VerbAttribute>().FirstOrDefault();
                if (verb != null && string.Equals(verb.Name, verbName, StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Add(m, verb);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the operands associated with a specific method.
        /// </summary>
        /// <param name="verbMethod">The <see cref="MethodInfo"/> object to check for <see cref="OperandAttribute"/>s.</param>
        /// <returns>A <see cref="Dictionary{ParameterInfo, OperandAttribute}"/> with any operands found.</returns>        
        internal static Dictionary<ParameterInfo, OperandAttribute> GetOperands(MethodInfo verbMethod)
        {
            var result = new Dictionary<ParameterInfo, OperandAttribute>();

            var parameters = verbMethod.GetParameters();

            foreach (var p in parameters)
            {
                var oa = p.GetCustomAttributes(false).OfType<OperandAttribute>().FirstOrDefault();
                if (oa != null)
                {
                    result.Add(p, oa);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the flags associated with a specific method.
        /// </summary>
        /// <param name="verbMethod">The <see cref="MethodInfo"/> object to check for <see cref="FlagAttribute"/>s.</param>
        /// <returns>A <see cref="Dictionary{ParameterInfo, FlagAttribute}"/> with any flags found.</returns>        
        internal static Dictionary<ParameterInfo, FlagAttribute> GetFlags(MethodInfo verbMethod)
        {
            var result = new Dictionary<ParameterInfo, FlagAttribute>();

            var parameters = verbMethod.GetParameters();

            foreach (var p in parameters)
            {
                var fa = p.GetCustomAttributes(false).OfType<FlagAttribute>().FirstOrDefault();
                if (fa != null)
                {
                    result.Add(p, fa);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the examples associated with a specific verb.
        /// </summary>
        /// <param name="command">The <see cref="TerminalCommandBase"/> to check for verbs.</param>
        /// <param name="verbName">The name of the verb to retrieve.</param>
        /// <returns>A list of <see cref="SampleAttribute"/>s.</returns>        
        internal static List<SampleAttribute> GetVerbExamples(TerminalCommandBase command, string verbName)
        {
            var result = new List<SampleAttribute>();

            var verbs = GetVerb(command, verbName);

            foreach (var vm in verbs.Keys)
            {
                var eas = vm.GetCustomAttributes(false).OfType<SampleAttribute>();
                foreach (var ea in eas)
                {
                    if (ea != null)
                    {
                        result.Add(ea);
                    }
                }
            }

            return result;
        }
    }
}