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
using System.IO;
using IronSmalltalk.Compiler.Interchange.ParseNodes;
using System.Text;
using IronSmalltalk.Compiler.SemanticAnalysis;
using System.Collections.Generic;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Runtime.Installer;
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Runtime;
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Compiler.Interchange;
using IronSmalltalk.Common;

namespace IronSmalltalk.Interchange
{
    /// <summary>
    /// The InterchangeFormatProcessor is the entry point to the interchange installer.
    /// It is responsible for precessing a single interchange file.
    /// </summary>
    /// <remarks>
    /// The InterchangeFormatProcessor works on a TextReader that contains the source code
    /// to be installed. It reads the source code, processes elements in it and notifies
    /// the given IInterchangeFileInProcessor for each element that is encountered in the
    /// code. It is up to the IInterchangeFileInProcessor to do something usefull with 
    /// the given meta objects describing each filed-in element.
    /// 
    /// The InterchangeFormatProcessor is given a map of version identifiers and
    /// InterchangeVersionService objects. The InterchangeVersionService objects is
    /// responsible to handle differences between interchange versions. The X3J20
    /// defines only a single version, but we may extend this with IronSmalltalk
    /// sepecific information. The InterchangeVersionService is responsible for
    /// returning a parser compatible with the given version and handle other 
    /// version differences.
    /// 
    /// The ErrorSink is an optional IInterchangeErrorSink where parse and other 
    /// error are reported to.
    /// </remarks>
    public class InterchangeFormatProcessor
    {
        /// <summary>
        /// Once the header of the interchange file has been processed, 
        /// this property contains the interchange version identifier.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// The ErrorSink is an optional IInterchangeErrorSink where parse and other error are reported to.
        /// </summary>
        public IInterchangeErrorSink ErrorSink { get; set; }

        /// <summary>
        /// The IInterchangeFileInProcessor that will be notified for each element found in the source code.
        /// It is up to the IInterchangeFileInProcessor to do something usefull with the given meta objects 
        /// describing each filed-in element.
        /// </summary>
        public IInterchangeFileInProcessor FileInProcessor { get; private set; }

        /// <summary>
        /// The text reader containing the source code.
        /// </summary>
        private readonly TextReader Reader;

        /// <summary>
        /// Map between interchange versions and InterchangeVersionService objects.
        /// Once we've read the file header, we use this map to determine which service to use.
        /// </summary>
        private readonly IDictionary<string, InterchangeVersionService> VersionServicesMap;

        /// <summary>
        /// Once the header of the interchange file has been processed, this property contains the 
        /// InterchangeVersionService to be used for the rest of the processing. 
        /// It is based on the interchange version identifier found in the interchange file 
        /// and matched to a service in the VersionServicesMap.
        /// </summary>
        private InterchangeVersionService VersionService;

        /// <summary>
        /// This contains the current position. It is used to map the position that are relative 
        /// within each interchange chunk to absolute position within the interchange file.
        /// </summary>
        /// <remarks>
        /// This will contain the last source position of the previous token or 
        /// for newly created processor, this is the position before the first character.
        /// </remarks>
        internal SourceLocation SourcePosition { get; private set; }

        /// <summary>
        /// Create a new interchange format processor.
        /// </summary>
        /// <param name="sourceCodeReader">Text reader containing the source code.</param>
        /// <param name="fileInProcessor">File-in processor that will be notified about each filed-in element.</param>
        /// <param name="versionServicesMap">Map between interchange versions and interchange-version-services.</param>
        public InterchangeFormatProcessor(TextReader sourceCodeReader, IInterchangeFileInProcessor fileInProcessor, 
            IDictionary<string, InterchangeVersionService> versionServicesMap)
        {
            if (sourceCodeReader == null)
                throw new ArgumentNullException("sourceCodeReader");
            if (fileInProcessor == null)
                throw new ArgumentNullException("fileInProcessor");
            if (versionServicesMap == null)
                throw new ArgumentNullException("versionServicesMap");
            this.Reader = sourceCodeReader;
            this.FileInProcessor = fileInProcessor;
            this.VersionServicesMap = versionServicesMap;
            this.SourcePosition = SourceLocation.Invalid;
        }

