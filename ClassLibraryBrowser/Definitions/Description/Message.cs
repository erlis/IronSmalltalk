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
    public class Message
    {
        public string DocumentationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        /// Short description
        /// </summary>
        public HtmlString Synopsis { get; set; }

        /// <summary>
        /// Longer description
        /// </summary>
        public HtmlString DefinitionDescription { get; set; }

        public string DefinitionProtocol { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, HtmlString> Refinement { get; private set; }


        /// <summary>
        /// If refined, then the name is shown in italic.
        /// </summary>
        public bool IsRefined { get; set; }

        public IList<Parameter> Parameters { get; private set; }

        public ReturnValue ReturnValue { get; set; }

        public HtmlString Errors { get; set; }

        public string Source { get; set; }

        public Protocol Protocol { get; private set; }

        public Message(Protocol parent)
        {
            if (parent == null)
                throw new ArgumentNullException();
            this.Protocol = parent;
            this.Refinement = new Dictionary<string, HtmlString>();
            this.Parameters = new NotifyingCollection<Parameter>();
        }

        public Message(Protocol parent, XmlNode xml, XmlNamespaceManager nsm)
            : this(parent)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();

            XmlAttribute attr = xml.SelectSingleNode("@selector", nsm) as XmlAttribute;
            if (attr != null)
                this.Selector = attr.Value;
            attr = xml.SelectSingleNode("@docId", nsm) as XmlAttribute;
            if (attr != null)
                this.DocumentationId = attr.Value;
            attr = xml.SelectSingleNode("@refined", nsm) as XmlAttribute;
            if (attr != null)
                this.IsRefined = Boolean.Parse(attr.Value);

            XmlElement elem = xml.SelectSingleNode("sd:Synopsis", nsm) as XmlElement;
            if (elem != null)
                this.Synopsis = new HtmlString(elem);
            elem = xml.SelectSingleNode("sd:Definition", nsm) as XmlElement;
            if (elem != null)
            {
                this.DefinitionProtocol = elem.SelectSingleNode("@protocol").Value;
                this.DefinitionDescription = new HtmlString(elem);
            }
            elem = xml.SelectSingleNode("sd:Errors", nsm) as XmlElement;
            if (elem != null)
                this.Errors = new HtmlString(elem);
            elem = xml.SelectSingleNode("sd:Source", nsm) as XmlElement;
            if (elem != null)
                this.Source = elem.InnerText;
            elem = xml.SelectSingleNode("sd:ReturnValue", nsm) as XmlElement;
            if (elem != null)
                this.ReturnValue = new ReturnValue(this, elem, nsm);

            foreach (XmlElement node in xml.SelectNodes("sd:Parameter", nsm))
                this.Parameters.Add(new Parameter(this, node, nsm));
            foreach (XmlElement node in xml.SelectNodes("sd:Refinement", nsm))
                this.Refinement[node.SelectSingleNode("@protocol").Value] = new HtmlString(node);
        }

        public override string ToString()
        {
            return "Message: " + this.Selector;
        }

        public void Save(XmlWriter xml)
        {
            xml.WriteStartElement("Message");
            xml.WriteAttributeString("selector", this.Selector);
            if (!String.IsNullOrWhiteSpace(this.DocumentationId))
                xml.WriteAttributeString("docId", this.DocumentationId);
            xml.WriteAttributeString("refined", this.IsRefined ? "true" : "false");

            this.Synopsis.SaveXml(xml, "Synopsis", true);
            if (!String.IsNullOrWhiteSpace(this.DefinitionProtocol))
            {
                xml.WriteStartElement("Definition");
                xml.WriteAttributeString("protocol", this.DefinitionProtocol);
                xml.WriteString(this.DefinitionDescription.Html);
                xml.WriteEndElement();
            }
            foreach (KeyValuePair<string, HtmlString> pair in this.Refinement)
            {
                xml.WriteStartElement("Refinement");
                xml.WriteAttributeString("protocol", pair.Key);
                xml.WriteString(pair.Value.Html);
                xml.WriteEndElement();
            }
            foreach (Parameter p in this.Parameters)
                p.Save(xml);
            if (this.ReturnValue != null)
                this.ReturnValue.Save(xml);
            this.Errors.SaveXml(xml, "Errors");
            if (!String.IsNullOrWhiteSpace(this.Source))
                xml.WriteElementString("Source", this.Source);

            xml.WriteEndElement();
        }
    }
}
