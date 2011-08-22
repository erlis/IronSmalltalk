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
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Compiler.Interchange.ParseNodes;
using IronSmalltalk.Compiler.LexicalAnalysis;
using System.IO;
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Common;

namespace IronSmalltalk.Compiler.Interchange
{
    public class InterchangeFormatParser : ParserBase
    {
        public InterchangeFormatParser(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException();
            this.InitParser(reader);
        }

        internal Token GetNextToken()
        {
            return this.GetNextTokenxx();
        }

        protected internal virtual InterchangeVersionIdentifierNode ParseVersionId()
        {
            // PARSE: <interchangeVersionIdentifier> ::= 'Smalltalk' 'interchangeVersion:' <versionId>
            //      <versionId> ::= quotedString
            InterchangeVersionIdentifierNode result = this.CreateInterchangeVersionIdentifierNode();
            Token token = this.GetNextTokenxx();
            IdentifierToken idt = token as IdentifierToken;
            if ((idt == null) || (idt.Value != "Smalltalk"))
            {
                this.ReportParserError(result, InterchangeFormatErrors.MissingInterchangeVersionIdentifier, token);
                return result;
            }

            token = this.GetNextTokenxx();
            KeywordToken kwt = token as KeywordToken;
            if ((kwt == null) || (kwt.Value != "interchangeVersion:"))
            {
                this.ReportParserError(result, InterchangeFormatErrors.MissingInterchangeVersionIdentifier, token);
                return result;
            }

            token = this.GetNextTokenxx();
            StringToken versionId = token as StringToken;
            if (versionId == null)
            {
                this.ReportParserError(result, InterchangeFormatErrors.MissingInterchangeVersionIdentifier, token);
                return result;
            }

            return new InterchangeVersionIdentifierNode(versionId);
        }

        protected virtual InterchangeVersionIdentifierNode CreateInterchangeVersionIdentifierNode()
        {
            return new InterchangeVersionIdentifierNode();
        }

