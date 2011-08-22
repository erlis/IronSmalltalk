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
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Runtime.Installer;
using System.Linq;
using IronSmalltalk.Interchange;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Common;

namespace IronSmalltalk.Compiler.Interchange
{
    /// <summary>
    /// This class represents a piece of source code in the interchange file.
    /// The code represents exctly one chunk of source, and is delimited by a "elementSeparator",
    /// as defined in X3J20 "4.1 Interchange Format BNF Syntax".
    /// </summary>
    /// <remarks>
    /// Note that some interchangeElements defined in the X3J20 "4.1 Interchange Format BNF Syntax"
    /// may span over several chunks.
    /// 
    /// This class implements the IParseErrorSink and ISourceCodeReferenceService interfaces.
    /// The reason is that instances if this class contain context information of where in the
    /// interchange source code file the chunk resides, and also how many ! characters were escaped.
    /// Therefore, instances of this class are in the unique position to convert source code positions
    /// that are relative to the source chunk to absolute positions in the interchange file.
    /// This class implements the IParseErrorSink, patches source code positions and passes the error
    /// to the outer sink hold by the owning interchange format processor. The ISourceCodeReferenceService 
    /// is used in similar way to convert between relative and absolute source code positions.
    /// </remarks>
    public class InterchangeChunk : IParseErrorSink, ISourceCodeReferenceService
    {
        /// <summary>
        /// Interchange processor that created and owns this chunk.
        /// </summary>
        public InterchangeFormatProcessor Processor { get; private set; }

        /// <summary>
        /// The source code string that is contained in this chunk.
        /// </summary>
        /// <remarks>
        /// Double exclamation points (double !!) that may occurer have be escaped and converted to a single one. 
        /// </remarks>
        public string SourceChunk { get; private set; }

        /// <summary>
        /// Start position of this chunk in the interchange file.
        /// </summary>
        public SourceLocation StartPosition { get; private set; }

        /// <summary>
        /// Create a new interchange chunk.
        /// </summary>
        /// <param name="processor">Interchange processor that created and owns this chunk.</param>
        /// <param name="sourceChunk">The source code string that is contained in this chunk.</param>
        /// <param name="startPosition">Start position of this chunk in the interchange file.</param>
        public InterchangeChunk(InterchangeFormatProcessor processor, string sourceChunk, SourceLocation startPosition)
        {
            if (processor == null)
                throw new ArgumentNullException("processor");
            if (sourceChunk == null)
                throw new ArgumentNullException("sourceChunk");

            this.Processor = processor;
            this.SourceChunk = sourceChunk;
            this.StartPosition = startPosition;
        }

        #region interface IParseErrorSink

        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        void IParseErrorSink.AddParserError(SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, LexicalTokens.IToken offendingToken)
        {
            startPosition = this.TranslateSourcePosition(startPosition);
            stopPosition = this.TranslateSourcePosition(stopPosition);
            if (this.Processor.ErrorSink != null)
                this.Processor.ErrorSink.AddParserError(startPosition, stopPosition, parseErrorMessage, offendingToken);
        }

        /// <summary>
        /// Report a semantical error encountered during parsing of the source code.
        /// </summary>
        /// <param name="node">Parse node responsible / signaling the error.</param>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="parseErrorMessage">Parse error message because of source code semantical error.</param>
        /// <param name="offendingToken">Token responsible for the problem.</param>
        void IParseErrorSink.AddParserError(SemanticNodes.IParseNode node, SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, LexicalTokens.IToken offendingToken)
        {
            startPosition = this.TranslateSourcePosition(startPosition);
            stopPosition = this.TranslateSourcePosition(stopPosition);
            if (this.Processor.ErrorSink != null)
                this.Processor.ErrorSink.AddParserError(node, startPosition, stopPosition, parseErrorMessage, offendingToken);
        }

        /// <summary>
        /// Report a syntax error encountered during scan of the source code.
        /// </summary>
        /// <param name="token">Token responsible for the problem.</param>
        /// <param name="startPosition">Source code start position.</param>
        /// <param name="stopPosition">source code end position.</param>
        /// <param name="scanErrorMessage">Scan error message because of source code syntax error.</param>   
        void LexicalAnalysis.IScanErrorSink.AddScanError(LexicalTokens.IToken token, SourceLocation startPosition, SourceLocation stopPosition, string scanErrorMessage)
        {
            startPosition = this.TranslateSourcePosition(startPosition);
            stopPosition = this.TranslateSourcePosition(stopPosition);
            if (this.Processor.ErrorSink != null)
                this.Processor.ErrorSink.AddScanError(token, startPosition, stopPosition, scanErrorMessage);
        }

        #endregion

        /// <summary>
        /// Convert a relative source chunk string position to absolute intechange file position.
        /// </summary>
        /// <param name="position">Position relative to the source chunk string.</param>
        /// <returns>Absolute position in the interchange format source code file where this chunk originated from.</returns>
        public int TranslateSourcePosition(int position)
        {
            if (position < 0)
                return position;
            // patch source positions due to '!' char escaping.
            int escapedChars = this.SourceChunk.Take(position-1).Count(ch => ch == InterchangeFormatConstants.ElementSeparator);
            return this.StartPosition.Position + position + escapedChars;
        }

        /// <summary>
        /// Convert a relative source chunk string position to absolute intechange file position.
        /// </summary>
        /// <param name="position">Position relative to the source chunk string.</param>
        /// <returns>Absolute position in the interchange format source code file where this chunk originated from.</returns>
        public SourceLocation TranslateSourcePosition(SourceLocation position)
        {
            if (position.Position < 0)
                return position;
            // patch source positions due to '!' char escaping.
            int escapedChars = this.SourceChunk.Take(position.Position).Count(ch => ch == InterchangeFormatConstants.ElementSeparator);
            int abspos = this.StartPosition.Position + position.Position + escapedChars;
            // patch column positions due to '!' char escaping.
            int start = Math.Max(position.Position - position.Column + 1, 0);
            string line = this.SourceChunk.Substring(start, Math.Min(this.SourceChunk.Length - start, position.Column));
            escapedChars = line.Count(ch => ch == InterchangeFormatConstants.ElementSeparator);
            return new SourceLocation(abspos, this.StartPosition.Line + position.Line - 1, 
                ((position.Line == 1) ? this.StartPosition.Column - 1 : 0) + position.Column + escapedChars);
        }
    }
}
