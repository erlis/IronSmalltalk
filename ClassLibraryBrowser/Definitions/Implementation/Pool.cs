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
    public class Pool : GlobalItem 
    {
        public ISet<PoolValue> Values { get; private set; }

        public Pool(SystemImplementation parent)
            : base(parent)
        {
            this.Values = new NotifyingHashSet<PoolValue>();
        }

        public Pool(SystemImplementation parent, XmlNode xml, XmlNamespaceManager nsm)
            : base(parent, xml, nsm)
        {
            this.Values = new NotifyingHashSet<PoolValue>();

            foreach (XmlNode node in xml.SelectNodes("si:PoolValue", nsm))
                this.Values.Add(new PoolValue(this, node, nsm));
        }

        public override void Save(XmlWriter xml)
        {
            xml.WriteStartElement("Pool");
            xml.WriteAttributeString("name", this.Name);
            if (!String.IsNullOrWhiteSpace(this.DefiningProtocol))
                xml.WriteAttributeString("definingProtocol", this.DefiningProtocol);

            foreach (PoolValue pv in this.Values)
                pv.Save(xml);

            this.Description.SaveXml(xml, "Description");

            this.Annotations.Save(xml);

            xml.WriteEndElement();
        }
    }
}
