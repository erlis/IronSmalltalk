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

namespace IronSmalltalk.Compiler.LexicalAnalysis
{
    public partial class ScanResult
    {
        /// <summary>
        /// Currently scanned character.
        /// </summary>
        /// <remarks>
        /// This always contains '0x00' in case if EOF. Makes some of the test functions easier.
        /// </remarks>
        public char Character { get; private set; }

        /// <summary>
        /// Indicates End-Of-File.
        /// </summary>
        /// <remarks>
        /// If set to true, then Character must be set to '0x00'.
        /// </remarks>
        public bool EndOfFile { get; private set; }

        /// <summary>
        /// Creates a new scan result
        /// </summary>
        public ScanResult()
        {
        }

        /// <summary>
        /// Creates a new scan result and initializes it based on the given character code.
        /// </summary>
        /// <param name="c">Result of TextRead.Read() or TextReader.Peek().</param>
        public ScanResult(int c)
            : this()
        {
            this.SetResult(c);
        }

        /// <summary>
        /// Set the ScanResult values based on the given character code.
        /// </summary>
        /// <param name="c">Result of TextRead.Read() or TextReader.Peek().</param>
        public void SetResult(int c)
        {
            if (c == -1)
            {
                this.EndOfFile = true;
                this.Character = '\x0000';
            }
            else
            {
                this.EndOfFile = false;
                this.Character = (char)c;
            }
        }
    }
}
