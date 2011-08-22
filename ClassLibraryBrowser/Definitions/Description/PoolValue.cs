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
    public abstract class PoolValue
    {
        public GlobalPool Pool { get; private set; }

        public string Name { get; set; }

        public string Initializer { get; set; }

        public PoolValue(GlobalPool parent)
        {
            if (parent == null)
                throw new ArgumentNullException();
            this.Pool = parent;
        }

        public PoolValue(GlobalPool parent, XmlNode xml, XmlNamespaceManager nsm)
            : this(parent)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();

            XmlAttribute attr = xml.SelectSingleNode("@name", nsm) as XmlAttribute;
            if (attr != null)
                this.Name = attr.Value;

            XmlElement elem = xml.SelectSingleNode("sd:Initializer", nsm) as XmlElement;
            if (elem != null)
                this.Initializer = elem.InnerText;
        }

        public void Save(XmlWriter xml)
        {
            xml.WriteStartElement(this.XmlElementName);
            xml.WriteAttributeString("name", this.Name);

            if (!String.IsNullOrWhiteSpace(this.Initializer))
                xml.WriteElementString("Initializer", this.Initializer);

            xml.WriteEndElement();
        }

        protected abstract string XmlElementName { get; }
    }

    public class PoolVariable : PoolValue
    {
        public PoolVariable(GlobalPool parent)
            : base(parent)
        {
        }

        public PoolVariable(GlobalPool parent, XmlNode xml, XmlNamespaceManager nsm)
            : base(parent, xml, nsm)
        {
        }

        protected override string XmlElementName
        {
            get { return "Variable"; }
        }
    }

    public class PoolConstant : PoolValue
    {
        public PoolConstant(GlobalPool parent)
            : base(parent)
        {
        }

        public PoolConstant(GlobalPool parent, XmlNode xml, XmlNamespaceManager nsm)
            : base(parent, xml, nsm)
        {
        }

        protected override string XmlElementName
        {
            get { return "Constant"; }
        }
    }
}
