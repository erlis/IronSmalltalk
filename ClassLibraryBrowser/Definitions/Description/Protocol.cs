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
    public class Protocol
    {
        /// <summary>
        /// Chapter / Section of this Protocol in the ANSI 319-1998 document
        /// </summary>
        public string DocumentationId { get; set; }

        /// <summary>
        /// Abstract protocols must have name stating with lower case letter
        /// Non-abstract protocols (i.e. concrete protocols) have names starting
        /// with uppercase letter. They must implement corresponding global (class).
        /// </summary>
        public bool IsAbstract { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// HTML formatted description
        /// </summary>
        public HtmlString Description { get; set; }

        public IList<string> ConformsTo { get; private set; }

        public IList<Message> Messages { get; private set; }

        public IList<Global> StandardGlobals { get; private set; }

        public SystemDescription SystemDescription { get; private set; }

        public IEnumerable<string> AllConformsTo()
        {
            List<string> result = new List<string>();
            this.AddConformsTo(result);
            return result;
        }

        private void AddConformsTo(List<string> result)
        {
            foreach (string pn in this.ConformsTo)
            {
                if (!result.Contains(pn))
                {
                    result.Add(pn);
                    Protocol p = this.SystemDescription.Protocols.FirstOrDefault(pc => pc.Name == pn);
                    if (p != null)
                        p.AddConformsTo(result);
                }
            }
        }

        public Protocol(SystemDescription parent)
        {
            this.SystemDescription = parent;
            this.ConformsTo = new NotifyingCollection<string>();
            this.Messages = new NotifyingCollection<Message>();
            this.StandardGlobals = new NotifyingCollection<Global>();
        }

        public Protocol(SystemDescription parent, XmlNode xml, XmlNamespaceManager nsm)
            : this(parent)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();

            XmlAttribute attr = xml.SelectSingleNode("@name", nsm) as XmlAttribute;
            if (attr != null)
                this.Name = attr.Value;
            attr = xml.SelectSingleNode("@docId", nsm) as XmlAttribute;
            if (attr != null)
                this.DocumentationId = attr.Value;
            attr = xml.SelectSingleNode("@abstract", nsm) as XmlAttribute;
            if (attr != null)
                this.IsAbstract = Boolean.Parse(attr.Value);

            foreach (XmlAttribute node in xml.SelectNodes("sd:ConformsTo/@protocol", nsm))
                this.ConformsTo.Add(node.Value);
            foreach (XmlElement node in xml.SelectNodes("sd:Description", nsm))
                this.Description = new HtmlString(node);
            foreach (XmlElement node in xml.SelectNodes("sd:StandardGlobal", nsm))
                this.StandardGlobals.Add(new Global(this, node, nsm));
            foreach (XmlElement node in xml.SelectNodes("sd:Message", nsm))
                this.Messages.Add(new Message(this, node, nsm));
        }

        public override string ToString()
        {
            return "Protocol: " + this.Name;
        }

        public void Save(XmlWriter xml)
        {
            xml.WriteStartElement("Protocol");
            xml.WriteAttributeString("name", this.Name);
            if (!String.IsNullOrWhiteSpace(this.DocumentationId))
                xml.WriteAttributeString("docId", this.DocumentationId);
            xml.WriteAttributeString("abstract", this.IsAbstract ? "true" : "false");

            foreach (string p in this.ConformsTo)
            {
                xml.WriteStartElement("ConformsTo");
                xml.WriteAttributeString("protocol", p);
                xml.WriteEndElement();
            }
            this.Description.SaveXml(xml, "Description");
            foreach (Global g in this.StandardGlobals)
                g.Save(xml);
            foreach (Message m in this.Messages)
                m.Save(xml);

            xml.WriteEndElement();
        }
    }
}
