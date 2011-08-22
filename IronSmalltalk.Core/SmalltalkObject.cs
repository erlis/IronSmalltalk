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
using System.Dynamic;
using System.Linq.Expressions;

namespace IronSmalltalk.Runtime
{
    /// <summary>
    /// An instance of a Smalltalk class that only contains named instance variables.
    /// </summary>
    /// <remarks>
    /// Some classes cannot create instances of a SmalltalkObject and 
    /// either have built-in .Net types or wrap arround existing .Net types.
    /// </remarks>
    public partial class SmalltalkObject 
    {
        /// <summary>
        /// The SmalltalkClass that this object is instance of.
        /// </summary>
        public readonly SmalltalkClass Class;

        /// <summary>
        /// Array with the instance variables of the object.
        /// </summary>
        public readonly object[] InstanceVariables;

        /// <summary>
        /// Create a new SmalltalkObject for a given SmalltalkClass.
        /// </summary>
        /// <param name="cls">The SmalltalkClass that this object will be instance of.</param>
        public SmalltalkObject(SmalltalkClass cls)
        {
            if (cls == null)
                throw new ArgumentNullException();
            this.Class = cls;
            this.InstanceVariables = new object[cls.InstanceSize];
        }

        /// <summary>
        /// A Smalltalk object containing variable number of unnamed instance variables containing binary data.
        /// </summary>
        public class ByteIndexableSmalltalkObject : SmalltalkObject
        {
            /// <summary>
            /// The byte-indexable contents (instance variables) of the receiver.
            /// </summary>
            public readonly byte[] Contents;

            /// <summary>
            /// Create a new ByteIndexable SmalltalkObject for a given SmalltalkClass.
            /// </summary>
            /// <param name="cls">The SmalltalkClass that this object will be instance of.</param>
            /// <param name="objectSize">Number of unnamed instance variables.</param>
            public ByteIndexableSmalltalkObject(SmalltalkClass cls, int objectSize)
                : base(cls)
            {
                if (cls.InstanceState != SmalltalkClass.InstanceStateEnum.ByteIndexable)
                    throw new ArgumentOutOfRangeException("Expected a ByteIndexable class");
                if (objectSize < 0)
                    throw new ArgumentOutOfRangeException("objectSize");
                this.Contents = new byte[objectSize];
            }
        }

        /// <summary>
        /// A Smalltalk object containing variable number of unnamed instance variables referencing other objects.
        /// </summary>
        public class ObjectIndexableSmalltalkObject : SmalltalkObject
        {
            /// <summary>
            /// The object-indexable contents (instance variables) of the receiver.
            /// </summary>
            public readonly object[] Contents;

            /// <summary>
            /// Create a new ObjectIndexable SmalltalkObject for a given SmalltalkClass.
            /// </summary>
            /// <param name="cls">The SmalltalkClass that this object will be instance of.</param>
            /// <param name="objectSize">Number of unnamed instance variables.</param>
            public ObjectIndexableSmalltalkObject(SmalltalkClass cls, int objectSize)
                : base(cls)
            {
                if (cls.InstanceState != SmalltalkClass.InstanceStateEnum.ObjectIndexable)
                    throw new ArgumentOutOfRangeException("Expected an ObjectIndexable class");
                if (objectSize < 0)
                    throw new ArgumentOutOfRangeException("objectSize");
                this.Contents = new object[objectSize];
            }
        }
    }
}
