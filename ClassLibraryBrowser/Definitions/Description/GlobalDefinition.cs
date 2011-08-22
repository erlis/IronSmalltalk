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
    public abstract class GlobalDefinition
    {
        public Global Global { get; private set; }

        public GlobalDefinition(Global parent)
        {
            if (parent == null)
                throw new ArgumentNullException();
            this.Global = parent;
        }

        public abstract void Save(XmlWriter xml);
    }

    public class GlobalClass : GlobalDefinition
    {
        public IList<string> InstanceVariables { get; private set; }
        public IList<string> ClassVariables { get; private set; }
        public IList<string> ClassInstanceVariables { get; private set; }
        public IList<string> ImportedPools { get; private set; }
        public string Superclass { get; set; }
        public InstanceStateEnum InstanceState { get; set; }

        public enum InstanceStateEnum
        {
            None,
            Byte,
            Object
        }

        public GlobalClass(Global parent)
            : base(parent)
        {
            this.ClassInstanceVariables = new NotifyingCollection<string>();
            this.ClassVariables = new NotifyingCollection<string>();
            this.ImportedPools = new NotifyingCollection<string>();
            this.InstanceVariables = new NotifyingCollection<string>();
        }

        public GlobalClass(Global parent, XmlNode xml, XmlNamespaceManager nsm)
            : this(parent)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();

            XmlAttribute attr = xml.SelectSingleNode("@superclass", nsm) as XmlAttribute;
            if (attr != null)
                this.Superclass = attr.Value;
            attr = xml.SelectSingleNode("@instaneState", nsm) as XmlAttribute;
            if (attr != null)
            {
                if (attr.Value == "byte")
                    this.InstanceState = InstanceStateEnum.Byte;
                else if (attr.Value == "object")
                    this.InstanceState = InstanceStateEnum.Object;
                else
                    this.InstanceState = InstanceStateEnum.None;
            }

            foreach (XmlAttribute node in xml.SelectNodes("sd:InstanceVariable/@name", nsm))
                this.InstanceVariables.Add(node.Value);
            foreach (XmlAttribute node in xml.SelectNodes("sd:ClassVariable/@name", nsm))
                this.ClassVariables.Add(node.Value);
            foreach (XmlAttribute node in xml.SelectNodes("sd:ClassInstanceVariable/@name", nsm))
                this.ClassInstanceVariables.Add(node.Value);
            foreach (XmlAttribute node in xml.SelectNodes("sd:ImportedPool/@name", nsm))
                this.ImportedPools.Add(node.Value);
        }

        public override void Save(XmlWriter xml)
        {
            xml.WriteStartElement("Class");
            if (!String.IsNullOrWhiteSpace(this.Superclass))
                xml.WriteAttributeString("superclass", this.Superclass);
            if (this.InstanceState == InstanceStateEnum.Byte)
                xml.WriteAttributeString("instaneState", "byte");
            else if (this.InstanceState == InstanceStateEnum.Object)
                xml.WriteAttributeString("instaneState", "object");
            else
                xml.WriteAttributeString("instaneState", "none");

            foreach (string vn in this.InstanceVariables)
            {
                xml.WriteStartElement("InstanceVariable");
                xml.WriteAttributeString("name", vn);
                xml.WriteEndElement();
            }
            foreach (string vn in this.ClassVariables)
            {
                xml.WriteStartElement("ClassVariable");
                xml.WriteAttributeString("name", vn);
                xml.WriteEndElement();
            }
            foreach (string vn in this.ClassInstanceVariables)
            {
                xml.WriteStartElement("ClassInstanceVariable");
                xml.WriteAttributeString("name", vn);
                xml.WriteEndElement();
            }
            foreach (string vn in this.InstanceVariables)
            {
                xml.WriteStartElement("ImportedPool");
                xml.WriteAttributeString("name", vn);
                xml.WriteEndElement();
            }

            xml.WriteEndElement();
        }
    }

    public class GlobalVariable : GlobalDefinition
    {
        public GlobalVariable(Global parent)
            : base(parent)
        {
        }

        public GlobalVariable(Global parent, XmlNode xml, XmlNamespaceManager nsm)
            : this(parent)
        {
        }

        public override void Save(XmlWriter xml)
        {
            xml.WriteElementString("Global", "");
        }
    }
    public class GlobalConstant : GlobalDefinition
    {
        public GlobalConstant(Global parent)
            : base(parent)
        {
        }

        public GlobalConstant(Global parent, XmlNode xml, XmlNamespaceManager nsm)
            : this(parent)
        {
        }

        public override void Save(XmlWriter xml)
        {
            xml.WriteElementString("Constant", "");
        }
    }

    public class GlobalPool : GlobalDefinition
    {
        public IList<PoolValue> Items { get; private set; }

        public GlobalPool(Global parent)
            : base(parent)
        {
            this.Items = new NotifyingCollection<PoolValue>();
        }

        public GlobalPool(Global parent, XmlNode xml, XmlNamespaceManager nsm)
            : this(parent)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();            

            foreach (XmlElement node in xml.SelectNodes("sd:Constant", nsm))
                this.Items.Add(new PoolConstant(this, node, nsm));
            foreach (XmlElement node in xml.SelectNodes("sd:Variable", nsm))
                this.Items.Add(new PoolVariable(this, node, nsm));
        }

        public override void Save(XmlWriter xml)
        {
            xml.WriteStartElement("Pool");

            foreach (PoolValue p in this.Items)
                p.Save(xml);

            xml.WriteEndElement();
        }
    }

}
