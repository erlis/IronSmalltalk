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
    public class Class : GlobalItem, IInitializableObject
    {
        public string SuperclassName { get; set; }
        public InstanceStateEnum InstanceState { get; set; }
        public ISet<string> InstanceVariables { get; private set; }
        public ISet<string> ClassVariables { get; private set; }
        public ISet<string> ClassInstanceVariables { get; private set; }
        public ISet<string> SharedPools { get; private set; }
        public Initializer<Class> Initializer { get; private set; }

        public ISet<Method> InstanceMethods { get; private set; }
        public ISet<Method> ClassMethods { get; private set; }

        public ISet<string> ImplementedInstanceProtocols { get; private set; }
        public ISet<string> ImplementedClassProtocols { get; private set; }

        public Class(SystemImplementation parent)
            : base(parent)
        {
            this.InstanceState = InstanceStateEnum.NamedObjectVariables;
            this.InstanceVariables = new NotifyingSortedSet<string>();
            this.ClassVariables = new NotifyingSortedSet<string>();
            this.ClassInstanceVariables = new NotifyingSortedSet<string>();
            this.SharedPools = new NotifyingSortedSet<string>();
            this.InstanceMethods = new NotifyingHashSet<Method>();
            this.ClassMethods = new NotifyingHashSet<Method>();
            this.Initializer = new Initializer<Class>(this);
            this.ImplementedInstanceProtocols = new NotifyingSortedSet<string>();
            this.ImplementedClassProtocols = new NotifyingSortedSet<string>();
        }

        public Class(SystemImplementation parent, XmlNode xml, XmlNamespaceManager nsm)
            : base(parent, xml, nsm)
        {
            this.InstanceState = InstanceStateEnum.NamedObjectVariables;
            this.InstanceVariables = new NotifyingSortedSet<string>();
            this.ClassVariables = new NotifyingSortedSet<string>();
            this.ClassInstanceVariables = new NotifyingSortedSet<string>();
            this.SharedPools = new NotifyingSortedSet<string>();
            this.InstanceMethods = new NotifyingHashSet<Method>();
            this.ClassMethods = new NotifyingHashSet<Method>();
            this.Initializer = new Initializer<Class>(this);
            this.ImplementedInstanceProtocols = new NotifyingSortedSet<string>();
            this.ImplementedClassProtocols = new NotifyingSortedSet<string>();

            XmlAttribute attr = xml.SelectSingleNode("@instanceState", nsm) as XmlAttribute;
            if (attr != null)
                this.InstanceState = (InstanceStateEnum)Enum.Parse(typeof(InstanceStateEnum), attr.Value, true);
            attr = xml.SelectSingleNode("@superclassName", nsm) as XmlAttribute;
            if (attr != null)
                this.SuperclassName = attr.Value.Trim();

            foreach (XmlAttribute att in xml.SelectNodes("si:SharedPools/si:Pool/@name", nsm))
                this.SharedPools.Add(att.Value);
            foreach (XmlAttribute att in xml.SelectNodes("si:ImplementedInstanceProtocols/si:Protocol/@name", nsm))
                this.ImplementedInstanceProtocols.Add(att.Value);
            foreach (XmlAttribute att in xml.SelectNodes("si:ImplementedClassProtocols/si:Protocol/@name", nsm))
                this.ImplementedClassProtocols.Add(att.Value);
            foreach (XmlAttribute att in xml.SelectNodes("si:InstanceVariables/si:Variable/@name", nsm))
                this.InstanceVariables.Add(att.Value);
            foreach (XmlAttribute att in xml.SelectNodes("si:ClassVariables/si:Variable/@name", nsm))
                this.ClassVariables.Add(att.Value);
            foreach (XmlAttribute att in xml.SelectNodes("si:ClassInstanceVariables/si:Variable/@name", nsm))
                this.ClassInstanceVariables.Add(att.Value);
            foreach (XmlAttribute att in xml.SelectNodes("si:SharedPools/si:Variable/@name", nsm))
                this.SharedPools.Add(att.Value);

            foreach (XmlNode node in xml.SelectNodes("si:InstanceMethods/si:Method", nsm))
                this.InstanceMethods.Add(new Method(this, node, nsm));
            foreach (XmlNode node in xml.SelectNodes("si:ClassMethods/si:Method", nsm))
                this.ClassMethods.Add(new Method(this, node, nsm));

            XmlNode elem = xml.SelectSingleNode("si:Initializer", nsm);
            if (elem != null)
                this.Initializer = new Initializer<Class>(this, elem, nsm);
        }

        public override void Save(XmlWriter xml)
        {
            xml.WriteStartElement("Class");
            xml.WriteAttributeString("name", this.Name);
            if (this.InstanceState == InstanceStateEnum.ByteIndexable)
                xml.WriteAttributeString("instanceState", "byteIndexable");
            else if (this.InstanceState == InstanceStateEnum.ObjectIndexable)
                xml.WriteAttributeString("instanceState", "objectIndexable");
            else if (this.InstanceState == InstanceStateEnum.Native)
                xml.WriteAttributeString("instanceState", "native");
            else
                xml.WriteAttributeString("instanceState", "namedObjectVariables");
            if (!String.IsNullOrWhiteSpace(this.DefiningProtocol))
                xml.WriteAttributeString("definingProtocol", this.DefiningProtocol);
            if (!String.IsNullOrWhiteSpace(this.SuperclassName))
                xml.WriteAttributeString("superclassName", this.SuperclassName);


            xml.WriteStartElement("ImplementedInstanceProtocols");
            foreach (string p in this.ImplementedInstanceProtocols)
            {
                xml.WriteStartElement("Protocol");
                xml.WriteAttributeString("name", p);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();

            xml.WriteStartElement("ImplementedClassProtocols");
            foreach (string p in this.ImplementedClassProtocols)
            {
                xml.WriteStartElement("Protocol");
                xml.WriteAttributeString("name", p);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();

            xml.WriteStartElement("InstanceVariables");
            foreach (string n in this.InstanceVariables)
            {
                xml.WriteStartElement("Variable");
                xml.WriteAttributeString("name", n);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();

            xml.WriteStartElement("ClassVariables");
            foreach (string n in this.ClassVariables)
            {
                xml.WriteStartElement("Variable");
                xml.WriteAttributeString("name", n);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();

            xml.WriteStartElement("ClassInstanceVariables");
            foreach (string n in this.ClassInstanceVariables)
            {
                xml.WriteStartElement("Variable");
                xml.WriteAttributeString("name", n);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();

            xml.WriteStartElement("SharedPools");
            foreach (string n in this.SharedPools)
            {
                xml.WriteStartElement("Pool");
                xml.WriteAttributeString("name", n);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();

            xml.WriteStartElement("InstanceMethods");
            foreach (Method m in this.InstanceMethods)
                m.Save(xml);
            xml.WriteEndElement();

            xml.WriteStartElement("ClassMethods");
            foreach (Method m in this.ClassMethods)
                m.Save(xml);
            xml.WriteEndElement();

            this.Initializer.Save(xml);

            this.Description.SaveXml(xml, "Description");

            this.Annotations.Save(xml);

            xml.WriteEndElement();
        }

        public IEnumerable<Class> AllSuperclasses()
        {
            List<Class> result = new List<Class>();
            Class cls = this;
            do
            {
                cls = this.Parent.Classes.FirstOrDefault(c => !String.IsNullOrWhiteSpace(cls.SuperclassName) && (c.Name == cls.SuperclassName));
                if (cls != null)
                    result.Add(cls);
            } while (cls != null) ;
            return result;
        }

        public IEnumerable<Class> WithAllSuperclasses()
        {
            List<Class> result = new List<Class>();
            Class cls = this;
            do
            {
                result.Add(cls);
                cls = this.Parent.Classes.FirstOrDefault(c => !String.IsNullOrWhiteSpace(cls.SuperclassName) && (c.Name == cls.SuperclassName));
            } while (cls != null) ;
            return result;
        }

        public IEnumerable<Class> AllSubclasses()
        {
            List<Class> result = new List<Class>();
            this.AddAllSubclassesTo(result);
            return result;
        }

        private void AddAllSubclassesTo(List<Class> classes)
        {
            foreach (Class cls in this.Parent.Classes)
            {
                if (!String.IsNullOrEmpty(cls.SuperclassName) && (cls.SuperclassName == this.Name)) 
                {
                    if (!classes.Contains(cls))
                    {
                        classes.Add(cls);
                        cls.AddAllSubclassesTo(classes);
                    }
                }
            }
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

    public enum InstanceStateEnum
    {
        /// <summary>
        /// Variable number of unnamed instance variables containing binary data.
        /// </summary>
        /// <remarks>
        /// IronSmalltalk allows a binary object to have named instance variables.
        /// De-facto in .Net only byte[] makes sense for this type of object,
        /// because binary objects are primarily used when interfacing the OS.
        /// </remarks>
        ByteIndexable,
        /// <summary>
        /// Variable number of unnamed instance variables referencing other objects.
        /// </summary>
        /// <remarks>
        /// Indexable object may also have named instance variables.
        /// </remarks>
        ObjectIndexable,
        /// <summary>
        /// Object contains named instance variables only.
        /// </summary>
        NamedObjectVariables,
        /// <summary>
        /// Native .Net object mapped to a Smalltalk class.
        /// </summary>
        Native
    }
}
