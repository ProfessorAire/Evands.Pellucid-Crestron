#region copyright
// <copyright file="CommandHelpers.cs" company="Christopher McNeely">
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
using Crestron.SimplSharp.Reflection;
using Elegant.Pellucid.Attributes;
using System.ComponentModel;

namespace Elegant.Pellucid
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class CommandHelpers
    {
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

        internal static bool HasCommandAttribute(TerminalCommandBase command)
        {
            var ca = command.GetType().GetCType().GetCustomAttributes(false).OfType<CommandAttribute>().FirstOrDefault();
            return ca != null;
        }

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