        protected internal virtual InterchangeUnitNode ParseInterchangeElement(InterchangeElementNode nodeForAnnotation)
        {
            // PARSE: <interchangeElement> ::= <classDefinition> | <classInitialization> | <globalDefinition> |
            //      <globalValueInitialization> | <poolDefinition> | <poolVariableDefinition> | <poolValueInitialization> |
            //      <methodDefinition> | <classMethodDefinition> | <programInitialization> | comment <elementSeparator>

            // PARSE: The above definitions expand to:
            // ’Class’ ’named:’ <classNameString> .... <elementSeparator>
            // <className> ’initializer’ <elementSeparator>
            // ’Global’ ’variable:’ <globalNameString> <elementSeparator>
            // ’Global’ ’constant:’ <globalNameString> <elementSeparator>
            // <globalName> ’initializer’ <elementSeparator>
            // ’Pool’ ’named:’ <poolNameString> <elementSeparator>
            // <poolName> ’variable:’ <poolVariableNameString> <elementSeparator>
            // <poolName> ’constant:’ <poolVariableNameString> <elementSeparator>
            // <poolName> ’initializerFor:’ <poolVariableNameString> <elementSeparator>
            // <className> ’method’ <elementSeparator>
            // <className> ’classMethod’ <elementSeparator>
            // ’Global’ ’initializer’ <elementSeparator>
            // comment <elementSeparator> ... NB: This will NEVER be returned by GetNextTokenxx().
            
            Token token = this.GetNextTokenxx();
            if (token is EofToken)
                return null; // Most probably just a comment ...

            // The abode syntax shows us that we are dealing with the following pattern:
            //      identifier indetifier_or_keyword undefined_token*
            // Comments are ignored and discarded by GetNextTokenxx().
            IdentifierToken id = token as IdentifierToken;
            if (id == null)
            {
                this.ReportParserError("Expected identifier.", token);
                return null;
            }

            token = this.GetNextTokenxx();
            IdentifierOrKeywordToken cmd = token as IdentifierOrKeywordToken;
            if (cmd == null)
            {
                this.ReportParserError("Expected identifier or keyword.", token);
                return null;
            }
            
            // <classDefinition> ::= ’Class’ ’named:’ <classNameString> .... <elementSeparator>
            if ((id.Value == "Class") && (cmd.Value == "named:"))
                return this.ParseClassDefinition(); // Discard <id> and <cmd> 
            // <globalDefinition> ::= ’Global’ ’variable:’ <globalNameString> <elementSeparator>
            if ((id.Value == "Global") && (cmd.Value == "variable:"))
                return this.ParseGlobalVariableDefinition(); // Discard <id> and <cmd> 
            // <globalDefinition> ::= ’Global’ ’constant:’ <globalNameString> <elementSeparator>
            if ((id.Value == "Global") && (cmd.Value == "constant:"))
                return this.ParseGlobalConstantDefinition(); // Discard <id> and <cmd> 
            // <poolDefinition> ::= ’Pool’ ’named:’ <poolNameString> <elementSeparator>
            if ((id.Value == "Pool") && (cmd.Value == "named:"))
                return this.ParsePoolDefinition(); // Discard <id> and <cmd> 
            // <programInitialization> ::= ’Global’ ’initializer’ <elementSeparator>
            if ((id.Value == "Global") && (cmd.Value == "initializer"))
                return this.ParseProgramInitialization(); // Discard <id> and <cmd>

            // <methodDefinition> ::= <className> ’method’ <elementSeparator>
            if (cmd.Value == "method")
                return this.ParseInstanceMethodDefinition(id); // Discard <cmd>
            // <classMethodDefinition> ::= <className> ’classMethod’ <elementSeparator>
            if (cmd.Value == "classMethod")
                return this.ParseClassMethodDefinition(id); // Discard <cmd>
            // <classInitialization> ::= <className> ’initializer’ <elementSeparator>
            // <globalValueInitialization> ::= <globalName> ’initializer’ <elementSeparator>
            if (cmd.Value == "initializer")
                return this.ParseGlobalInitialization(id); // Discard <cmd>
            // <poolVariableDefinition> ::= <poolName> ’variable:’ <poolVariableNameString> <elementSeparator>
            if (cmd.Value == "variable:")
                return this.ParsePoolVariableDefinition(id); // Discard <cmd>
            // <poolVariableDefinition> ::= <poolName> ’constant:’ <poolVariableNameString> <elementSeparator>
            if (cmd.Value == "constant:")
                return this.ParsePoolConstantDefinition(id); // Discard <cmd>
            // <poolValueInitialization> ::= <poolName> ’initializerFor:’ <poolVariableNameString> <elementSeparator>
            if (cmd.Value == "initializerFor:")
                return this.ParsePoolValueInitialization(id);

            // SPECIAL CASE .... Not really interchange element, but handled like one for simplicity.
            // <annotation> ::= ’Annotation’ ’key:’ quotedString ’value:’ quotedString <elementSeparator>
            if ((id.Value == "Annotation") && (cmd.Value == "key:"))
            {
                if (nodeForAnnotation == null)
                    this.ReportParserError("Annotation must follow an interchange element", cmd);
                return this.ParseAnnotation(nodeForAnnotation);
            }

            this.ReportParserError("Unrecognized interchange element.", token);
            return null;
        }

        protected virtual AnnotationNode ParseAnnotation(InterchangeElementNode nodeForAnnotation)
        {
            // PARSE: <annotation> ::= ’Annotation’ ’key:’ quotedString ’value:’ quotedString <elementSeparator>
            AnnotationNode result = this.CreateAnnotationNode(nodeForAnnotation);
            Token token = this.GetNextTokenxx();
            StringToken str = token as StringToken;
            if (str == null)
                this.ReportParserError(result, "Missing annotation key.", token);
            result.Key = str;

            token = this.GetNextTokenxx();
            KeywordToken cmd = token as KeywordToken;
            if ((cmd == null) || (cmd.Value != "value:"))
                this.ReportParserError("Missing annotation #value: keyword.", token);

            token = this.GetNextTokenxx();
            str = token as StringToken;
            if (str == null)
                this.ReportParserError(result, "Missing annotation value.", token);
            result.Value = str;

            token = this.GetNextTokenxx();
            if (!(token is EofToken))
            {
                this.ReportParserError(result, "Unexpected code found after annotation value.", token);
                result.Key = null; // This is to avoid somethind like: Annotation key: 'x' value: 'y' crash: 'yes'.
                result.Value = null; // This is to avoid somethind like: Annotation key: 'x' value: 'y' crash: 'yes'.
                return result;
            }

            return result;
        }

