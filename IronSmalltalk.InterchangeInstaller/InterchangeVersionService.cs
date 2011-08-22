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
using System.IO;
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Runtime.Installer;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Compiler.Interchange.ParseNodes;
using IronSmalltalk.Compiler.Interchange;
using IronSmalltalk.Common;

namespace IronSmalltalk.Interchange
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This class will not keep state!
    /// </remarks>
    public abstract class InterchangeVersionService
    {
        /// <summary>
        /// Process the interchange elements (chunks) provided by the given interchange format processor.
        /// </summary>
        /// <param name="processor">Interchange format processor that can provide interchange chunks for processing.</param>
        protected internal virtual void ProcessInterchangeElements(InterchangeFormatProcessor processor)
        {
            InterchangeChunk chunk = this.GetNextChunk(processor);
            if (chunk == null)
            {
                // Must have at least ONE interchange unit.
                this.ReportError(processor, InterchangeFormatErrors.ExpectedInterchangeUnit);
                return;
            }
            InterchangeUnitNode node = null;
            while (chunk != null)
            {
                node = this.ProcessInterchangeElement(processor, node, chunk);
                chunk = this.GetNextChunk(processor);
            }
        }

        /// <summary>
        /// Process a given interchange element (chunk).
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="node">Last node that was parsed. This is used for annotations.</param>
        /// <param name="chunk">Interchange chunk to be processed.</param>
        /// <returns>Returns the new interchange parse node (or null in case of error).</returns>
        protected virtual InterchangeUnitNode ProcessInterchangeElement(InterchangeFormatProcessor processor, InterchangeUnitNode node, InterchangeChunk chunk)
        {
            InterchangeFormatParser parser = this.GetInterchangeParser(chunk);
            InterchangeElementNode nodeForAnnotation = (node == null) ? null : node.GetAnnotatedNode();
            node = parser.ParseInterchangeElement(nodeForAnnotation);
            if (node == null)
                return null;
            return node.FileIn(processor, chunk, chunk);
        }

        /// <summary>
        /// Create and return a parser capable of parsing interchange nodes / interchange chunks.
        /// </summary>
        /// <remarks>
        /// The parser returned here is capable of parsing interchange chunks that follow the 
        /// interchange version this version service is supporting.
        /// </remarks>
        /// <param name="sourceChunk">Interchange chunk to be parsed.</param>
        /// <returns>Interchange format parser.</returns>
        protected virtual InterchangeFormatParser GetInterchangeParser(InterchangeChunk sourceChunk)
        {
            if (sourceChunk == null)
                throw new ArgumentNullException();
            InterchangeFormatParser parser = new InterchangeFormatParser(new StringReader(sourceChunk.SourceChunk));
            parser.ErrorSink = sourceChunk;
            return parser;
        }

        /// <summary>
        /// Retereive the next interchange chunk from the processor and parse it as a method.
        /// </summary>
        /// <param name="processor">Interchange format processor that can provide the interchange chunk for parsing.</param>
        /// <remarks>
        /// The parser used here is capable of parsing interchange chunks containg methods that follow the 
        /// interchange version standard that this version service is supporting.
        /// </remarks>
        /// <returns>MethodNode representing the parsed method source.</returns>
        protected internal virtual MethodNode ParseMethod(InterchangeFormatProcessor processor, out ISourceCodeReferenceService sourceCodeService)
        {
            InterchangeChunk chunk = this.GetNextChunk(processor);
            sourceCodeService = chunk;
            if (chunk == null)
            {
                // Must have an interchange element that contains <initializer definition>.
                this.ReportError(processor, "Expected method definition.");
                return null;
            }
            Parser parser = this.GetFunctionParser();
            parser.ErrorSink = chunk;
            return parser.ParseMethod(new StringReader(chunk.SourceChunk));
        }

        /// <summary>
        /// Retereive the next interchange chunk from the processor and parse it as an initializer.
        /// </summary>
        /// <param name="processor">Interchange format processor that can provide the interchange chunk for parsing.</param>
        /// <remarks>
        /// The parser used here is capable of parsing interchange chunks containg initializers that follow the 
        /// interchange version standard that this version service is supporting.
        /// </remarks>
        /// <returns>InitializerNode representing the parsed initializer source.</returns>
        protected internal virtual InitializerNode ParseInitializer(InterchangeFormatProcessor processor, out ISourceCodeReferenceService sourceCodeService)
        {
            InterchangeChunk chunk = this.GetNextChunk(processor);
            sourceCodeService = chunk;
            if (chunk == null)
            {
                // Must have an interchange element that contains <initializer definition>.
                this.ReportError(processor, "Expected initializer definition.");
                return null;
            }
            Parser parser = this.GetFunctionParser();
            parser.ErrorSink = chunk;
            return parser.ParseInitializer(new StringReader(chunk.SourceChunk));
        }

        /// <summary>
        /// Create and return a parser capable of parsing methods and initializers.
        /// </summary>
        /// <remarks>
        /// The parser returned here is capable of parsing interchange chunks containg methods and initializers 
        /// that follow the interchange version standard that this version service is supporting.
        /// </remarks>
        /// <returns>Parser for parsing method and initilizers.</returns>
        protected virtual Parser GetFunctionParser()
        {
            Parser parser = new Parser();
            return parser;
        }

        /// <summary>
        /// Parse identifier list. Identifier list have the syntax: identifierList ::= stringDelimiter identifier* stringDelimiter
        /// </summary>
        /// <param name="token">Token containing the string with the identifier list.</param>
        /// <param name="parseErrorSink">Error sink for reporting parse errors.</param>
        /// <param name="sourceCodeService">Source code service that can convert tokens to source code span and reports issues.</param>
        /// <returns>A collection of sub-tokens if successful or null if token string is illegal.</returns>
        protected internal virtual IEnumerable<SourceReference<string>> ParseIdentifierList(StringToken token, IParseErrorSink parseErrorSink, ISourceCodeReferenceService sourceCodeService)
        {
            if (token == null)
                throw new ArgumentNullException("token");
            if (parseErrorSink == null)
                throw new ArgumentNullException("parseErrorSink");
            if (sourceCodeService == null)
                throw new ArgumentNullException("sourceCodeService");

            List<SourceReference<string>> result = new List<SourceReference<string>>();
            if (token.Value.Length == 0)
                return result;

            Scanner scanner = new Scanner(new StringReader(token.Value));
            Token t = scanner.GetToken();
            while (!(t is EofToken))
            {
                // Skip whitespaces (incl. comments).
                if (!(t is WhitespaceToken))
                {
                    // Offset the token to the position within the original token. +1 is due to the opening string ' quote.
                    t.SetTokenValues(
                        new SourceLocation(t.StartPosition.Position + token.StartPosition.Position + 1,
                            t.StartPosition.Line + token.StartPosition.Line,
                            t.StartPosition.Column + token.StartPosition.Column + 1),
                        new SourceLocation(t.StopPosition.Position + token.StartPosition.Position + 1,
                            t.StopPosition.Line + token.StartPosition.Line,
                            t.StopPosition.Column + token.StartPosition.Column + 1),
                        t.ScanError);

                    if (t is IdentifierToken)
                    {
                        // identifier  OK
                        result.Add(new SourceReference<string>(((IdentifierToken)t).Value, t.StartPosition, t.StopPosition, sourceCodeService));
                    }
                    else
                    {
                        // Non-identifier, report error
                        parseErrorSink.AddParserError(t.StartPosition, t.StopPosition, "Expected identifier", t);
                        return null; // Error condition
                    }
                }
                t = scanner.GetToken();
            }
            return result;
        }

        /// <summary>
        /// Report that an error has occured during parsing of the source code.
        /// </summary>
        /// <param name="processor">Interchange format processor whos error-sink is used to report the error.</param>
        /// <param name="parseErrorMessage">Error message.</param>
        protected void ReportError(InterchangeFormatProcessor processor, string parseErrorMessage)
        {
            if (processor == null)
                throw new ArgumentNullException();
            if (processor.ErrorSink != null)
                processor.ErrorSink.AddInterchangeError(processor.SourcePosition, processor.SourcePosition, parseErrorMessage);
        }

        /// <summary>
        /// Get the next interchage chunk to be processor.
        /// </summary>
        /// <param name="processor">Interchange format processor that contains the interchange sources.</param>
        /// <returns></returns>
        protected InterchangeChunk GetNextChunk(InterchangeFormatProcessor processor)
        {
            if (processor == null)
                throw new ArgumentNullException();
            return processor.GetNextChunk();
        }
    }
}
