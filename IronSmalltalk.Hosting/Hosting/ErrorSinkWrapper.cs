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
using IronSmalltalk.Interchange;
//using Microsoft.Scripting;
using IronSmalltalk.Compiler.SemanticNodes;
using IronSmalltalk.Compiler.LexicalTokens;
using IronSmalltalk.Common;


namespace IronSmalltalk.Runtime.Hosting
{
    /// <summary>
    /// This class implements IInterchangeErrorSink (incl. IParseErrorSink and IScanErrorSink).
    /// It is use bu the Installer and Parser services to report error.
    /// 
    /// It forwards the error to the external ErrorSink provided by the DLR.
    /// </summary>
    public class ErrorSinkWrapper : IInterchangeErrorSink
    {
        public const int ScanErrorCode = 100;
        public const int ParseErrorCode = 200;
        public const int InstallErrorCode = 300;

        private readonly Microsoft.Scripting.ErrorSink ErrorSink;
        private readonly Microsoft.Scripting.SourceUnit SourceUnit;

        public ErrorSinkWrapper(Microsoft.Scripting.SourceUnit sourceUnit, Microsoft.Scripting.ErrorSink errorSink)
        {
            if (sourceUnit == null)
                throw new ArgumentNullException();
            if (errorSink == null)
                throw new ArgumentNullException();
            this.SourceUnit = sourceUnit;
            this.ErrorSink = errorSink;
        }

        public void AddInterchangeError(SourceLocation startPosition, SourceLocation stopPosition, string errorMessage)
        {
            this.ErrorSink.Add(this.SourceUnit, 
                errorMessage, 
                this.GetSpan(startPosition, stopPosition),
                ErrorSinkWrapper.InstallErrorCode, 
                Microsoft.Scripting.Severity.Error);
        }

        public void AddParserError(SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IToken offendingToken)
        {
            this.ErrorSink.Add(this.SourceUnit,
                parseErrorMessage,
                this.GetSpan(startPosition, stopPosition),
                ErrorSinkWrapper.ParseErrorCode,
                Microsoft.Scripting.Severity.Error);
        }

        public void AddParserError(IParseNode node, SourceLocation startPosition, SourceLocation stopPosition, string parseErrorMessage, IToken offendingToken)
        {
            this.ErrorSink.Add(this.SourceUnit,
                parseErrorMessage,
                this.GetSpan(startPosition, stopPosition),
                ErrorSinkWrapper.ParseErrorCode,
                Microsoft.Scripting.Severity.Error);
        }

        public void AddScanError(Compiler.LexicalTokens.IToken token, SourceLocation startPosition, SourceLocation stopPosition, string scanErrorMessage)
        {
            this.ErrorSink.Add(this.SourceUnit,
                scanErrorMessage,
                this.GetSpan(startPosition, stopPosition),
                ErrorSinkWrapper.ScanErrorCode,
                Microsoft.Scripting.Severity.Error);
        }

        private Microsoft.Scripting.SourceSpan GetSpan(SourceLocation start, SourceLocation end)
        {
            var startLocation = this.GetLocation(start);
            var endLocation = this.GetLocation(end);
            if (!startLocation.IsValid || endLocation.IsValid)
                return Microsoft.Scripting.SourceSpan.Invalid;
            return new Microsoft.Scripting.SourceSpan(startLocation, endLocation);
        }

        /// <summary>
        /// Helper function to convert source indexes to SourceLocation.
        /// </summary>
        /// <param name="position">0-based source index in the source stream.</param>
        /// <returns>SourceLocation object for the given source index.</returns>
        /// <remarks>
        /// Current implementation is dumb, so it will re-read the source file to find the location.
        /// </remarks>
        private Microsoft.Scripting.SourceLocation GetLocation(SourceLocation position)
        {
            return new Microsoft.Scripting.SourceLocation(position.Position, position.Line, position.Column);
            //if (this.LineInfoMap == null)
            //{
            //    using (var reader = this.SourceUnit.GetReader())
            //        this.LineInfoMap = SourceLocationInfo.BuildLineInfoMap(reader);
            //}

            //return SourceLocationInfo.FindLocation(this.LineInfoMap, position.Position);
        }


