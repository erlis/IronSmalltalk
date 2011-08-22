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
    public class SystemDescription
    {
        public SmalltalkSystem SmalltalkSystem { get; private set; }

        public NotifyingCollection<Protocol> Protocols { get; private set; }

        public string FilePath { get; private set; }

        public SystemDescription(SmalltalkSystem parent)
        {
            this.SmalltalkSystem = parent;
            this.Protocols = new NotifyingCollection<Protocol>();
        }

        public SystemDescription(SmalltalkSystem parent, XmlElement xml, XmlNamespaceManager nsm)
            : this(parent)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();

            foreach (XmlNode node in xml.SelectNodes("sd:Protocol", nsm))
                    this.Protocols.Add(new Protocol(this, node, nsm));
        }

        public void Save(XmlWriter xml)
        {
            if (xml == null)
                throw new ArgumentNullException();
            xml.WriteStartElement(null, "SystemDescription", "http://schemas.ironsmalltalk.org/version1.0/Tools/SystemDescription.xsd");
            xml.WriteAttributeString("xmlns", null, null, "http://schemas.ironsmalltalk.org/version1.0/Tools/SystemDescription.xsd");
            xml.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            xml.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "SystemDescription.xsd");

            foreach (Protocol p in this.Protocols)
                p.Save(xml);

            xml.WriteEndElement();
        }
    }
}
