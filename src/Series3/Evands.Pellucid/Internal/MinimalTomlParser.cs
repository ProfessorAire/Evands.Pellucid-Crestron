#region copyright
// <copyright file="MinimalTomlParser.cs" company="Christopher McNeely">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Evands.Pellucid.Internal.Parts;
#if SERIES4
using System.Reflection;
#else
using Crestron.SimplSharp.Reflection;
#endif

namespace Evands.Pellucid.Internal
{
    /// <summary>
    /// Provides minimal parsing for TOML configuration files used internally.
    /// </summary>
    internal class MinimalTomlParser
    {
        /// <summary>
        /// The regex used for parsing TOML files.
        /// </summary>
        private static Regex regex;

        /// <summary>
        /// Initializes static members of the <see cref="MinimalTomlParser"/> class.
        /// </summary>        
        static MinimalTomlParser()
        {
            regex = new Regex("^\\W?(?<key>[A-Za-z0-9_\\.-]*) ?= ?(?:[\"']?(?<value>[^\\[\\r\\n\"']*)[\"']?)?(?:\\[ ?(?<array>[^\\]]*?) ?\\])?$", RegexOptions.Multiline | RegexOptions.Compiled);
        }

        /// <summary>
        /// Serializes the provided object to disk.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="filePath">The file path to write the serialized object to.</param>        
        public static void SerializeToDisk(object obj, string filePath)
        {
            var contents = SerializeObject(obj);
            using (var f = Crestron.SimplSharp.CrestronIO.File.CreateText(filePath))
            {
                if (f != null)
                {
                    f.Write(contents.PrintLines(false));
                }
            }
        }

        /// <summary>
        /// Deserializes the contents of a file from disk as the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the object as.</typeparam>
        /// <param name="filePath">The file to deserialize from.</param>
        /// <returns>A value of the type T.</returns>        
        public static T DeserializeFromDisk<T>(string filePath) where T : new()
        {
            var contents = Crestron.SimplSharp.CrestronIO.File.ReadToEnd(filePath, Encoding.UTF8);
            return DeserializeObject<T>(contents);
        }

        /// <summary>
        /// Deserializes the contents of a string as the specified object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="contents">The string to deserialize from.</param>
        /// <returns>A value of the type T.</returns>
        public static T DeserializeObject<T>(string contents)
        {
#if SERIES4
            return (T)DeserializeTopLevelObject(typeof(T), contents);
#else
            return (T)DeserializeTopLevelObject(typeof(T).GetCType(), contents);
#endif
        }

        /// <summary>
        /// Serializes the specified object to an <see cref="IPrintToml"/> object.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>a <see cref="IPrintToml"/> object.</returns>        
        public static IPrintToml SerializeObject(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var t = obj.GetType();

            if (t == typeof(string))
            {
                return new TomlKeyValuePair<string>(string.Format("\"{0}\"", obj));
            }
            else if (t.IsEnum)
            {
                return SerializeEnum(obj, t);
            }
            else if (t.IsArray)
            {
                return SerializeArray(obj, t);
            }
            else if (obj is IList)
            {
                return SerializeList(obj, t);
            }
            else if (t.IsClass)
            {
                return SerializeClass(obj, t);
            }
            else
            {
                return new TomlKeyValuePair<string>(obj.ToString());
            }
        }

