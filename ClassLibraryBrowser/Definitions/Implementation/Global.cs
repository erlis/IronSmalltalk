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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation
{
    public class Global : GlobalItem, IInitializableObject
    {
        public GlobalTypeEnum GlobalType { get; set; }
        public Initializer<Global> Initializer { get; private set; }
        public ISet<string> ImplementedProtocols { get; private set; }

        public Global(SystemImplementation parent)
            : base(parent)
        {
            this.GlobalType = GlobalTypeEnum.Variable;
            this.Initializer = new Initializer<Global>(this);
            this.ImplementedProtocols = new NotifyingSortedSet<string>();
        }

        public Global(SystemImplementation parent, XmlNode xml, XmlNamespaceManager nsm)
            : base(parent, xml, nsm)
        {
            this.GlobalType = GlobalTypeEnum.Variable;
            this.Initializer = new Initializer<Global>(this);
            this.ImplementedProtocols = new NotifyingSortedSet<string>();

            XmlAttribute attr = xml.SelectSingleNode("@type", nsm) as XmlAttribute;
            if (attr != null)
                this.GlobalType = (GlobalTypeEnum)Enum.Parse(typeof(GlobalTypeEnum), attr.Value, true);

            foreach (XmlAttribute att in xml.SelectNodes("si:ImplementedProtocols/si:Protocol/@name", nsm))
                this.ImplementedProtocols.Add(att.Value);

            XmlNode elem = xml.SelectSingleNode("si:Initializer", nsm);
            if (elem != null)
                this.Initializer = new Initializer<Global>(this, elem, nsm);
        }

        public override void Save(XmlWriter xml)
        {
            xml.WriteStartElement("Global");
            xml.WriteAttributeString("name", this.Name);
            if (!String.IsNullOrWhiteSpace(this.DefiningProtocol))
                xml.WriteAttributeString("definingProtocol", this.DefiningProtocol);
            if (this.GlobalType == GlobalTypeEnum.Constant)
                xml.WriteAttributeString("type", "constant");
            else
                xml.WriteAttributeString("type", "variable");

            xml.WriteStartElement("ImplementedProtocols");
            foreach (string p in this.ImplementedProtocols)
            {
                xml.WriteStartElement("Protocol");
                xml.WriteAttributeString("name", p);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();

            this.Initializer.Save(xml);

            this.Description.SaveXml(xml, "Description");

            this.Annotations.Save(xml);

            xml.WriteEndElement();
        }

        IInitializer IInitializableObject.Initializer
        {
            get { return this.Initializer; }
        }

        int IInitializableObject.CompareTo(IInitializableObject other)
        {
            return this.Initializer.SortKey.CompareTo(other.Initializer.SortKey);
        }
    }

    public enum GlobalTypeEnum
    {
        Variable,
        Constant
    }
}
