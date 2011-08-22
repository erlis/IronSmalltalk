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
    public class SystemImplementation
    {
        public SmalltalkSystem SmalltalkSystem { get; private set; }

        public NotifyingSortedSet<GlobalItem> GlobalItems { get; private set; }

        public IEnumerable<Global> Globals
        {
            get
            {
                return this.GlobalItems.Where(e => e is Global).Cast<Global>();
            }
        }

        public IEnumerable<Global> GlobalVariables
        {
            get
            {
                return this.Globals.Where(g => g.GlobalType == GlobalTypeEnum.Variable);
            }
        }

        public IEnumerable<Global> GlobalConstants
        {
            get
            {
                return this.Globals.Where(g => g.GlobalType == GlobalTypeEnum.Constant);
            }
        }

        public IEnumerable<Class> Classes
        {
            get
            {
                return this.GlobalItems.Where(e => e is Class).Cast<Class>();
            }
        }

        public IEnumerable<Pool> Pools
        {
            get
            {
                return this.GlobalItems.Where(e => e is Pool).Cast<Pool>();
            }
        }

        public SystemImplementation(SmalltalkSystem parent)
        {
            this.SmalltalkSystem = parent;
            this.GlobalItems = new NotifyingSortedSet<GlobalItem>();
        }

        public SystemImplementation(SmalltalkSystem parent, XmlNode xml, XmlNamespaceManager nsm)
            : this(parent)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();

            foreach (XmlNode node in xml.SelectNodes("si:Class", nsm))
                this.GlobalItems.Add(new Class(this, node, nsm));
            foreach (XmlNode node in xml.SelectNodes("si:Global", nsm))
                this.GlobalItems.Add(new Global(this, node, nsm));
            foreach (XmlNode node in xml.SelectNodes("si:Pool", nsm))
                this.GlobalItems.Add(new Pool(this, node, nsm));
        }


        public void Save(XmlWriter xml)
        {
            if (xml == null)
                throw new ArgumentNullException();
            xml.WriteStartElement(null, "SystemImplementation", "http://schemas.ironsmalltalk.org/version1.0/Tools/SystemImplementation.xsd");
            xml.WriteAttributeString("xmlns", null, null, "http://schemas.ironsmalltalk.org/version1.0/Tools/SystemImplementation.xsd");
            xml.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            xml.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "SystemImplementation.xsd");

            foreach (GlobalItem i in this.GlobalItems)
                i.Save(xml);

            xml.WriteEndElement();
        }
    }
}