        protected virtual AnnotationNode CreateAnnotationNode(InterchangeElementNode nodeForAnnotation)
        {
            return new AnnotationNode(nodeForAnnotation);
        }

        protected virtual ClassDefinitionNode ParseClassDefinition()
        {
            // PARSE: <classDefinition> ::= ’Class’ ’named:’ <classNameString> 
            //      ’superclass:’ <superclassNameString>
            //      ’indexedInstanceVariables:’ <indexableInstVarType>
            //      ’instanceVariableNames:’ <instanceVariableNames>
            //      ’classVariableNames:’ <classVariableList>
            //      ’sharedPools:’ <poolList>
            //      ’classInstanceVariableNames:’<classInstVariableList>
            //      <elementSeparator>
            ClassDefinitionNode result = this.CreateClassDefinitionNode();

            Token token = this.GetNextTokenxx();
            StringToken str = token as StringToken;
            if (str == null)
                this.ReportParserError(result, "Missing class name.", token);
            str = this.VerifyIdentifierString(str, "Class name not an identifier.");
            result.ClassName = str;

            // ’superclass:’ <superclassNameString>
            token = this.GetNextTokenxx();
            KeywordToken cmd = token as KeywordToken;
            if ((cmd == null) || (cmd.Value != "superclass:"))
                this.ReportParserError("Missing class definition #superclass: keyword.", token);
            token = this.GetNextTokenxx();
            str = token as StringToken;
            if (str == null)
                this.ReportParserError(result, "Missing superclass name.", token);
            if (str.Value.Length != 0) // It's OK to have empry superclass .... Object does
                str = this.VerifyIdentifierString(str, "Superclass name not an identifier");
            result.SuperclassName = str;

            // ’indexedInstanceVariables:’ <indexableInstVarType>
            token = this.GetNextTokenxx();
            cmd = token as KeywordToken;
            if ((cmd == null) || (cmd.Value != "indexedInstanceVariables:"))
                this.ReportParserError("Missing class definition #indexedInstanceVariables: keyword.", token);
            token = this.GetNextTokenxx();
            HashedStringToken hstr = token as HashedStringToken;
            if (hstr == null)
                this.ReportParserError(result, "Missing indexed instance variables type.", token);
            result.IndexedInstanceVariables = hstr;

            // ’instanceVariableNames:’ <instanceVariableNames>
            token = this.GetNextTokenxx();
            cmd = token as KeywordToken;
            if ((cmd == null) || (cmd.Value != "instanceVariableNames:"))
                this.ReportParserError("Missing class definition #instanceVariableNames: keyword.", token);
            token = this.GetNextTokenxx();
            str = token as StringToken;
            if (str == null)
                this.ReportParserError(result, "Missing instance variable names.", token);
            str = this.VerifyIdentifierList(str, "Instance variable names list contains non-identifiers.");
            result.InstanceVariableNames = str;

            // ’classVariableNames:’ <classVariableList>
            token = this.GetNextTokenxx();
            cmd = token as KeywordToken;
            if ((cmd == null) || (cmd.Value != "classVariableNames:"))
                this.ReportParserError("Missing class definition #classVariableNames: keyword.", token);
            token = this.GetNextTokenxx();
            str = token as StringToken;
            if (str == null)
                this.ReportParserError(result, "Missing class variable names.", token);
            str = this.VerifyIdentifierList(str, "Class variable names list contains non-identifiers.");
            result.ClassVariableNames = str;

            // ’sharedPools:’ <poolList>
            token = this.GetNextTokenxx();
            cmd = token as KeywordToken;
            if ((cmd == null) || (cmd.Value != "sharedPools:"))
                this.ReportParserError("Missing class definition #sharedPools: keyword.", token);
            token = this.GetNextTokenxx();
            str = token as StringToken;
            if (str == null)
                this.ReportParserError(result, "Missing shared pool names.", token);
            str = this.VerifyIdentifierList(str, "Shared pool names list contains non-identifiers.");
            result.SharedPools = str;

            // ’classInstanceVariableNames:’<classInstVariableList>
            token = this.GetNextTokenxx();
            cmd = token as KeywordToken;
            if ((cmd == null) || (cmd.Value != "classInstanceVariableNames:"))
                this.ReportParserError("Missing class definition #classInstanceVariableNames: keyword.", token);
            token = this.GetNextTokenxx();
            str = token as StringToken;
            if (str == null)
                this.ReportParserError(result, "Missing class instance variable names.", token);
            str = this.VerifyIdentifierList(str, "Class instance variable names list contains non-identifiers.");
            result.ClassInstanceVariableNames = str;

            token = this.GetNextTokenxx();
            if (!(token is EofToken))
                this.ReportParserError(result, "Unexpected code found after class definition.", token);

            return result;
        }

