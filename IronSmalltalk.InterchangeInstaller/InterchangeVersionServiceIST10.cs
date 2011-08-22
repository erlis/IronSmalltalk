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
using IronSmalltalk.Compiler.Interchange;
using System.IO;

namespace IronSmalltalk.Interchange
{
    /// <summary>
    /// Interchange Format Version Service for the IronSmalltalk interchange format.
    /// </summary>
    /// <remarks>
    /// Most of the functionality is implemented in the base class. 
    /// </remarks>
    public class InterchangeVersionServiceIST10 : InterchangeVersionService
    {
        /// <summary>
        /// Create and return a parser capable of parsing interchange nodes / interchange chunks.
        /// </summary>
        /// <remarks>
        /// The parser returned here is capable of parsing interchange chunks that follow the 
        /// interchange version this version service is supporting.
        /// </remarks>
        /// <param name="sourceChunk">Interchange chunk to be parsed.</param>
        /// <returns>Interchange format parser.</returns>
        protected override InterchangeFormatParser GetInterchangeParser(InterchangeChunk sourceChunk)
        {
            if (sourceChunk == null)
                throw new ArgumentNullException();
            InterchangeFormatParserIST10 parser = new InterchangeFormatParserIST10(new StringReader(sourceChunk.SourceChunk));
            parser.ErrorSink = sourceChunk;
            return parser;
        }
    }
}
