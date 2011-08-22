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

namespace IronSmalltalk.Runtime.Internal
{
    /// <summary>
    /// Inteface that describes objects that are annotetable.
    /// </summary>
    public interface IAnnotetable
    {
        /// <summary>
        /// The annotation pairs associated with the annotetable object.
        /// </summary>
        IEnumerable<KeyValuePair<string, string>> Annotations { get; }

        /// <summary>
        /// Set (or overwrite) an annotation on the annotetable object.
        /// </summary>
        /// <param name="key">Key of the annotation.</param>
        /// <param name="value">Value or null to remove the annotation.</param>
        void Annotate(string key, string value);
    }

    /// <summary>
    /// Just a cache for empty annotations.
    /// </summary>
    internal static class AnnotationsHelper
    {
        /// <summary>
        /// Empty annotations
        /// </summary>
        public static readonly KeyValuePair<string, string>[] Empty = new KeyValuePair<string, string>[0];
    }
}