        protected virtual ClassDefinitionNode CreateClassDefinitionNode()
        {
            return new ClassDefinitionNode();
        }

        protected virtual GlobalVariableDefinitionNode ParseGlobalVariableDefinition()
        {
            // PARSE: <globalDefinition> ::= ’Global’ ’variable:’ <globalNameString> <elementSeparator>
            Token token = this.GetNextTokenxx();
            StringToken name = token as StringToken;
            if (name == null)
            {
                GlobalVariableDefinitionNode result = new GlobalVariableDefinitionNode();
                this.ReportParserError(result, "Missing global variable name.", token);
                return result;
            }
            name = this.VerifyIdentifierString(name, "Global variable name not an identifier.");
            token = this.GetNextTokenxx();
            if (!(token is EofToken))
            {
                GlobalVariableDefinitionNode result = new GlobalVariableDefinitionNode();
                this.ReportParserError(result, "Unexpected code found after global variable name.", token);
                return result;
            }
            return new GlobalVariableDefinitionNode(name);
        }

        protected virtual GlobalConstantDefinitionNode ParseGlobalConstantDefinition()
        {
            // PARSE: <globalDefinition> ::= ’Global’ ’constant:’ <globalNameString> <elementSeparator>
            Token token = this.GetNextTokenxx();
            StringToken name = token as StringToken;
            if (name == null)
            {
                GlobalConstantDefinitionNode result = new GlobalConstantDefinitionNode();
                this.ReportParserError(result, "Missing global constant name.", token);
                return result;
            }
            name = this.VerifyIdentifierString(name, "Global constant name not an identifier.");
            token = this.GetNextTokenxx();
            if (!(token is EofToken))
            {
                GlobalConstantDefinitionNode result = new GlobalConstantDefinitionNode();
                this.ReportParserError(result, "Unexpected code found after global constant name.", token);
                return result;
            }
            return new GlobalConstantDefinitionNode(name);
        }

        protected virtual PoolDefinitionNode ParsePoolDefinition()
        {
            // PARSE: <poolDefinition> ::= ’Pool’ ’named:’ <poolNameString> <elementSeparator>
            Token token = this.GetNextTokenxx();
            StringToken name = token as StringToken;
            if (name == null)
            {
                PoolDefinitionNode result = new PoolDefinitionNode();
                this.ReportParserError(result, "Missing pool name.", token);
                return result;
            }
            name = this.VerifyIdentifierString(name, "Pool name not an identifier.");
            token = this.GetNextTokenxx();
            if (!(token is EofToken))
            {
                PoolDefinitionNode result = new PoolDefinitionNode();
                this.ReportParserError(result, "Unexpected code found after pool name.", token);
                return result;
            }
            return new PoolDefinitionNode(name);

        }

        protected virtual ProgramInitializationNode ParseProgramInitialization()
        {
            // PARSE: <programInitialization> ::= ’Global’ ’initializer’ <elementSeparator>
            ProgramInitializationNode result = this.CreateProgramInitializationNode();
            Token token = this.GetNextTokenxx();
            if (!(token is EofToken))
                this.ReportParserError(result, "Unexpected code found after program initializer.", token);
            return result;
        }

        protected virtual ProgramInitializationNode CreateProgramInitializationNode()
        {
            return new ProgramInitializationNode();
        }

