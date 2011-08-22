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
using IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Implementation;
using System.IO;

namespace IronSmalltalk.Tools.ClassLibraryBrowser.Definitions.Saving
{
    public abstract class FileOutWriterBase
    {
        private readonly SystemImplementation SystemImplementation;

        private readonly TextWriter Writer;

        protected FileOutWriterBase(SystemImplementation systemImplementation, TextWriter writer)
        {
            if (systemImplementation == null)
                throw new ArgumentNullException();
            if (writer == null)
                throw new ArgumentNullException();

            this.SystemImplementation = systemImplementation;
            this.Writer = writer;
        }

        #region Emitting Interchange File 

        protected virtual void EmitInterchangeFile()
        {
            // <interchangeFile> ::= <interchangeVersionIdentifier> ( <interchangeUnit> ) +

            // 1. Emit the version for the IF. 
            this.EmitVersion();
            // 2. Emit a header ... with comments about export time etc.
            this.EmitHeader();

            // 3. Emit class definitions.
            this.WriteNewLine();
            this.WriteComment("Class Definitions");
            this.WriteNewLine();
            foreach (Class cls in this.SystemImplementation.Classes)
            {
                this.EmitClassDefinition(cls);
                this.EmitAnnotations(cls.Annotations);
            }

            // 4. Emit global definitions.
            this.WriteNewLine();
            this.WriteComment("Global Definitions");
            this.WriteNewLine();
            foreach (Global global in this.SystemImplementation.Globals)
            {
                this.EmitGlobalDefinition(global);
                this.EmitAnnotations(global.Annotations);
            }

            // 5. Emit pool definitions and pool value definitions
            this.WriteNewLine();
            this.WriteComment("Pool Definitions");
            this.WriteNewLine();
            foreach (Pool pool in this.SystemImplementation.Pools)
            {
                this.EmitPoolDefinition(pool);
                this.EmitAnnotations(pool.Annotations);
                foreach (PoolValue value in pool.Values)
                {
                    this.EmitPoolValueDefinition(value);
                    this.EmitAnnotations(value.Annotations);
                }
            }

            // 6. Emit methods
            foreach (Class cls in this.SystemImplementation.Classes)
            {
                this.WriteNewLine();
                this.WriteComment(String.Format("{0} Class Methods", cls.Name));
                this.WriteNewLine();
                foreach (Method mth in cls.ClassMethods)
                {
                    this.EmitClassMethodDefinition(mth);
                    this.EmitAnnotations(mth.Annotations);
                }
                this.WriteNewLine();
                this.WriteComment(String.Format("{0} Instance Methods", cls.Name));
                this.WriteNewLine();
                foreach (Method mth in cls.InstanceMethods)
                {
                    this.EmitInstanceMethodDefinition(mth);
                    this.EmitAnnotations(mth.Annotations);
                }
            }

            // 7. Emit initializers
            this.WriteNewLine();
            this.WriteComment("Initializers (in execution sorted order)");
            this.WriteNewLine();
            this.EmitInitializers();
        }

        protected virtual void EmitAnnotations(Annotations annotations)
        {
            if (annotations == null)
                return;
            foreach (KeyValuePair<string, string> annotation in annotations)
                this.EmitAnnotation(annotation.Key, annotation.Value);
        }

        protected abstract void EmitAnnotation(string key, string value);

        protected abstract void EmitVersion();

        protected virtual void EmitHeader()
        {
            this.WriteRaw("\"");
            this.WriteNewLine();
            this.WriteRaw("Iron Smalltalk Source Code");
            this.WriteNewLine();
            this.WriteRaw(String.Format("Filed-Out: {0:yyyy-MM-dd HH:mm}", DateTime.Now).Replace("\"", "\"\""));
            this.WriteNewLine();
            this.WriteRaw(String.Format("Definition: {0}", this.SystemImplementation.SmalltalkSystem.FilePath).Replace("\"", "\"\""));
            this.WriteNewLine();
            this.WriteRaw(String.Format("Browser Version: {0}", this.GetType().Assembly.GetName().Version).Replace("\"", "\"\""));
            this.WriteNewLine();
            this.WriteRaw("\" ! ");
            this.WriteNewLine();
        }

        #region Emitting Definitions

        protected abstract void EmitClassDefinition(Class cls);

        protected abstract void EmitGlobalDefinition(Global global);

        protected abstract void EmitPoolDefinition(Pool pool);

        protected abstract void EmitPoolValueDefinition(PoolValue value);

        protected abstract void EmitClassMethodDefinition(Method method);

        protected abstract void EmitInstanceMethodDefinition(Method method);

        #endregion

        #region Emitting Initializers

        private class InitializableObjectComparer : IComparer<IInitializableObject>
        {
            public int Compare(IInitializableObject x, IInitializableObject y)
            {
                return x.CompareTo(y);
            }
        }

