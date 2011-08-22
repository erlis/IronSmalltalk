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
using System.Globalization;

namespace IronSmalltalk.Runtime
{
    /// <summary>
    /// An object that is a smalltalk string.
    /// </summary>
    public class SmalltalkString
    {
        /// <summary>
        /// The contents of the string.
        /// </summary>
        /// <remarks>
        /// It does not contain NULL terminated character or similar.
        /// The variable may be null for empty strings and should be treat as if it was empty array.
        /// </remarks>
        public char[] Contents;

        /// <summary>
        /// Create a new SmalltalkString object with the given length and 
        /// allocate storage for the given number of characters.
        /// </summary>
        /// <param name="length">Number of characters that the string will container.</param>
        public SmalltalkString(int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException("length");
            this.Contents = new char[length];
        }

        /// <summary>
        /// Create a new SmalltalkString object based on the given contents.
        /// </summary>
        /// <param name="contents">Contents to initialize the SmalltalkString with.</param>
        public SmalltalkString(char[] contents)
            : this(contents, false)
        {
        }

        /// <summary>
        /// Create a new SmalltalkString object based on the given contents.
        /// </summary>
        /// <param name="contents">Contents to initialize the SmalltalkString with.</param>
        /// <param name="copyContents">Indicates if the contetns are copied or stored directly (shared).</param>
        public SmalltalkString(char[] contents, bool copyContents)
        {
            if (copyContents)
            {
                if (contents != null)
                {
                    this.Contents = new char[contents.Length];
                    Array.Copy(contents, this.Contents, contents.Length);
                }
            }
            else
            {
                this.Contents = contents;
            }
        }

        /// <summary>
        /// Create a new SmalltalkString object based on the given contents.
        /// </summary>
        /// <param name="contents">Contents to initialize the SmalltalkString with.</param>
        public SmalltalkString(string contents)
        {
            if (contents != null)
                this.Contents = contents.ToCharArray();
        }

        /// <summary>
        /// Create a new SmalltalkString object based on the given contents.
        /// </summary>
        /// <param name="ch1">First character of the string.</param>
        public SmalltalkString(char ch1)
        {
            this.Contents = new char[] { ch1 };
        }

        /// <summary>
        /// Create a new SmalltalkString object based on the given contents.
        /// </summary>
        /// <param name="ch1">First character of the string.</param>
        /// <param name="ch2">Second character of the string.</param>
        public SmalltalkString(char ch1, char ch2)
        {
            this.Contents = new char[] { ch1, ch2 };
        }

        /// <summary>
        /// Create a new SmalltalkString object based on the given contents.
        /// </summary>
        /// <param name="ch1">First character of the string.</param>
        /// <param name="ch2">Second character of the string.</param>
        /// <param name="ch3">Third character of the string.</param>
        public SmalltalkString(char ch1, char ch2, char ch3)
        {
            this.Contents = new char[] { ch1, ch2, ch3 };
        }

        /// <summary>
        /// Create a new SmalltalkString object based on the given contents.
        /// </summary>
        /// <param name="ch1">First character of the string.</param>
        /// <param name="ch2">Second character of the string.</param>
        /// <param name="ch3">Third character of the string.</param>
        /// <param name="ch4">Fourth character of the string.</param>
        public SmalltalkString(char ch1, char ch2, char ch3, char ch4)
        {
            this.Contents = new char[] { ch1, ch2, ch3, ch4 };
        }

        /// <summary>
        /// Return the length in characters of this SmalltalkString.
        /// </summary>
        public int Length
        {
            get
            {
                if (this.Contents == null)
                    return 0;
                else
                    return this.Contents.Length;
            }
        }

        /// <summary>
        /// Returns a System.String that represents the current SmalltalkString.
        /// </summary>
        /// <returns>A System.String that represents the current SmalltalkString.</returns>
        public override string ToString()
        {
            base.ToString();
            return (string)this;
        }

        /// <summary>
        /// Defines an implicit conversion of Symbol value to a String value.
        /// </summary>
        /// <param name="value">The value to convert to a String.</param>
        /// <returns>A String that contains the value of the value parameter.</returns>
        public static implicit operator string(SmalltalkString value)
        {
            if (value == null)
                return null;
            if (value.Contents == null)
                return String.Empty;
            return new String(value.Contents);
        }

        /// <summary>
        /// Get or set the character at a specified character position in the current  string.
        /// </summary>
        /// <param name="index">An 1-indexed character position in the current string.</param>
        /// <returns>A Unicode character.</returns>
        /// <exception cref="System.IndexOutOfRangeException"> index is greater than the length of this object or less than one.</exception>
        public char this[int index]
        {
            get
            {
                if (this.Contents == null)
                    throw new IndexOutOfRangeException();
                return this.Contents[index-1];
            }
            set
            {
                if (this.Contents == null)
                    throw new IndexOutOfRangeException(); 
                this.Contents[index - 1] = value;
            }
        }

        /// <summary>
        /// Returns a copy of this string converted to lowercase.
        /// </summary>
        /// <returns>A string in lowercase.</returns>
        public SmalltalkString ToUpper()
        {
            return this.ToUpper(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a copy of this string converted to lowercase.
        /// </summary>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>A string in lowercase.</returns>
        public SmalltalkString ToUpper(CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            if (this.Contents == null)
                return new SmalltalkString(0);
            char[] buffer = new char[this.Contents.Length];
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = Char.ToUpper(this.Contents[i]);
            return new SmalltalkString(buffer, false);
        }

        /// <summary>
        /// Returns a copy of this string converted to uppercase.
        /// </summary>
        /// <returns>A string in uppercase.</returns>
        public SmalltalkString ToLower()
        {
            return this.ToLower(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a copy of this string converted to uppercase.
        /// </summary>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>A string in uppercase.</returns>
        public SmalltalkString ToLower(CultureInfo culture)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            if (this.Contents == null)
                return new SmalltalkString(0);
            char[] buffer = new char[this.Contents.Length];
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = Char.ToLower(this.Contents[i]);
            return new SmalltalkString(buffer, false);
        }
    }
}
