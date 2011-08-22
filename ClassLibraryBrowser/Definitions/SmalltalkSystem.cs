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

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Definitions
{
    public class SmalltalkSystem
    {
        public string FilePath { get; private set; }

        public Definitions.Description.SystemDescription SystemDescription { get; private set; }

        public Definitions.Implementation.SystemImplementation SystemImplementation { get; private set; }

        public SmalltalkSystem()
        {
            this.SystemDescription = new Definitions.Description.SystemDescription(this);
            this.SystemImplementation = new Definitions.Implementation.SystemImplementation(this);
        }

        public SmalltalkSystem(XmlDocument doc, XmlNamespaceManager nsm, string path)
            : this(doc.SelectSingleNode("/ss:SmalltalkSystem", nsm) as XmlElement, nsm, path)
        {
        }

        public SmalltalkSystem(XmlElement xml, XmlNamespaceManager nsm, string path)
            : this()
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException();
            this.FilePath = path;

            XmlElement elem = xml.SelectSingleNode("sd:SystemDescription", nsm) as XmlElement;
            if (elem != null)
                this.SystemDescription = new Definitions.Description.SystemDescription(this, elem, nsm);

            elem = xml.SelectSingleNode("si:SystemImplementation", nsm) as XmlElement;
            if (elem != null)
                this.SystemImplementation = new Definitions.Implementation.SystemImplementation(this, elem, nsm);
        }

        private static XmlNamespaceManager CreateXmlNamespaceManager(XmlDocument doc)
        {
            if (doc == null)
                throw new ArgumentNullException();
            XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);
            nsm.AddNamespace("ss", "http://schemas.ironsmalltalk.org/version1.0/Tools/SmalltalkSystem.xsd");
            nsm.AddNamespace("sd", "http://schemas.ironsmalltalk.org/version1.0/Tools/SystemDescription.xsd");
            nsm.AddNamespace("si", "http://schemas.ironsmalltalk.org/version1.0/Tools/SystemImplementation.xsd");
            return nsm;
        }

        public static SmalltalkSystem LoadFrom(string filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
                throw new ArgumentNullException();

            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNamespaceManager nsm = SmalltalkSystem.CreateXmlNamespaceManager(doc);
            return new SmalltalkSystem(doc, nsm, filename);
        }

        public void Save(XmlWriter xml)
        {
            if (xml == null)
                throw new ArgumentNullException();
            xml.WriteStartDocument();
            xml.WriteStartElement(null, "SmalltalkSystem", "http://schemas.ironsmalltalk.org/version1.0/Tools/SmalltalkSystem.xsd");
            xml.WriteAttributeString("xmlns", null, null, "http://schemas.ironsmalltalk.org/version1.0/Tools/SmalltalkSystem.xsd");
            xml.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            xml.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "SmalltalkSystem.xsd");

            this.SystemDescription.Save(xml);
            this.SystemImplementation.Save(xml);

            xml.WriteEndElement();
            xml.WriteEndDocument();
        }
    }
}
