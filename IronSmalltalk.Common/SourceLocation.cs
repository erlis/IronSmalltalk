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

namespace IronSmalltalk.Common
{
    /// <summary>
    /// Location of the chunk of source code in the source code file.
    /// </summary>
    /// <remarks>
    /// The DLR has a class SourceLocation with the same/similar functionality,
    /// but we don't want to reference the DLR assembly (Miscrosoft.Scripting)
    /// from this project, so we have implemented our own class that is
    /// compatible with the DLR class (if we would need to convert between the two).
    /// </remarks>
    [Serializable]
    public struct SourceLocation : IComparable, IComparable<SourceLocation>, IEquatable<SourceLocation>
    {
        /// <summary>
        /// Source code start position expressed in 0-based character index from the start of the source file.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Source code start position expressed as 1-based line index from the start of the source file.
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// Source code start position expressed as 1-based column index from the start of the line.
        /// </summary>
        public int Column { get; private set; }

        // <summary>
        // Creates a new, but invalid source location.
        // </summary>
        // NB: This is a valid constructor, but commented out for technical reasons
        //public SourceLocation()
        //{
        //}

        /// <summary>
        /// Returns an invalid source location.
        /// </summary>
        public static SourceLocation Invalid
        {
            get
            {
                SourceLocation result = new SourceLocation();
                result.Position = -1;
                result.Line = 0;
                result.Column = 0;
                return result;
            }
        }

        /// <summary>
        /// Create a new source location.
        /// </summary>
        /// <param name="index">Character position in the source code (0-based).</param>
        /// <param name="line">Line in the source code (1-based).</param>
        /// <param name="column">Column in the source code (1-based).</param>
        public SourceLocation(int index, int line, int column)
            : this()
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");
            if (line < 1)
                throw new ArgumentOutOfRangeException("line");
            if (column < 1)
                throw new ArgumentOutOfRangeException("column");

            this.Position = index;
            this.Line = line;
            this.Column = column;
        }

        /// <summary>
        /// Determines if the current object points to a valid source code location.
        /// </summary>
        public bool IsValid
        {
            get { return (this.Line > 0) && (this.Column > 0); }
        }

        /// <summary>
        /// Returns a value that indicates whether the values of two source locations values are equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if the left and right parameters have the same value; otherwise, false.</returns>
        public static bool operator ==(SourceLocation left, SourceLocation right)
        {
            return (left.Column == right.Column) && (left.Line == right.Line) && (left.Position == right.Position);
        }

        /// <summary>
        /// Returns a value that indicates whether the values of two source locations values are not equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if the left and right parameters have different value; otherwise, false.</returns>
        public static bool operator !=(SourceLocation left, SourceLocation right)
        {
            return (left.Column != right.Column) && (left.Line != right.Line) && (left.Position != right.Position);
        }

        /// <summary>
        /// Returns a value that indicates whether a SourceLocation value is greater than another SourceLocation value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if left is greater than right; otherwise, false.</returns>
        public static bool operator >(SourceLocation left, SourceLocation right)
        {
            return left.Position > right.Position;
        }

        /// <summary>
        /// Returns a value that indicates whether a SourceLocation value is greater than or equal to another SourceLocation value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if left is greater than right; otherwise, false.</returns>
        public static bool operator >=(SourceLocation left, SourceLocation right)
        {
            return left.Position >= right.Position;
        }

        /// <summary>
        /// Returns a value that indicates whether a SourceLocation value is less than another SourceLocation value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if left is less than right; otherwise, false.</returns>
        public static bool operator <(SourceLocation left, SourceLocation right)
        {
            return left.Position < right.Position;
        }

        /// <summary>
        /// Returns a value that indicates whether a SourceLocation value is less than or equal to another SourceLocation value.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns>True if left is less than or equal to right; otherwise, false.</returns>
        public static bool operator <=(SourceLocation left, SourceLocation right)
        {
            return left.Position <= right.Position;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has the following meanings: Value Meaning Less than zero
        /// This object is less than the other parameter.Zero This object is equal to
        /// other. Greater than zero This object is greater than other.
        /// </returns>
        public int CompareTo(SourceLocation other)
        {
            return this.Position.CompareTo(other.Position);
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns
        /// an integer that indicates whether the current instance precedes, follows,
        /// or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared.
        /// The return value has these meanings: Value Meaning Less than zero This instance
        /// is less than obj. Zero This instance is equal to obj. Greater than zero This
        /// instance is greater than obj.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is SourceLocation))
                throw new ArgumentException("Must be SourceLocation");
            return this.CompareTo((SourceLocation)obj);
        }

        /// <summary>
        /// Indicates whether the current source location is equal to another source location.
        /// </summary>
        /// <param name="other">A source location to compare with this source location.</param>
        /// <returns>True if the current source location is equal to the other parameter; otherwise, false.</returns>
        public bool Equals(SourceLocation other)
        {
            return (this == other);
        }

        /// <summary>
        /// Returns the hash code for the current SourceLocation object.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return ((this.Line << 16) ^ this.Column) ^ this.Position;
        }

        /// <summary>
        /// Indicates whether the current source location is equal to another object.
        /// </summary>
        /// <param name="obj">An object to compare with this source location.</param>
        /// <returns>True if the current source location is equal to the other parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SourceLocation))
                return false;
            return this == (SourceLocation)obj;
        }

        /// <summary>
        /// Returns the smaller of two source locations.
        /// </summary>
        /// <param name="other">The other of two source locations to compare.</param>
        /// <returns>The current source location itself or the parameter, whichever is smaller.</returns>
        public SourceLocation Min(SourceLocation other)
        {
            return (this < other) ? this : other;
        }

        /// <summary>
        /// Returns the largest of two source locations.
        /// </summary>
        /// <param name="other">The other of two source locations to compare.</param>
        /// <returns>The current source location itself or the parameter, whichever is largest.</returns>
        public SourceLocation Max(SourceLocation other)
        {
            return (this > other) ? this : other;
        }

        /// <summary>
        /// A human readable string repsesentation of the source location.
        /// </summary>
        /// <returns>A human readable string repsesentation of the source location.</returns>
        public override string ToString()
        {
            return String.Format(System.Globalization.CultureInfo.CurrentCulture,
                "Ln {0}, Col {1}, Pos {2}", this.Line, this.Column, this.Position);
        }
    }
}
