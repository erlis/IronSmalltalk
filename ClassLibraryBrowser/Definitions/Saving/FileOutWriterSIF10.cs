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
    public class FileOutWriterSIF10 : FileOutWriterBase
    {
        public static void FileOut(SystemImplementation systemImplementation, TextWriter writer)
        {
            FileOutWriterSIF10 fileout = new FileOutWriterSIF10(systemImplementation, writer);
            fileout.EmitInterchangeFile();
        }

        protected FileOutWriterSIF10(SystemImplementation systemImplementation, TextWriter writer)
            : base(systemImplementation, writer)
        {
        }

        #region Implementation 

        protected override void EmitVersion()
        {
            //<interchangeVersionIdentifier> ::=
            //    ’Smalltalk’ ’interchangeVersion:’ <versionId> <elementSeparator>
            //<versionId> ::= quotedString
            this.WriteString("Smalltalk interchangeVersion: ");
            this.WriteQuotedString(this.InterchangeVersionId);
            this.WriteElementSeparator();
            this.WriteNewLine();
            this.WriteNewLine();
        }

        protected virtual string InterchangeVersionId
        {
            get { return "1.0"; }
        }

        protected override void EmitClassDefinition(Class cls)
        {
            //<classDefinition> ::=
            //    ’Class’ ’named:’ <classNameString>
            //    ’superclass:’ <superclassNameString>
            //    ’indexedInstanceVariables:’ <indexableInstVarType>
            //    ’instanceVariableNames:’ <instanceVariableNames>
            //    ’classVariableNames:’ <classVariableList>
            //    ’sharedPools:’ <poolList>
            //    ’classInstanceVariableNames:’<classInstVariableList>
            //    <elementSeparator>      
            //<classNameString> ::= stringDelimiter <className> stringDelimiter
            //<superclassNameString> ::= stringDelimiter <className> stringDelimiter
            //<className> ::= identifier
            //<indexableInstVarType> ::= hashedString
            //<instanceVariableNames> ::= <identifierList>
            //<classVariableList> ::= <identifierList>
            //<classInstVariableList> ::= <identifierList>
            //<poolList> ::= <identifierList>
            //<identifierList> ::= stringDelimiter identifier* stringDelimiter

            //    ’Class’ ’named:’ <classNameString>
            this.WriteString("Class named: ");
            this.WriteQuotedIdentifier(cls.Name);
            this.WriteNewLine();

            //    ’superclass:’ <superclassNameString>
            this.WriteTab();
            this.WriteString("superclass: ");
            if (String.IsNullOrEmpty(cls.SuperclassName))
                this.WriteRaw("''"); // No superclass ... e.g. Object. NB: The X3J20 is unclear here!
            else
                this.WriteQuotedIdentifier(cls.SuperclassName);

            //    ’indexedInstanceVariables:’ <indexableInstVarType>
            this.WriteNewLine();
            this.WriteTab();
            this.WriteString("indexedInstanceVariables: ");
            this.WriteHashedString(this.GetInstanceState(cls));

            //    ’instanceVariableNames:’ <instanceVariableNames>
            this.WriteNewLine();
            this.WriteTab();
            this.WriteString("instanceVariableNames: ");
            this.WriteIdentifierList(cls.InstanceVariables);

            //    ’classVariableNames:’ <classVariableList>
            this.WriteNewLine();
            this.WriteTab();
            this.WriteString("classVariableNames: ");
            this.WriteIdentifierList(cls.ClassVariables);

            //    ’sharedPools:’ <poolList>
            this.WriteNewLine();
            this.WriteTab();
            this.WriteString("sharedPools: ");
            this.WriteIdentifierList(cls.SharedPools);

            //    ’classInstanceVariableNames:’<classInstVariableList>
            this.WriteNewLine();
            this.WriteTab();
            this.WriteString("classInstanceVariableNames: ");
            this.WriteIdentifierList(cls.ClassInstanceVariables);

            //    <elementSeparator>      
            this.WriteElementSeparator();
            this.WriteNewLine();
            this.WriteNewLine();

            this.WriteAnnotation("comment", cls.Description.Text);
            this.WriteAnnotation("ist.description", cls.Description.Html);
            this.WriteAnnotation("ist.defining-protocol", cls.DefiningProtocol);
            this.WriteAnnotation("ist.implemented-class-protocols", String.Join(", ", cls.ImplementedClassProtocols));
            this.WriteAnnotation("ist.implemented-instance-protocols", String.Join(", ", cls.ImplementedInstanceProtocols));   
        }

        protected virtual string GetInstanceState(Class cls)
        {
            switch (cls.InstanceState)
            {
                case InstanceStateEnum.ByteIndexable:
                    return "byte";
                case InstanceStateEnum.ObjectIndexable:
                    return "object";
                case InstanceStateEnum.NamedObjectVariables:
                default:
                    return "none";
            }
        }

        protected override void EmitGlobalDefinition(Global global)
        {
            //<globalDefinition> ::= <globalVariableDefinition> | <globalConstantDefinition>

            //<globalVariableDefinition> ::= ’Global’ ’variable:’ <globalNameString> <elementSeparator>
            //<globalConstantDefinition> ::= ’Global’ ’constant:’ <globalNameString> <elementSeparator>

            //<globalNameString> ::= stringDelimiter <globalName> stringDelimiter
            //<globalName> ::= identifier
            if(global.GlobalType == GlobalTypeEnum.Constant)
                this.WriteString("Global constant: ");
            else
                this.WriteString("Global variable: ");
            this.WriteQuotedIdentifier(global.Name);
            this.WriteElementSeparator();
            this.WriteNewLine();
            this.WriteNewLine();

            this.WriteAnnotation("comment", global.Description.Text);
            this.WriteAnnotation("ist.description", global.Description.Html);
            this.WriteAnnotation("ist.defining-protocol", global.DefiningProtocol);
            this.WriteAnnotation("ist.implemented-protocols", String.Join(", ", global.ImplementedProtocols));
        }

        protected override void EmitPoolDefinition(Pool pool)
        {
            //<poolDefinition> ::= ’Pool’ ’named:’ <poolNameString> <elementSeparator>
            //<poolNameString> ::= stringDelimiter <poolName> stringDelimiter
            this.WriteString("Pool named: ");
            this.WriteQuotedIdentifier(pool.Name);
            this.WriteElementSeparator();
            this.WriteNewLine();
            this.WriteNewLine();

            this.WriteAnnotation("comment", pool.Description.Text);
            this.WriteAnnotation("ist.description", pool.Description.Html);
        }

        protected override void EmitPoolValueDefinition(PoolValue value)
        {
            //<poolVariableDefinition> ::= <poolValueDefinition> | <poolConstantDefinition>
            //<poolValueDefinition> ::= <poolName> ’variable:’ <poolVariableNameString> <elementSeparator>
            //<poolConstantDefinition> ::= <poolName> ’constant:’ <poolVariableNameString> <elementSeparator>

            //<poolVariableNameString> ::= stringDelimiter <poolVariableName> stringDelimiter
            //<poolName> ::= identifier
            //<poolVariableName> ::= identifier
            this.WriteIdentifier(value.Parent.Name);
            if (value.PoolValueType == PoolValueTypeEnum.Constant)
                this.WriteString(" constant: ");
            else
                this.WriteString(" variable: ");
            this.WriteQuotedIdentifier(value.Name);
            this.WriteElementSeparator();
            this.WriteNewLine();
            this.WriteNewLine();
        }

        protected override void EmitClassMethodDefinition(Method method)
        {
            this.EmitMethodDefinition(method, "classMethod");
        }

        protected override void EmitInstanceMethodDefinition(Method method)
        {
            this.EmitMethodDefinition(method, "method");
        }

        private void EmitMethodDefinition(Method method, string type)
        {
            //<methodDefinition> ::= <className> ’method’ <elementSeparator>
            //  <method definition> <elementSeparator>
            //<classMethodDefinition> ::= <className> ’classMethod’ <elementSeparator>
            //  <method definition> <elementSeparator>
            this.WriteIdentifier(method.Parent.Name);
            this.WriteSpace();
            this.WriteString(type);
            this.WriteElementSeparator();
            this.WriteNewLine();
            this.WriteString(method.Source);
            this.WriteElementSeparator();
            this.WriteNewLine();
            this.WriteNewLine();
        }

        protected override void EmitClassInitializer(Class cls)
        {
            //<classInitializationr> ::= <className> ’initializer’ <elementSeparator>
            //<initializer definition> <elementSeparator>
            this.WriteIdentifier(cls.Name);
            this.WriteString(" initializer");
            this.WriteElementSeparator();
            this.WriteNewLine();

            this.EmitInitializer(cls.Initializer);
        }

        protected override void EmitGlobalInitializer(Global global)
        {
            //<globalValueInitialization> ::= <globalName> ’initializer’ <elementSeparator>
            //<variableInitializer> <elementSeparator>
            //<variableInitializer> ::= <initializer definition>
            this.WriteIdentifier(global.Name);
            this.WriteString(" initializer");
            this.WriteElementSeparator();
            this.WriteNewLine();

            this.EmitInitializer(global.Initializer);
        }

        protected override void EmitPoolValueInitializer(PoolValue value)
        {
            //<poolValueInitialization> ::= <poolName> ’initializerFor:’ <poolVariableNameString>
            //<elementSeparator> <variableInitializer> <elementSeparator>
            //<poolVariableNameString> ::= stringDelimiter <poolVariableName> stringDelimiter
            this.WriteIdentifier(value.Parent.Name);
            this.WriteString(" initializerFor: ");
            this.WriteQuotedIdentifier(value.Name);
            this.WriteElementSeparator();
            this.WriteNewLine();

            this.EmitInitializer(value.Initializer);
        }

        private void EmitInitializer<TParent>(Initializer<TParent> initializer)
        {
            //<initializer definition> <elementSeparator>
            if (initializer == null)
                throw new ArgumentNullException();

            this.WriteString(initializer.Source);
            this.WriteElementSeparator();
            this.WriteNewLine();
            this.WriteNewLine();
        }

        protected override void EmitAnnotation(string key, string value)
        {
            this.WriteString("Annotation key: ");
            this.WriteQuotedString(key);
            this.WriteString(" value: ");
            this.WriteQuotedString(value);
            this.WriteElementSeparator();
            this.WriteNewLine();
            this.WriteNewLine();
        }

        #endregion
    }
}