        protected virtual InstanceMethodDefinitionNode ParseInstanceMethodDefinition(IdentifierToken className)
        {
            // PARSE: <methodDefinition> ::= <className> ’method’ <elementSeparator>
            InstanceMethodDefinitionNode result = this.CreateInstanceMethodDefinitionNode(className);
            Token token = this.GetNextTokenxx();
            if (!(token is EofToken))
                this.ReportParserError(result, "Unexpected code found after method definition.", token);
            return result;
        }

        protected virtual InstanceMethodDefinitionNode CreateInstanceMethodDefinitionNode(IdentifierToken className)
        {
            return new InstanceMethodDefinitionNode(className);
        }

        protected virtual ClassMethodDefinitionNode ParseClassMethodDefinition(IdentifierToken className)
        {
            // PARSE: <classMethodDefinition> ::= <className> ’classMethod’ <elementSeparator>
            ClassMethodDefinitionNode result = this.CreateClassMethodDefinitionNode(className);
            Token token = this.GetNextTokenxx();
            if (!(token is EofToken))
                this.ReportParserError(result, "Unexpected code found after class method definition.", token);
            return result;
        }

        protected virtual ClassMethodDefinitionNode CreateClassMethodDefinitionNode(IdentifierToken className)
        {
            return new ClassMethodDefinitionNode(className);
        }

        protected virtual GlobalInitializationNode ParseGlobalInitialization(IdentifierToken globalName)
        {
            // PARSE: <classInitialization> ::= <className> ’initializer’ <elementSeparator>
            //      <globalValueInitialization> ::= <globalName> ’initializer’ <elementSeparator>
            // NB: Classes are threated as globals (classes are globals).
            GlobalInitializationNode result = this.CreateGlobalInitializationNode(globalName);
            Token token = this.GetNextTokenxx();
            if (!(token is EofToken))
                this.ReportParserError(result, "Unexpected code found after global or class initializer.", token);
            return result;
        }

        protected virtual GlobalInitializationNode CreateGlobalInitializationNode(IdentifierToken globalName)
        {
            return new GlobalInitializationNode(globalName);
        }

        protected virtual PoolVariableDefinitionNode ParsePoolVariableDefinition(IdentifierToken poolName)
        {
            // PARSE: <poolVariableDefinition> ::= <poolName> ’variable:’ <poolVariableNameString> <elementSeparator>
            PoolVariableDefinitionNode result = this.CreatePoolVariableDefinitionNode(poolName);
            Token token = this.GetNextTokenxx();
            StringToken name = token as StringToken;
            if (name == null)
            {
                this.ReportParserError(result, "Missing pool variable name.", token);
                return result;
            }
            name = this.VerifyIdentifierString(name, "Pool variable name not an identifier.");
            result.PoolVariableName = name;
            token = this.GetNextTokenxx();
            if (!(token is EofToken))
                this.ReportParserError(result, "Unexpected code found after pool variable name.", token);
            return result;
        }

        protected virtual PoolVariableDefinitionNode CreatePoolVariableDefinitionNode(IdentifierToken poolName)
        {
            return new PoolVariableDefinitionNode(poolName);
        }

        protected virtual PoolConstantDefinitionNode ParsePoolConstantDefinition(IdentifierToken poolName)
        {
            // PARSE: <poolVariableDefinition> ::= <poolName> ’constant:’ <poolVariableNameString> <elementSeparator>
            PoolConstantDefinitionNode result = this.CreatePoolConstantDefinitionNode(poolName);
            Token token = this.GetNextTokenxx();
            StringToken name = token as StringToken;
            if (name == null)
            {
                this.ReportParserError(result, "Missing pool constant name.", token);
                return result;
            }
            name = this.VerifyIdentifierString(name, "Pool constant name not an identifier.");
            result.PoolVariableName = name;
            token = this.GetNextTokenxx();
            if (!(token is EofToken))
                this.ReportParserError(result, "Unexpected code found after pool constant name.", token);
            return result;
        }

        protected virtual PoolConstantDefinitionNode CreatePoolConstantDefinitionNode(IdentifierToken poolName)
        {
            return new PoolConstantDefinitionNode(poolName);
        }