        /// <summary>
        /// Deserializes a top level object from a string.
        /// </summary>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="contents">The string to deserialize from.</param>
        /// <returns>A basic object.</returns>        
#if SERIES4
        private static object DeserializeTopLevelObject(Type type, string contents)
#else
        private static object DeserializeTopLevelObject(CType type, string contents)
#endif
        {
            var matches = regex.Matches(contents);
#if SERIES4
            var value = Activator.CreateInstance(type);
#else
            var value = Crestron.SimplSharp.Reflection.Activator.CreateInstance(type);
#endif

            var props = type.GetProperties().Where(p => p.IsDefined(typeof(TomlPropertyAttribute), true));

            for (var i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
#if SERIES4
                var prop = props.Where(p => ((TomlPropertyAttribute)p.GetCustomAttributes(typeof(TomlPropertyAttribute), true).FirstOrDefault() ?? new TomlPropertyAttribute(string.Empty)).Name == match.Groups["key"].Value).FirstOrDefault();
#else
                var prop = props.Where(p => ((TomlPropertyAttribute)p.GetCustomAttributes(typeof(TomlPropertyAttribute).GetCType(), true).FirstOrDefault() ?? new TomlPropertyAttribute(string.Empty)).Name == match.Groups["key"].Value).FirstOrDefault();
#endif
                if (prop != null)
                {
                    if (!match.Groups["array"].Success && match.Groups["value"].Success)
                    {
                        prop.SetValue(value, DeserializeObject(prop.PropertyType, match.Groups["value"].Value), null);
                    }
                    else if (match.Groups["array"].Success)
                    {
                        prop.SetValue(value, DeserializeObject(prop.PropertyType, match.Groups["array"].Value), null);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Deserializes an object from a string.
        /// </summary>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="contents">The string to deserialize from.</param>
        /// <returns>A basic object.</returns>        
#if SERIES4
        private static object DeserializeObject(Type type, string contents)
#else
        private static object DeserializeObject(CType type, string contents)
#endif
        {
            if (type.IsEnum)
            {
                return Enum.Parse(type, contents.Replace("\"", string.Empty), true);
            }
            else if (type.IsValueType)
            {
                return Convert.ChangeType(contents, type, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (type == typeof(string))
            {
                return contents;
            }
            else if (type.IsArray)
            {
                var items = contents.Split(',');
                var array = Array.CreateInstance(type, items.Length);
                for (var j = 0; j < items.Length; j++)
                {
                    array.SetValue(DeserializeObject(type.GetElementType(), items[j].Trim('"')), j);
                }

                return array;
            }
            else if (type.IsClass)
            {
                object value = null;
#if SERIES4
                Type listGen = null;
#else
                CType listGen = null;
#endif

                if (type.Name.StartsWith("List`1"))
                {
                    var par = type.GetGenericArguments();
                    if (par.Length > 0)
                    {
                        listGen = par[0];
                    }

                    if (par.Length > 0 && par[0] == typeof(string))
                    {
                        value = new List<string>();
                    }
                }

                var list = value as IList;
                if (list != null && listGen != null)
                {
                    string[] items = null;
                    if (contents.StartsWith("{"))
                    {
                        items = contents.Split('}');
                    }
                    else
                    {
                        items = contents.Split(',');
                    }

                    for (var j = 0; j < items.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(items[j].Trim()))
                        {
                            list.Add(DeserializeObject(listGen, items[j].Trim('"')));
                        }
                    }

                    return list;
                }
                else
                {
                    return DeserializeTopLevelObject(type, contents);
                }
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Serializes the specified object to an <see cref="IPrintToml"/> object, as an enumeration.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="type">The type of the enum to serialize.</param>
        /// <returns>a <see cref="IPrintToml"/> object.</returns>    
        private static IPrintToml SerializeEnum(object obj, Type type)
        {
#if SERIES4
            var flags = type.GetCustomAttributes(typeof(FlagsAttribute), true).Any();
#else
            var flags = type.GetCustomAttributes(typeof(FlagsAttribute).GetCType(), true).Any();
#endif
            if (flags)
            {
                var items = obj.ToString().Split(',');

                if (items.Length > 1)
                {
                    var children = new List<IPrintToml>();
                    for (var i = 0; i < items.Length; i++)
                    {
                        children.Add(new TomlKeyValuePair<string>(string.Format("{0}", items[i].Trim())));
                    }

                    return new TomlArray(children);
                }
            }

            return new TomlKeyValuePair<string>(string.Format("{0}", obj.ToString()));
        }

        /// <summary>
        /// Serializes the specified object to an <see cref="IPrintToml"/> object, as a list.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="type">The type of the object in the list to serialize.</param>
        /// <returns>a <see cref="IPrintToml"/> object.</returns>      
        private static TomlArray SerializeList(object obj, Type type)
        {
            var l = new List<IPrintToml>();
            var list = obj as IList;
            if (list != null)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    var value = SerializeObject(list[i]);
                    if (value != null)
                    {
                        l.Add(SerializeObject(list[i]));
                    }
                }
            }
            else
            {
                throw new InvalidCastException();
            }

            return new TomlArray(l);
        }

        /// <summary>
        /// Serializes the specified object to an <see cref="IPrintToml"/> object, as an array.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="type">The type of the object in the array to serialize.</param>
        /// <returns>a <see cref="IPrintToml"/> object.</returns> 
        private static TomlArray SerializeArray(object obj, Type type)
        {
            var l = new List<IPrintToml>();
            var array = obj as Array;
            if (array != null)
            {
                for (var i = 0; i < array.Length; i++)
                {
                    var item = array.GetValue(i);
                    l.Add(SerializeObject(item));
                }
            }
            else
            {
                throw new InvalidCastException();
            }

            return new TomlArray(l);
        }

        /// <summary>
        /// Serializes the specified object to an <see cref="IPrintToml"/> object, as a class.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="type">The type of the class to serialize.</param>
        /// <returns>a <see cref="IPrintToml"/> object.</returns> 
        private static TomlClass SerializeClass(object obj, Type type)
        {
#if SERIES4
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
#else
            var props = obj.GetType().GetCType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy);
#endif
            var l = new List<IPrintToml>();

            foreach (var prop in props)
            {
#if SERIES4
                if (prop.GetCustomAttributes(typeof(NonSerializedAttribute), true).Any())
#else
                if (prop.GetCustomAttributes(typeof(NonSerializedAttribute).GetCType(), true).Any())
#endif
                {
                    continue;
                }

                string name;

#if SERIES4
                var nameAttribute = (TomlPropertyAttribute)prop.GetCustomAttributes(typeof(TomlPropertyAttribute), true).FirstOrDefault();
#else
                var nameAttribute = (TomlPropertyAttribute)prop.GetCustomAttributes(typeof(TomlPropertyAttribute).GetCType(), true).FirstOrDefault();
#endif
                if (nameAttribute != null)
                {
                    name = nameAttribute.Name;
                }
                else
                {
                    continue;
                }

                var value = SerializeObject(prop.GetValue(obj, null));

                var named = value as INamedToml;
                if (named != null)
                {
                    named.Name = name;
                }

                if (value != null)
                {
                    l.Add(value);
                }
            }

            return new TomlClass(l);
        }

        /// <summary>
        /// Gets the name of the specified object, if it exists.
        /// </summary>
        /// <param name="obj">The name of the object.</param>
        /// <returns>A string representing the name of the object.</returns>        
        private static string GetName(object obj)
        {
#if SERIES4
            var t = obj.GetType();
#else
            var t = obj.GetType().GetCType();
#endif
            var atts = t.GetCustomAttributes(true);

            var propertyName = atts.OfType<TomlPropertyAttribute>().FirstOrDefault();
            if (propertyName != null)
            {
                return propertyName.Name;
            }

            return t.Name;
        }
    }
}