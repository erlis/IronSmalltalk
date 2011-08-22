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
    public abstract class GlobalItem : Definition<SystemImplementation> , IComparable<GlobalItem>, IComparable
    {
        public string Name { get; set; }

        public string DefiningProtocol { get; set; }

        public HtmlString Description { get; set; }

        public GlobalItem(SystemImplementation parent)
            : base(parent)
        {
        }

        public GlobalItem(SystemImplementation parent, XmlNode xml, XmlNamespaceManager nsm)
            : base(parent, xml, nsm)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();

            XmlAttribute attr = xml.SelectSingleNode("@name", nsm) as XmlAttribute;
            if (attr != null)
                this.Name = attr.Value.Trim();
            attr = xml.SelectSingleNode("@definingProtocol", nsm) as XmlAttribute;
            if (attr != null)
                this.DefiningProtocol = attr.Value.Trim();

            XmlNode elem = xml.SelectSingleNode("si:Description", nsm) as XmlElement;
            if (elem != null)
                this.Description = new HtmlString(elem);
        }

        public abstract void Save(XmlWriter xml);

        int IComparable<GlobalItem>.CompareTo(GlobalItem other)
        {
            if (other == null)
                throw new ArgumentNullException();
            return String.Compare(this.Name, other.Name, StringComparison.InvariantCulture);
        }

        int IComparable.CompareTo(object obj)
        {
            if (!(obj is GlobalItem))
                throw new ArgumentException();
            return String.Compare(this.Name, ((GlobalItem) obj).Name, StringComparison.InvariantCulture);
        }
    }
}
