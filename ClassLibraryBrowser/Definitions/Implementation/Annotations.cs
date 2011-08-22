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
    public class Annotations : IEnumerable<KeyValuePair<string, string>>
    {
        private readonly List<KeyValuePair<string, string>> _annotations = new List<KeyValuePair<string, string>>();

        public Annotations()
        {
        }

        public Annotations(XmlNode xml, XmlNamespaceManager nsm)
        {
            if (xml == null)
                throw new ArgumentNullException();
            if (nsm == null)
                throw new ArgumentNullException();

            foreach (XmlElement node in xml.SelectNodes("si:Annotations/si:Annotation", nsm))
            {
                string key = node.GetAttribute("key");
                string val = node.InnerText;
                if (!String.IsNullOrWhiteSpace(key))
                    this._annotations.Add(new KeyValuePair<string,string>(key, val));
            }
        }

        public void Add(string key, string value)
        {
            this.EnsureKeyNotNull(key);
            this._annotations.Add(new KeyValuePair<string, string>(key, value));
        }

        public void Replace(string key, string value)
        {
            this.Remove(key);
            this.Add(key, value);
        }

        public void Remove(string key)
        {
            foreach (var pair in this._annotations.Where(p => p.Key == key).ToArray())
                this._annotations.Remove(pair);
        }

        public bool Exists(string key)
        {
            foreach (var pair in this._annotations)
            {
                if (pair.Key == key)
                    return true;
            }
            return false;
        }

        public IEnumerable<string> Get(string key)
        {
            return this._annotations.Where(p => p.Key == key).Select(p => p.Value);
        }

        public string GetFirst(string key)
        {
            foreach (var pair in this._annotations)
            {
                if (pair.Key == key)
                    return pair.Value;
            }
            return null;
        }

        public int Count
        {
            get { return this._annotations.Count; }
        }

        public void Save(XmlWriter xml)
        {
            if (this._annotations.Count == 0)
                return;
            xml.WriteStartElement("Annotations");
            foreach (var pair in this._annotations)
            {
                xml.WriteStartElement("Annotation");
                xml.WriteAttributeString("key", pair.Key);
                xml.WriteString(pair.Value);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();
        }

        private void EnsureKeyNotNull(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");
        }

        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            return this._annotations.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this._annotations.GetEnumerator();
        }
    }
}