        /// <summary>
        /// Process the interchange file. The FileInProcessor will be notified for each read element.
        /// </summary>
        public void ProcessInterchangeFile()
        {
            InterchangeChunk chunk = this.GetNextChunk();
            if (chunk == null)
                return; // Empty file

            // All interchange files start with a version identifier.
            if (!this.ProcessVersionSpecification(chunk))
                return; // Cannot process this version

            // ProcessVersionSpecification() should have ensured this is set correctly or aborted, but we check anyway.
            if (this.VersionService != null)
                this.VersionService.ProcessInterchangeElements(this);
        }

        /// <summary>
        /// Process the interchange version identifier of the interchange file.
        /// Set internal state based on the version identifier.
        /// </summary>
        /// <param name="chunk">Chunk of source code that is supposed to contain the version identifier.</param>
        /// <returns></returns>
        private bool ProcessVersionSpecification(InterchangeChunk chunk)
        {
            InterchangeFormatParser parser = parser = new InterchangeFormatParser(new StringReader(chunk.SourceChunk));
            parser.ErrorSink = chunk;
            InterchangeVersionIdentifierNode vidNode = parser.ParseVersionId(); // Parser always returns a node.
            if (vidNode.VersionId != null)
                this.Version = vidNode.VersionId.Value;

            if ((vidNode.VersionId != null) && !this.SetVersionService())
            {
                if (this.ErrorSink != null)
                    this.ErrorSink.AddInterchangeError(
                        chunk.TranslateSourcePosition(vidNode.VersionId.StartPosition),
                        chunk.TranslateSourcePosition(vidNode.VersionId.StopPosition),
                        InterchangeFormatErrors.InvalidVersionId);
                return false; // Cannot process this version
            }
            return true;
        }

        /// <summary>
        /// Look-up the version service for the encountered interchange version.
        /// </summary>
        /// <returns>Return true if version service was found, otherwise false.</returns>
        private bool SetVersionService()
        {
            if (this.Version == null)
                return false;
            if (!this.VersionServicesMap.TryGetValue(this.Version, out this.VersionService))
                return false;
            if (this.VersionService == null)
                return false;
            return true;
        }

        /// <summary>
        /// Parse identifier list. Identifier list have the syntax: identifierList ::= stringDelimiter identifier* stringDelimiter
        /// </summary>
        /// <param name="token">Token containing the string with the identifier list.</param>
        /// <param name="parseErrorSink">Error sink for reporting parse errors.</param>
        /// <param name="sourceCodeService">Source code service that can convert tokens to source code span and reports issues.</param>
        /// <returns>A collection of sub-tokens if successful or null if token string is illegal.</returns>
        public IEnumerable<SourceReference<string>> ParseIdentifierList(StringToken token, IParseErrorSink parseErrorSink, ISourceCodeReferenceService sourceCodeService)
        {
            if (this.VersionService == null)
                throw new InvalidOperationException("Processor cannor be used if version service is not set.");
            return this.VersionService.ParseIdentifierList(token, parseErrorSink, sourceCodeService);
        }

        /// <summary>
        /// It is expect the next chunk to contain a method. This method parses the next chunk 
        /// as a method and returns the method parse node representing the method source. 
        /// </summary>
        /// <returns>MethodNode representing the parsed method.</returns>
        public MethodNode ParseMethod(out ISourceCodeReferenceService sourceCodeService)
        {
            if (this.VersionService == null)
                throw new InvalidOperationException("Processor cannor be used if version service is not set.");
            return this.VersionService.ParseMethod(this, out sourceCodeService);
        }

        /// <summary>
        /// It is expect the next chunk to contain an initializer. This method parses the next chunk 
        /// as an initializer and returns the initializer parse node representing the initializer source. 
        /// </summary>
        /// <returns>InitializerNode representing the parsed initializer.</returns>
        public InitializerNode ParseInitializer(out ISourceCodeReferenceService sourceCodeService)
        {
            if (this.VersionService == null)
                throw new InvalidOperationException("Processor cannor be used if version service is not set.");
            return this.VersionService.ParseInitializer(this, out sourceCodeService);
        }

