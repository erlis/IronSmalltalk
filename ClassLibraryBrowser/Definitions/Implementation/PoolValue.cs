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
    public class PoolValue : Definition<Pool>, IInitializableObject
    {
        public string Name { get; set; }
        public PoolValueTypeEnum PoolValueType { get; set; }
        public Initializer<PoolValue> Initializer { get; private set; }

        public PoolValue(Pool parent)
            : base(parent)
        {
            this.PoolValueType = PoolValueTypeEnum.Variable;
            this.Initializer = new Initializer<PoolValue>(this);
        }

        public PoolValue(Pool parent, XmlNode xml, XmlNamespaceManager nsm)
            : base(parent, xml, nsm)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();

            this.PoolValueType = PoolValueTypeEnum.Variable;
            this.Initializer = new Initializer<PoolValue>(this);

            XmlAttribute attr = xml.SelectSingleNode("@name", nsm) as XmlAttribute;
            if (attr != null)
                this.Name = attr.Value.Trim();
            attr = xml.SelectSingleNode("@type", nsm) as XmlAttribute;
            if (attr != null)
                this.PoolValueType = (PoolValueTypeEnum)Enum.Parse(typeof(PoolValueTypeEnum), attr.Value, true);

            XmlNode elem = xml.SelectSingleNode("si:Initializer", nsm);
            if (elem != null)
                this.Initializer = new Initializer<PoolValue>(this, elem, nsm);
        }

        public void Save(XmlWriter xml)
        {
            xml.WriteStartElement("PoolValue");
            xml.WriteAttributeString("name", this.Name);
            if (this.PoolValueType == PoolValueTypeEnum.Constant)
                xml.WriteAttributeString("type", "constant");
            else
                xml.WriteAttributeString("type", "variable");

            this.Initializer.Save(xml);

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

    public enum PoolValueTypeEnum
    {
        Variable,
        Constant
    }
}
