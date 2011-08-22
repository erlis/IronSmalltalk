/*
 * **************************************************************************
 *
 * Copyright (c) The IronSmalltalk Project. 
 *
 * This source code is subject to terms and conditions of the 
 * license agreement found in the solution directory. 
 * See: $(SolutionDir)\License.htm ... in the root of this distribution.
 * By using this source code in any fashion, you are agreeing 
 * to be bound by the terms of the license agreement.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Controls.Classes
{
    public class MethodHelper
    {
        /// <summary>
        /// Build lists of interesting class, protocol and method relations.
        /// </summary>
        /// <param name="currentClass">Current class to build lists for.</param>
        /// <param name="includeUpToClass">Build the list form the <paramref name="currentClass">currentClass</paramref> up-to this class.</param>
        /// <param name="methodType">Type of methods to include in the lists (instance or class)</param>
        /// <param name="classesList">List of classes from <paramref name="currentClass">currentClass</paramref> up-to <paramref name="includeUpToClass">includeUpToClass</paramref></param>
        /// <param name="superclassesList">List of classes from <paramref name="includeUpToClass">includeUpToClass</paramref> (but not included) up-to the root class.</paramref></param>
        /// <param name="classesMap">Map of class-name to class-object of the classes in <paramref name="classesList">classesList</paramref></param>
        /// <param name="protocolNameClassesMap">Mep between the protocols involved in <paramref name="classesList">classesList</paramref> and the classes where they are implemented</param>
        /// <param name="methodNameClassesMap">Map between the methods involved in <paramref name="classesList">classesList</paramref> and the classes where they are implemented</param>
        /// <param name="methodNameProtocolNamesMap">Map between the methods defined by the protocols involved in <paramref name="classesList">classesList</paramref> and <paramref name="superclassesList">superclassesList</paramref> and the protocol where they are defined</param>
        /// <param name="protocolMap">>Map of protocol-name to protocol-object of the protocols in <paramref name="protocolNameClassesMap">protocolNameClassesMap</paramref></param>
        /// <param name="subclassResponsibilityMethods">Collection of methods that are subclass-responsibility (must be implemented in this class)</param>
        /// <param name="methodNameLocalImplementorsMap">Map of method names and list of local implementors of those relative to <paramref name="currentClass"/>currentClass</paramref>, but excluding the class itself.</param>
        public static void BuildLists(Class currentClass,
            Class includeUpToClass,
            MethodType methodType,
            out List<Class> classesList,
            out List<Class> superclassesList,
            out Dictionary<string, Class> classesMap,
            out Dictionary<string, List<Class>> protocolNameClassesMap,
            out Dictionary<string, List<Class>> protocolNameSuperslassesMap,
            out Dictionary<string, List<Class>> methodNameClassesMap,
            out Dictionary<string, List<Class>> methodNameSuperclassesMap,
            out Dictionary<string, List<string>> methodNameProtocolNamesMap,
            out Dictionary<string, List<string>> allMethodNameProtocolNamesMap,
            out Dictionary<string, Definitions.Description.Protocol> protocolMap,
            out HashSet<string> subclassResponsibilityMethods,
            out Dictionary<string, List<Class>> methodNameLocalImplementorsMap,
            out Dictionary<string, List<Class>> methodNameSuperImplementorsMap
            )
        {
            classesList = new List<Class>();
            superclassesList = new List<Class>();
            classesMap = new Dictionary<string, Class>();
            protocolMap = new Dictionary<string, Definitions.Description.Protocol>();
            protocolNameClassesMap = new Dictionary<string, List<Class>>();
            protocolNameSuperslassesMap = new Dictionary<string, List<Class>>();
            methodNameClassesMap = new Dictionary<string, List<Class>>();
            methodNameSuperclassesMap = new Dictionary<string, List<Class>>();
            methodNameProtocolNamesMap = new Dictionary<string, List<string>>();
            allMethodNameProtocolNamesMap = new Dictionary<string, List<string>>();
            subclassResponsibilityMethods = new HashSet<string>();
            methodNameLocalImplementorsMap = new Dictionary<string, List<Class>>();
            methodNameSuperImplementorsMap = new Dictionary<string, List<Class>>();

            if (currentClass == null)
                return;
            if (includeUpToClass == null)
                includeUpToClass = currentClass;

            // Build a list of classes up to the wanted class ... and map between names and classes
            List<Class> col = classesList;
            foreach (Class cls in currentClass.WithAllSuperclasses())
            {
                col.Add(cls);
                classesMap[cls.Name] = cls;
                if (cls == includeUpToClass)
                    col = superclassesList;
            }

            // Build a map between the protocols involved in <classesList> and the classes where they are implemented
            foreach (Class cls in classesList)
            {
                var protocols = (methodType == MethodType.Instance) ? cls.ImplementedInstanceProtocols : cls.ImplementedClassProtocols;
                foreach (string pn in protocols)
                {
                    if (!protocolNameClassesMap.ContainsKey(pn))
                        protocolNameClassesMap.Add(pn, new List<Class>());
                    protocolNameClassesMap[pn].Add(cls);
                }
            }

            // Build a map between the protocols involved in <superclassesList> and the classes where they are implemented
            foreach (Class cls in superclassesList)
            {
                var protocols = (methodType == MethodType.Instance) ? cls.ImplementedInstanceProtocols : cls.ImplementedClassProtocols;
                foreach (string pn in protocols)
                {
                    if (!protocolNameSuperslassesMap.ContainsKey(pn))
                        protocolNameSuperslassesMap.Add(pn, new List<Class>());
                    protocolNameSuperslassesMap[pn].Add(cls);
                }
            }

            // Build a map between the methods involved in <classesList> and the classes where they are implemented
            foreach (Class cls in classesList)
            {
                var mths = (methodType == MethodType.Instance) ? cls.InstanceMethods : cls.ClassMethods;
                foreach (Method mth in mths)
                {
                    if (!methodNameClassesMap.ContainsKey(mth.Selector))
                        methodNameClassesMap.Add(mth.Selector, new List<Class>());
                    methodNameClassesMap[mth.Selector].Add(cls);
                }
            }

            // Build a map between the methods involved in <superclassesList> and the classes where they are implemented
            foreach (Class cls in superclassesList)
            {
                var mths = (methodType == MethodType.Instance) ? cls.InstanceMethods : cls.ClassMethods;
                foreach (Method mth in mths)
                {
                    if (!methodNameSuperclassesMap.ContainsKey(mth.Selector))
                        methodNameSuperclassesMap.Add(mth.Selector, new List<Class>());
                    methodNameSuperclassesMap[mth.Selector].Add(cls);
                }
            }

            // Build a map between the methods defined by the protocols involved in <classesList> and the protocol where they are defined
            foreach (string pn in protocolNameClassesMap.Keys)
            {
                var prot = currentClass.Parent.SmalltalkSystem.SystemDescription.Protocols.FirstOrDefault(p => p.Name == pn);
                if (prot != null)
                {
                    if (!protocolMap.ContainsKey(pn))
                        protocolMap.Add(pn, prot);

                    foreach (var msg in prot.Messages)
                    {
                        if (!methodNameProtocolNamesMap.ContainsKey(msg.Selector))
                            methodNameProtocolNamesMap.Add(msg.Selector, new List<string>());
                        methodNameProtocolNamesMap[msg.Selector].Add(pn);
                    }
                }
            }

            // Build a map between the methods defined by the protocols involved in <classesList> and <superclassesList> the protocol where they are defined
            foreach (var pair in methodNameProtocolNamesMap)
                allMethodNameProtocolNamesMap[pair.Key] = new List<string>(pair.Value);
            foreach (string pn in protocolNameSuperslassesMap.Keys)
            {
                var prot = currentClass.Parent.SmalltalkSystem.SystemDescription.Protocols.FirstOrDefault(p => p.Name == pn);
                if (prot != null)
                {
                    if (!protocolMap.ContainsKey(pn))
                        protocolMap.Add(pn, prot);

                    foreach (var msg in prot.Messages)
                    {
                        if (!allMethodNameProtocolNamesMap.ContainsKey(msg.Selector))
                            allMethodNameProtocolNamesMap.Add(msg.Selector, new List<string>());
                        allMethodNameProtocolNamesMap[msg.Selector].Add(pn);
                    }
                }
            }

            // Build a list of method names that have a super implementor with #subclassResponsibility.
            Dictionary<string, bool?> srm = new Dictionary<string, bool?>();
            foreach (Class cls in currentClass.WithAllSuperclasses())
            {
                var mths = (methodType == MethodType.Instance) ? cls.InstanceMethods : cls.ClassMethods;
                foreach (Method mth in mths)
                {
                    bool? sr = null;
                    srm.TryGetValue(mth.Selector, out sr);
                    if (sr == null)
                    {
                        if (MethodHelper.IsSubclassResponsibility(mth))
                            sr = (cls != currentClass);
                        else if (MethodHelper.HasImplementation(mth))
                            sr = false;
                        else
                            sr = true; // Not really subcl.res., but a method without code, so this needs attention as well
                    }
                    srm[mth.Selector] = sr;
                }
            }
            foreach (KeyValuePair<string, bool?> pair in srm)
            {
                if (pair.Value ?? false)
                    subclassResponsibilityMethods.Add(pair.Key);
            }

            // Find out which methods have local implementors
            HashSet<string> methodNames = new HashSet<string>();
            methodNames.UnionWith(methodNameClassesMap.Keys);
            methodNames.UnionWith(methodNameProtocolNamesMap.Keys);
            foreach (string selector in methodNames)
                methodNameLocalImplementorsMap[selector] = new List<Class>();
            foreach (Class cls in currentClass.AllSubclasses().OrderBy(c => c.Name))
            {
                var mths = (methodType == MethodType.Instance) ? cls.InstanceMethods : cls.ClassMethods;
                foreach (Method mth in mths)
                {
                    if (methodNameLocalImplementorsMap.ContainsKey(mth.Selector))
                        methodNameLocalImplementorsMap[mth.Selector].Add(cls);
                }
            }

            // Find out which methods wiht super implementors
            foreach (string selector in methodNames)
                methodNameSuperImplementorsMap[selector] = new List<Class>();
            foreach (Class cls in includeUpToClass.AllSuperclasses())
            {
                var mths = (methodType == MethodType.Instance) ? cls.InstanceMethods : cls.ClassMethods;
                foreach (Method mth in mths)
                {
                    if (methodNameSuperImplementorsMap.ContainsKey(mth.Selector))
                        methodNameSuperImplementorsMap[mth.Selector].Add(cls);
                }
            }
        }

        public static bool IsSubclassResponsibility(Method mth)
        {
            if (mth == null)
                return false;
            if (String.IsNullOrWhiteSpace(mth.Source))
                return false;
            return mth.Source.Contains("subclassResponsibility");
        }

        /// <summary>
        /// Get (or try to get) the first and the best protocol that implements the given message.
        /// </summary>
        /// <param name="methodName">Selector of the message to look for.</param>
        /// <param name="methodType">Type of the method (instance / class) to look for.</param>
        /// <param name="cls">Class where to start looking for the method.</param>
        /// <param name="protocolName">Optional protocol that may contain the message.</param>
        /// <returns>Message definition for the given selector or null if one was not found.</returns>
        public static Definitions.Description.Message GetMessageForMethod(string methodName, MethodType methodType, Class cls, string protocolName)
        {
            if (String.IsNullOrWhiteSpace(methodName))
                throw new ArgumentNullException("methodName");
            if (methodType == null)
                throw new ArgumentNullException("methodType");
            if (cls == null)
                throw new ArgumentNullException("cls");

            while (cls != null)
            {
                Definitions.Description.Message msg = null;
                var prot = cls.Parent.SmalltalkSystem.SystemDescription.Protocols.FirstOrDefault(p => p.Name == protocolName);
                if (prot != null)
                    msg = prot.Messages.FirstOrDefault(m => m.Selector == methodName);
                if (msg != null)
                    return msg;

                List<Class> classesList;
                List<Class> superclassesList;
                Dictionary<string, Class> classesMap;
                Dictionary<string, List<Class>> protocolNameClassesMap;
                Dictionary<string, List<Class>> protocolNameSuperslassesMap;
                Dictionary<string, List<Class>> methodNameClassesMap;
                Dictionary<string, List<Class>> methodNameSuperclassesMap;
                Dictionary<string, List<string>> methodNameProtocolNamesMap;
                Dictionary<string, List<string>> allMethodNameProtocolNamesMap; 
                Dictionary<string, Definitions.Description.Protocol> protocolMap;
                HashSet<string> subclassResponsibilityMethods;
                Dictionary<string, List<Class>> methodNameLocalImplementorsMap;
                Dictionary<string, List<Class>> methodNameSuperImplementorsMap;
                MethodHelper.BuildLists(cls, cls, methodType, out classesList, out superclassesList, out classesMap,
                    out protocolNameClassesMap, out protocolNameSuperslassesMap, out methodNameClassesMap, 
                    out methodNameSuperclassesMap, out methodNameProtocolNamesMap, out allMethodNameProtocolNamesMap,
                    out protocolMap, out subclassResponsibilityMethods, out methodNameLocalImplementorsMap, 
                    out methodNameSuperImplementorsMap);
                List<string> pns;
                methodNameProtocolNamesMap.TryGetValue(methodName, out pns);
                if ((pns != null) && (pns.Count > 0))
                {
                    protocolMap.TryGetValue(pns[0], out prot);
                    if (prot != null)
                        msg = prot.Messages.FirstOrDefault(m => m.Selector == methodName);
                }
                if (msg != null)
                    return msg;

                if (String.IsNullOrWhiteSpace(cls.SuperclassName))
                    cls = null;
                else
                    cls = cls.Parent.Classes.FirstOrDefault(c => c.Name == cls.SuperclassName);
            }

            return null;
        }

        /// <summary>
        /// Generate method header from a selector.
        /// </summary>
        /// <param name="selector">Selector of the method.</param>
        /// <param name="message">Optional message description that describes the method.</param>
        /// <returns></returns>
        public static string BuildMethodHeader(string selector, Definitions.Description.Message message)
        {
            List<string> parts = MethodHelper.SplitSelectorParts(selector);

            StringBuilder str = new StringBuilder();
            int args = parts.Count;
            if ((args == 1) && !selector.Contains(':') && (selector.IndexOfAny("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_".ToCharArray()) != -1))
                args = 0;
            for (int i = 0; i < parts.Count; i++)
            {
                if (i != 0)
                    str.Append(" ");
                str.Append(parts[i]);
                if (i < args)
                {
                    str.Append(" ");
                    if ((message != null) && (i < message.Parameters.Count))
                        str.Append(message.Parameters[i].Name);
                    else
                        str.AppendFormat("arg{0}", i);
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// Determine if the method appears to have some implementation.
        /// </summary>
        /// <param name="mth"></param>
        /// <returns></returns>
        public static bool HasImplementation(Method mth)
        {
            if (mth == null)
                return false;
            if (String.IsNullOrWhiteSpace(mth.Source))
                return false;
            var parser = new Compiler.SemanticAnalysis.Parser();
            try
            {
                var method = parser.ParseMethod(new System.IO.StringReader(mth.Source));
                if (method == null)
                    return false;
                return !((method.Statements == null) && (method.Primitive == null) && (method.Temporaries.Count == 0));
            }
            catch
            {
                return false;
            }
        }


        private static List<string> SplitSelectorParts(string selector)
        {
            List<string> parts = new List<string>();
            while (!String.IsNullOrEmpty(selector))
            {
                int i = selector.IndexOf(':');
                if (i > 0)
                {
                    parts.Add(selector.Substring(0, i + 1));
                    selector = selector.Substring(i + 1);
                }
                else
                {
                    parts.Add(selector);
                    selector = String.Empty;
                }
            }
            return parts;
        }

        public static string BuildDocumentation(Definitions.Description.Message message, int indent)
        {
            if (message == null)
                return null;

            StringBuilder str = new StringBuilder();
            if (!String.IsNullOrWhiteSpace(message.DocumentationId))
            {
                str.AppendHeading(String.Format("{0} {1}{2}:",
                    message.DocumentationId,
                    message.Protocol.Name,
                    (message.IsRefined) ? " (refined)" : ""));
                str.AppendParagraph(String.Format("<strong>{0}</strong>",
                    MethodHelper.BuildMethodHeader(message.Selector, message)));
            }
            string tmp = message.Synopsis.Text;
            if (!String.IsNullOrWhiteSpace(tmp))
            {
                str.AppendHeading("Synopsys:");
                str.AppendParagraph(tmp);
            }

            tmp = message.DefinitionDescription.Text;
            if (!String.IsNullOrWhiteSpace(tmp) || !String.IsNullOrWhiteSpace(message.DefinitionProtocol))
            {
                str.AppendHeading(String.Format("Definition: {0}", message.DefinitionProtocol));
                if (!String.IsNullOrWhiteSpace(tmp))
                    str.AppendParagraph(tmp);
            }

            foreach (var pair in message.Refinement)
            {
                str.AppendHeading(String.Format("Refinement: {0}", pair.Key));
                str.AppendParagraph(pair.Value.Text);
            }

            foreach (var param in message.Parameters)
            {
                str.AppendHeading("Parameter:");
                str.AppendParagraph(String.Format("<strong>{0}</strong>: {1} - {2}",
                    param.Name, String.Join(", ", param.Protocols), param.Aliasing));
            }

            if (message.ReturnValue != null)
            {
                str.AppendHeading("Return Value:");
                str.AppendParagraph(String.Format("{0} - {1}{2} {2}",
                    String.Join(", ", message.ReturnValue.Protocols),
                    message.ReturnValue.Aliasing,
                    (String.IsNullOrWhiteSpace(message.ReturnValue.Description.Text) ? "" : ":"),
                    message.ReturnValue.Description.Text));
            }

            tmp = message.Errors.Text;
            if (!String.IsNullOrWhiteSpace(tmp))
            {
                str.AppendHeading("Errors:");
                str.AppendParagraph(tmp);
            }


            //StringBuilder str = new StringBuilder();
            //if (!String.IsNullOrWhiteSpace(message.DocumentationId))
            //{
            //    str.Indent(indent);
            //    str.AppendFormat("<p><strong>{0} {1}{2}:</strong></p>",
            //        message.DocumentationId,
            //        message.Protocol.Name,
            //        (message.IsRefined) ? " (refined)" : "");
            //    str.AppendMultiline(MethodHelper.BuildMethodHeader(message.Selector, message), indent+1);
            //    str.AppendSectionSeparator();
            //}
            //string tmp = message.Synopsis.Text;
            //if (!String.IsNullOrWhiteSpace(tmp))
            //{
            //    str.Indent(indent);
            //    str.Append("<p><strong>Synopsys:</strong></p>");
            //    str.AppendMultiline(tmp, indent+1);
            //    str.AppendSectionSeparator();
            //}

            //tmp = message.DefinitionDescription.Text;
            //if (!String.IsNullOrWhiteSpace(tmp) || !String.IsNullOrWhiteSpace(message.DefinitionProtocol))
            //{
            //    str.Indent(indent);
            //    str.AppendFormat("<p><strong>Definition: {0}</strong></p>", message.DefinitionProtocol);
            //    if (!String.IsNullOrWhiteSpace(tmp))
            //        str.AppendMultiline(tmp, indent+1);
            //    str.AppendSectionSeparator();
            //}

            //foreach (var pair in message.Refinement)
            //{
            //    str.Indent(indent);
            //    str.AppendFormat("<p><strong>Refinement: {0}</strong></p>", pair.Key);
            //    str.AppendMultiline(pair.Value.Text, indent + 1);
            //    str.AppendSectionSeparator();
            //}

            //foreach (var param in message.Parameters)
            //{
            //    str.Indent(indent);
            //    str.Append("<p><strong>Parameter:</strong></p>");
            //    str.AppendMultiline(String.Format("<strong>{0}</strong>: {1} - {2}",
            //        param.Name, String.Join(", ", param.Protocols), param.Aliasing), indent+1);
            //    str.AppendSectionSeparator();
            //}

            //if (message.ReturnValue != null)
            //{
            //    str.Indent(indent);
            //    str.Append("<p><strong>Return Value:</strong>");
            //    str.AppendMultiline(String.Format("{0} - {1}{2} {2}",
            //        String.Join(", ", message.ReturnValue.Protocols),
            //        message.ReturnValue.Aliasing,
            //        (String.IsNullOrWhiteSpace(message.ReturnValue.Description.Text) ? "" : ":"),
            //        message.ReturnValue.Description.Text), indent + 1);
            //    str.AppendSectionSeparator();
            //}

            //tmp = message.Errors.Text;
            //if (!String.IsNullOrWhiteSpace(tmp))
            //{
            //    str.Indent(indent);
            //    str.Append("<p><strong>Errors:</strong></p>");
            //    str.AppendMultiline(tmp, indent + 1);
            //    str.AppendSectionSeparator();
            //}

            return str.ToString();
        }



    }

    public static class StringBuilderExtensions
    {
        public static void AppendSectionSeparator(this StringBuilder builder)
        {
            builder.AppendLine();
            builder.AppendLine();
        }

        public static void Indent(this StringBuilder builder, int indent)
        {
            for (int i = 0; i < indent; i++)
                builder.Append("    ");
        }

        public static void AppendMultiline(this StringBuilder builder, string str, int indent)
        {
            builder.AppendMultiline(str, indent, 120);
        }

        public static void AppendParagraph(this StringBuilder builder, string str)
        {
            builder.Append("<div>");
            builder.Append(str);
            builder.Append("</div>");
        }

        public static void AppendHeading(this StringBuilder builder, string str)
        {
            builder.Append("<h1 style=\"margin-bottom: 0px; font-family='Microsoft Sans Serif'; font-size: 12px; color: #0000C0\">");
            builder.Append(str);
            builder.Append("</h1>");
        }

        public static void AppendMultiline(this StringBuilder builder, string str, int indent, int lineLength)
        {
            if (str == null)
                return;
            do
            {
                builder.AppendLine();
                builder.Indent(indent);
                if (str.Length <= lineLength)
                {
                    builder.Append(str);
                    str = String.Empty;
                }
                else
                {
                    int idx = str.LastIndexOf(' ', Math.Min(lineLength, str.Length - 1));
                    if (idx == -1)
                        idx = str.IndexOf(' ', Math.Min(lineLength, str.Length - 1));
                    if (idx == -1)
                        idx = str.Length;
                    builder.Append(str.Substring(0, idx).Trim());
                    if (idx >= str.Length)
                        str = String.Empty;
                    else
                        str = str.Substring(idx + 1).Trim();
                }
            } while (str.Length > 0);
        }
    }



    public class MethodType
    {
        public static readonly MethodType Instance = new MethodType();
        public static readonly MethodType Class = new MethodType();

        private MethodType()
        {
        }

        public override string ToString()
        {
            return (this == MethodType.Class) ? "Class" : "Instance";
        }
    }
}