        ///// <summary>
        ///// Caching of line information
        ///// </summary>
        //private List<SourceLocationInfo> LineInfoMap;

        ///// <summary>
        ///// Structure used for caching of positions to line indexes.
        ///// </summary>
        //private class SourceLocationInfo : IComparable<SourceLocationInfo>
        //{
        //    public int StartIndex;
        //    public int EndIndex;
        //    public int LineIndex;

        //    /// <summary>
        //    /// This creates a dummy object used for searching.
        //    /// </summary>
        //    /// <param name="position"></param>
        //    public SourceLocationInfo(int position)
        //    {
        //        this.StartIndex = position;
        //        this.EndIndex = position;
        //    }

        //    public SourceLocationInfo(int startIndex, int endIndex, int lineIndex)
        //    {
        //        if ((startIndex < 0) || (endIndex < startIndex) || (lineIndex < 1) || (lineIndex > (startIndex + 1)))
        //            throw new ArgumentException();
        //        this.StartIndex = startIndex;
        //        this.EndIndex = endIndex;
        //        this.LineIndex = lineIndex;
        //    }

        //    public int CompareTo(SourceLocationInfo other)
        //    {
        //        if (other == null)
        //            throw new ArgumentNullException();

        //        if (this.EndIndex < other.StartIndex)
        //            return -2; // We end before <other> begins.
        //        if (this.StartIndex > other.EndIndex)
        //            return 2; // We start after <other> ends.
        //        // OK, there is some overlap.
        //        if ((this.StartIndex > other.StartIndex) && (this.EndIndex <= other.EndIndex))
        //            return 1; // There is an overlap, but <other> starts before us ... so we are bigger.
        //        if ((this.StartIndex <= other.StartIndex) && (this.EndIndex < other.EndIndex))
        //            return -1; // There is an overlap, but <other> ends after us .. so we are smaller.

        //        // Here, either:
        //        //  - The two intervals are identical
        //        //  - We are complitely within <other>
        //        //  - <other> is complitely within us.
        //        return 0;
        //    }

        //    public override string ToString()
        //    {
        //        return String.Format("{0} : [{1} - {2}]", this.LineIndex, this.StartIndex, this.EndIndex);
        //    }

        //    //public static List<SourceLocationInfo> BuildLineInfoMap(System.IO.TextReader reader)
        //    //{
        //    //    if (reader == null)
        //    //        throw new ArgumentNullException();

        //    //    int startIndex = 0;
        //    //    int currentIndex = 0;
        //    //    int colIndex = 1;
        //    //    int lineIndex = 1;
        //    //    List<SourceLocationInfo> result = new List<SourceLocationInfo>();
        //    //    while (true)
        //    //    {
        //    //        int ch = reader.Read();
        //    //        if (ch == -1)
        //    //            return result;

        //    //        if ((ch == 13) || (ch == 10))
        //    //        {
        //    //            if ((ch == 13) && (reader.Peek() == 10))
        //    //            {
        //    //                reader.Read();
        //    //                currentIndex++;
        //    //            }
        //    //            result.Add(new SourceLocationInfo(startIndex, currentIndex, lineIndex));
        //    //            lineIndex++;
        //    //            colIndex = 1;
        //    //            startIndex = currentIndex + 1;
        //    //        }
        //    //        else
        //    //        {
        //    //            colIndex++;
        //    //        };

        //    //        currentIndex++;
        //    //    }


        //    //}

        //    //public static SourceLocation FindLocation(List<SourceLocationInfo> lineInfoMap, int position)
        //    //{
        //    //    int idx = lineInfoMap.BinarySearch(new SourceLocationInfo(position));
        //    //    if (idx < 0)
        //    //        return SourceLocation.Invalid;
        //    //    SourceLocationInfo info = lineInfoMap[idx];
        //    //    return new SourceLocation(position, info.LineIndex, position - info.StartIndex + 1);
        //    //}
        //}
    }

}
