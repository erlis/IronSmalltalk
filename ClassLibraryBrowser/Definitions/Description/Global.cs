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
    public class Global
    {
        public string Name { get; set; }

        public HtmlString Description { get; set; }

        public Protocol Protocol { get; private set; }

        public GlobalDefinition Definition { get; set; }

        public string Initializer { get; set; }

        public Global(Protocol parent)
        {
            if (parent == null)
                throw new ArgumentNullException();
            this.Protocol = parent;
        }

        public Global(Protocol parent, XmlNode xml, XmlNamespaceManager nsm)
            : this(parent)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();

            XmlAttribute attr = xml.SelectSingleNode("@name", nsm) as XmlAttribute;
            if (attr != null)
                this.Name = attr.Value;

            XmlElement elem = xml.SelectSingleNode("sd:Description", nsm) as XmlElement;
            if (elem != null)
                this.Description = new HtmlString(elem);
            elem = xml.SelectSingleNode("sd:Class", nsm) as XmlElement;
            if (elem != null)
                this.Definition = new GlobalClass(this, elem, nsm);
            elem = xml.SelectSingleNode("sd:Constant", nsm) as XmlElement;
            if (elem != null)
                this.Definition = new GlobalConstant(this, elem, nsm);
            elem = xml.SelectSingleNode("sd:Pool", nsm) as XmlElement;
            if (elem != null)
                this.Definition = new GlobalPool(this, elem, nsm);
            elem = xml.SelectSingleNode("sd:Global", nsm) as XmlElement;
            if (elem != null)
                this.Definition = new GlobalVariable(this, elem, nsm);
            elem = xml.SelectSingleNode("sd:Initializer", nsm) as XmlElement;
            if (elem != null)
                this.Initializer = elem.InnerText;
        }

        public override string ToString()
        {
            return "Global: " + this.Name;
        }

        public void Save(XmlWriter xml)
        {
            xml.WriteStartElement("StandardGlobal");
            xml.WriteAttributeString("name", this.Name);

            this.Description.SaveXml(xml, "Description");
            if (this.Definition != null)
                this.Definition.Save(xml);
            if (!String.IsNullOrWhiteSpace(this.Initializer))
                xml.WriteElementString("Initializer", this.Initializer);

            xml.WriteEndElement();
        }
    }
}