        protected virtual PoolValueInitializationNode ParsePoolValueInitialization(IdentifierToken poolName)
        {
            // PARSE: <poolValueInitialization> ::= <poolName> ’initializerFor:’ <poolVariableNameString> <elementSeparator>
            PoolValueInitializationNode result = this.CreatePoolValueInitializationNode(poolName);
            Token token = this.GetNextTokenxx();
            StringToken name = token as StringToken;
            if (name == null)
            {
                this.ReportParserError(result, "Missing pool initializer variable name.", token);
                return result;
            }
            name = this.VerifyIdentifierString(name, "Pool variable name not an identifier.");
            result.PoolVariableName = name;
            token = this.GetNextTokenxx();
            if (!(token is EofToken))
                this.ReportParserError(result, "Unexpected code found after pool initializer variable name.", token);
            return result;
        }

        protected virtual PoolValueInitializationNode CreatePoolValueInitializationNode(IdentifierToken poolName)
        {
            return new PoolValueInitializationNode(poolName);
        }

        protected virtual Token GetNextTokenxx()
        {
            return this.GetNextTokenxx(LexicalAnalysis.Preference.Default);
        }

        protected virtual StringToken VerifyIdentifierString(StringToken token, string errorMessage)
        {
            // stringDelimiter identifier stringDelimiter
            // identifier ::= letter (letter | digit)*
            if (token == null)
                return null;

            string str = token.Value;
            if (str.Length == 0)
            {
                this.ReportParserError(errorMessage, token);
                return null; // Empty
            }
            ScanResult scanResult = new ScanResult();
            scanResult.SetResult(str[0]);
            if (!scanResult.IsLetter())
            {
                this.ReportParserError(errorMessage, token);
                return null; // First char non-letter
            }
            foreach (char ch in str)
            {
                scanResult.SetResult(ch);
                if (!(scanResult.IsLetter() || scanResult.IsDigit()))
                {
                    this.ReportParserError(errorMessage, token);
                    return null; // Non-letter or non-digit char
                }
                
            }

            return token; // OK
        }

        protected virtual StringToken VerifyIdentifierList(StringToken token, string errorMessage)
        {
            // stringDelimiter identifier* stringDelimiter
            if (token == null)
                return null;
            if (token.Value.Length == 0)
                return token;

            bool inComment = false;
            bool identifierStart = true;
            ScanResult scanResult = new ScanResult();
            foreach (char ch in token.Value)
            {
                scanResult.SetResult(ch);
                if (scanResult.IsCommentDelimiter())
                {
                    inComment = !inComment;
                    identifierStart = true;
                }
                else
                {
                    if (!inComment)
                    {
                        if (scanResult.IsWhitespace())
                        {
                            identifierStart = true;
                        }
                        else
                        {
                            if (identifierStart)
                            {
                                if (!scanResult.IsLetter())
                                {
                                    this.ReportParserError(errorMessage, token);
                                    return null;
                                }
                                identifierStart = false;
                            }
                            else
                            {
                                if (!(scanResult.IsLetter() || scanResult.IsDigit()))
                                {
                                    this.ReportParserError(errorMessage, token);
                                    return null;
                                }
                            }
                        }
                    }
                }
            }
 
            return token;
        }


        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        protected override void ReportParserError(SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IToken offendingToken)
        {
            if (offendingToken == null)
                return; // Error must be encountered while scanning and reported by the Scanner.
            base.ReportParserError(startPosition, stopPosition, parseErrorMessage, offendingToken);
        }

        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="node">Parse node responsible / signaling the error.</param>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        protected override void ReportParserError(IParseNode node, SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IToken offendingToken)
        {
            if (offendingToken == null)
                return; // Error must be encountered while scanning and reported by the Scanner.
            base.ReportParserError(node, startPosition, stopPosition, parseErrorMessage, offendingToken);
        }

        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="node">Parse node responsible / signaling the error.</param>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        protected override void ReportParserError(IParseNode node, string parseErrorMessage, IToken offendingToken)
        {
            if (offendingToken == null)
                return; // Error must be encountered while scanning and reported by the Scanner.
            base.ReportParserError(node, parseErrorMessage, offendingToken);
        }

        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        protected override void ReportParserError(string parseErrorMessage, IToken offendingToken)
        {
            if (offendingToken == null)
                return; // Error must be encountered while scanning and reported by the Scanner.
            base.ReportParserError(parseErrorMessage, offendingToken);
        }
    }
}