        /// <summary>
        /// Creates an empty reference to a source code for the given value.
        /// This empty source-reference is used when we cannot exactly determine 
        /// which source code created the value.
        /// </summary>
        /// <remarks>
        /// Usage if this method is discouraged, because it doesn't give the user 
        /// the necessary information about the location of an error in the source code.
        /// </remarks>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="value">Value that was created based on the source code.</param>
        /// <returns>Source code reference for the given value.</returns>
        public SourceReference<TValue> CreateSourceReference<TValue>(TValue value)
        {
            return new SourceReference<TValue>(value);
        }

        /// <summary>
        /// Creates a reference to a source code for the given value.
        /// The source-reference represents the source that directly or indirectly created the given value.
        /// </summary>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="value">Value that was created based on the source code.</param>
        /// <param name="token">Parse token that created the value.</param>
        /// <param name="sourceCodeService">Source code service that translated between relative and absolute positions.</param>
        /// <returns>Source code reference for the given value.</returns>
        public SourceReference<TValue> CreateSourceReference<TValue>(TValue value, IToken token, ISourceCodeReferenceService sourceCodeService)
        {
            return new SourceReference<TValue>(value, token.StartPosition, token.StopPosition, sourceCodeService);
        }

        /// <summary>
        /// Creates a reference to a source code for the given value.
        /// The source-reference represents the source that directly or indirectly created the given value.
        /// </summary>
        /// <typeparam name="TValue">Type of the value.</typeparam>
        /// <param name="value">Value that was created based on the source code.</param>
        /// <param name="startPosition">Relative start position in the source for the given value.</param>
        /// <param name="stopPosition">Relative end position in the source for the given value.</param>
        /// <param name="sourceCodeService">Source code service that translated between relative and absolute positions.</param>
        /// <returns>Source code reference for the given value.</returns>
        public SourceReference<TValue> CreateSourceReference<TValue>(TValue value, SourceLocation startPosition, SourceLocation stopPosition, ISourceCodeReferenceService sourceCodeService)
        {
            return new SourceReference<TValue>(value, startPosition, stopPosition, sourceCodeService);
        }

        /// <summary>
        /// Read and return the next interchange chunk.
        /// </summary>
        /// <remarks>
        /// This method is responsible for reading the interchange chunks 
        /// that are delimited by the ! delimitor and escape double !! characters.
        /// </remarks>
        /// <returns>Returns an InterchangeChunk representing the source code for the chunk.</returns>
        internal InterchangeChunk GetNextChunk()
        {
            if (this.Reader.Peek() == -1)
                return null;
            StringBuilder builder = new StringBuilder(1000);
            int separator = InterchangeFormatConstants.ElementSeparator;
            SourceLocation startPosition = SourceLocation.Invalid;

            int pos = this.SourcePosition.Position;
            int line = this.SourcePosition.Line;
            int col = this.SourcePosition.Column;

            while (true)
            {
                int ch = this.Reader.Read();

                // Line and column counting
                pos++;
                if (ch == 13)
                {
                    if (this.Reader.Peek() == 10)
                    {
                        builder.Append((char)ch);
                        ch = this.Reader.Read();
                        pos++;
                    }
                    line++;
                    col = 0;
                }
                else
                {
                    col++;
                }

                if (!startPosition.IsValid)
                    startPosition = new SourceLocation(pos, Math.Max(line, 1), Math.Max(col, 1));

                if (ch == -1)
                {
                    this.SourcePosition = new SourceLocation(pos, Math.Max(line, 1), Math.Max(col, 1));
                    if (String.IsNullOrWhiteSpace(builder.ToString()))
                        return null;
                    if (this.ErrorSink != null)
                        this.ErrorSink.AddInterchangeError(startPosition, this.SourcePosition, InterchangeFormatErrors.MissingTerminalSeparator);
                    return new InterchangeChunk(this, builder.ToString(), startPosition);
                }
                if (ch == separator)
                {
                    this.SourcePosition = new SourceLocation(pos, Math.Max(line, 1), Math.Max(col, 1));
                    if (this.Reader.Peek() != separator)
                        return new InterchangeChunk(this, builder.ToString(), startPosition);

                    pos++; col++;
                    this.Reader.Read();
                }
                builder.Append((char) ch);
            }
        }
    }
}