        private void EmitInitializers()
        {
            IEnumerable<IInitializableObject> initializables = this.SystemImplementation.Classes.Cast<IInitializableObject>()
                .Concat(this.SystemImplementation.Globals.Cast<IInitializableObject>())
                .Concat(this.SystemImplementation.Pools.SelectMany(p => p.Values.Cast<IInitializableObject>()));

            // Only those that really have a source 
            initializables = initializables.Where(i => !String.IsNullOrWhiteSpace(i.Initializer.Source));
            // And sort them by their sort key
            initializables = initializables.OrderBy(i => i, new InitializableObjectComparer());
            
            foreach (var initializable in initializables)
            {
                if (initializable is Class)
                    this.EmitClassInitializer((Class)initializable);
                else if (initializable is Global)
                    this.EmitGlobalInitializer((Global)initializable);
                else if (initializable is PoolValue)
                    this.EmitPoolValueInitializer((PoolValue)initializable);
                else
                    throw new InvalidCastException(String.Format("Did not expect: {0}", initializable.GetType()));
                this.EmitAnnotations(initializable.Initializer.Annotations);
            }
        }

        protected abstract void EmitClassInitializer(Class cls);

        protected abstract void EmitGlobalInitializer(Global global);

        protected abstract void EmitPoolValueInitializer(PoolValue value);

        #endregion

        #endregion

        #region Write Helpers

        protected void WriteRaw(string str)
        {
            this.Writer.Write(str);
        }

        protected void WriteNewLine()
        {
            this.Writer.WriteLine();
        }

        protected void WriteSpace()
        {
            this.WriteRaw(" ");
        }

        protected void WriteTab()
        {
            this.WriteRaw("\t");
        }

        protected void WriteElementSeparator()
        {
            this.WriteRaw("! ");
        }

        protected void WriteString(string str)
        {
            if (String.IsNullOrEmpty(str))
                return;
            if (str.Contains('!'))
                this.WriteRaw(str.Replace("!", "!!"));
            else
                this.WriteRaw(str);
        }

        protected void WriteIdentifier(string str)
        {
            // identifier ::= letter (letter | digit)*
            if (str == null)
                throw new ArgumentNullException();
            if (str.Length == 0)
                throw new ArgumentException("Requires identifier. String is empty.");
            if (!(Char.IsLetter(str[0]) || (str[0] == '_')))
                throw new ArgumentException("Requires identifier. Does not start with letter.");
            for (int i = 0; i < str.Length; i++)
			{
                char c = str[i];
			    if (!(Char.IsLetter(c) || (c == '_') || ((c >= '0') && (c <= '9'))))
                    throw new ArgumentException("Requires identifier. Does contain other than letter or digit.");
			}
            this.WriteString(str);
        }

        protected void WriteQuotedIdentifier(string str)
        {
            this.WriteRaw("'");
            this.WriteIdentifier(str);
            this.WriteRaw("'");
        }

        protected void WriteQuotedString(string str)
        {
            if (str == null)
                throw new ArgumentNullException();
            //quotedString ::= stringDelimiter stringBody stringDelimiter
            //stringBody ::= (nonStringDelimiter | (stringDelimiter stringDelimiter)*)
            //stringDelimiter ::= ’’’ "a single quote"
            //nonStringDelimiter ::= "any character except stringDelimiter"
            this.WriteRaw("'");
            if (str.Contains('\''))
                this.WriteString(str.Replace("'", "''"));
            else
                this.WriteString(str);
            this.WriteRaw("'");
        }

        protected void WriteHashedString(string str)
        {
            //hashedString ::= ’#’ quotedString
            this.WriteRaw("#");
            this.WriteQuotedString(str);
        }

        protected void WriteIdentifierList(IEnumerable<string> list)
        {
            //<identifierList> ::= stringDelimiter identifier* stringDelimiter
            if (list == null)
                throw new ArgumentNullException();
            this.WriteRaw("'");
            bool first = true;
            foreach(string str in list)
            {
                if (first)
                    first = false;
                else
                    this.WriteSpace();
                this.WriteIdentifier(str);
            }
            this.WriteRaw("'");
        }

        protected void WriteComment(string str)
        {
            if (str == null)
                throw new ArgumentNullException();
            //commentDelimiter ::= ’"’
            //nonCommentDelimiter::= "any character that is not a commentDelimiter "
            //comment := commentDelimiter nonCommentDelimiter * commentDelimiter
            this.WriteRaw("\" ");
            if (str.Contains('"'))
                this.WriteString(str.Replace("\"", "\"\""));
            else
                this.WriteString(str);
            this.WriteRaw(" \" ! ");
        }

        protected void WriteAnnotation(string key, string value)
        {
            this.WriteAnnotation(key, value, true);
        }

        protected void WriteAnnotation(string key, string value, bool ignoreEmpty)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (String.IsNullOrWhiteSpace(key))
                throw new ArgumentException("key");
            if (ignoreEmpty && String.IsNullOrWhiteSpace(value))
                return;

            //<annotation> ::= ’Annotation’ ’key:’ quotedString ’value:’ quotedString <elementSeparator>
            this.WriteString("Annotation key: ");
            this.WriteQuotedString(key);
            this.WriteNewLine();
            this.WriteTab();
            this.WriteString("value: ");
            this.WriteQuotedString(((value == null) ? "" : value));
            this.WriteElementSeparator();
            this.WriteNewLine();
            this.WriteNewLine();
        }

        #endregion
    }
}
