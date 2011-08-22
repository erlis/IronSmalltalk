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
using System.Xml;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Description
{
    public class Parameter : Specification
    {
        public Parameter(Message parent)
            :base(parent)
        {
        }

        public Parameter(Message parent, XmlNode xml, XmlNamespaceManager nsm)
            : base(parent)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();

            XmlAttribute attr = xml.SelectSingleNode("@name", nsm) as XmlAttribute;
            if (attr != null)
                this.Name = attr.Value;
            attr = xml.SelectSingleNode("@aliasing", nsm) as XmlAttribute;
            if (attr != null)
                this.Aliasing = (AliasingEnum)Enum.Parse(typeof(AliasingEnum), attr.Value, true);

            foreach (XmlAttribute node in xml.SelectNodes("sd:Protocol/@name", nsm))
                this.Protocols.Add(node.Value);
        }

        public string Name { get; set; }

        public AliasingEnum Aliasing { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public enum AliasingEnum
        {
            /// <summary>
            /// Unspecified; Reference is may or may not be retained as a result of this message.
            /// </summary>
            Unspecified,
            /// <summary>
            /// Reference to the parameter is never retained, directly or indirectly, as a result of this message.
            /// </summary>
            Uncaptured,
            /// <summary>
            /// Reference to the parameter is retained, directly or indirectly, as a result of this message.
            /// </summary>
            Captured
        }

        public override string ToString()
        {
            return "Parameter: " + this.Name;
        }

        public void Save(XmlWriter xml)
        {
            xml.WriteStartElement("Parameter");
            xml.WriteAttributeString("name", this.Name);
            if (this.Aliasing == AliasingEnum.Captured)
                xml.WriteAttributeString("aliasing", "captured");
            else if (this.Aliasing == AliasingEnum.Uncaptured)
                xml.WriteAttributeString("aliasing", "uncaptured");
            else
                xml.WriteAttributeString("aliasing", "unspecified");

            foreach (string p in this.Protocols)
            {
                xml.WriteStartElement("Protocol");
                xml.WriteAttributeString("name", p);
                xml.WriteEndElement();
            }

            xml.WriteEndElement();

        }
    }
}
