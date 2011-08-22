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
using IronSmalltalk.Runtime.Internal;
using System.Numerics;
using IronSmalltalk.Common;
using System.Collections.Concurrent;

namespace IronSmalltalk.Runtime.Internal
{
    /// <summary>
    /// This class contains the mappings between .Net types and Smalltalk classes.
    /// It is essential for the runtime to be able to find behavior (methods) for 
    /// a given object.
    /// </summary>
    /// <remarks>
    /// For performance reasons, because the values are read very often,
    /// we don't use properties but fields. It is illegal to set them to null,
    /// but we can't guard against that unless we have properties and backing fields,
    /// and we don't want to do that (critical performance). So, don't set them to null!
    /// </remarks>
    public class NativeTypeClassMap
    {
        //public MethodDictionary Message;
        //public MethodDictionary Array;
        //public MethodDictionary Float;
        //public MethodDictionary Symbol;
        //public MethodDictionary Decimal;
        //public MethodDictionary Error;

        internal NativeTypeClassMap(SmalltalkRuntime runtime)
        {
            if (runtime == null)
                throw new ArgumentNullException();
            this.Runtime = runtime;
            this.TypeClassMap = new ConcurrentDictionary<Type, SmalltalkClass>();
        }

        /// <summary>
        /// The Smalltalk runtime that this object is responsible for.
        /// </summary>
        public SmalltalkRuntime Runtime { get; private set; }

        private readonly ConcurrentDictionary<Type, SmalltalkClass> TypeClassMap;

        private SmalltalkClass _object;

        /// <summary>
        /// The SmalltalkClass for the class named 'Object'.
        /// </summary>
        public SmalltalkClass Object
        {
            get
            {
                if (this._object == null)
                    this._object = this.Runtime.GetClass("Object");
                return this._object;
            }
            set
            {
                this._object = value;
            }
        }

        /// <summary>
        /// The SmalltalkClass for .Net types that do not explicitely map to other types. This is the fall-back class.
        /// </summary>
        public SmalltalkClass Native { get; private set; }

        /// <summary>
        /// The SmalltalkClass for the "nil" object.
        /// </summary>
        public SmalltalkClass UndefinedObject { get; private set; }

        /// <summary>
        /// The SmalltalkClass for the "true" object.
        /// </summary>
        public SmalltalkClass True { get; private set; }

        /// <summary>
        /// The SmalltalkClass for the "false" object.
        /// </summary>
        public SmalltalkClass False { get; private set; }

        /// <summary>
        /// The SmalltalkClass for System.Int32 objects.
        /// </summary>
        public SmalltalkClass SmallInteger { get; private set; }

        /// <summary>
        /// The SmalltalkClass for System.Char objects.
        /// </summary>
        public SmalltalkClass Character { get; private set; }

        /// <summary>
        /// The SmalltalkClass for System.String objects.
        /// </summary>
        public SmalltalkClass String { get; private set; }

        /// <summary>
        /// The SmalltalkClass for System.Double objects.
        /// </summary>
        public SmalltalkClass Float { get; private set; }

        /// <summary>
        /// The SmalltalkClass for System.Decimal objects.
        /// </summary>
        public SmalltalkClass SmallDecimal { get; private set; }

        /// <summary>
        /// The SmalltalkClass for System.Numerics.BigInteger objects.
        /// </summary>
        public SmalltalkClass BigInteger { get; private set; }

        /// <summary>
        /// The SmalltalkClass for IronSmalltalk.Common.BigDecimal objects.
        /// </summary>
        public SmalltalkClass BigDecimal { get; private set; }

        /// <summary>
        /// The SmalltalkClass for IronSmalltalk.Runtime.Symbol objects.
        /// </summary>
        public SmalltalkClass Symbol { get; private set; }

        /// <summary>
        /// The SmalltalkClass for IronSmalltalk.Runtime.SmalltalkClass objects. (the class of classes)
        /// </summary>
        public SmalltalkClass Class { get; private set; }

        /// <summary>
        /// The SmalltalkClass for IronSmalltalk.Runtime.Pool objects.
        /// </summary>
        public SmalltalkClass Pool { get; private set; }

        /// <summary>
        /// The SmalltalkClass for System.Type and System.Type derived types.
        /// </summary>
        public SmalltalkClass SystemType { get; private set; }

        /// <summary>
        /// Map a .Net Type to SmalltalkClass.
        /// </summary>
        /// <param name="type">The .Net Type to map to SmalltalkClass.</param>
        /// <returns>The SmalltalkClass for the given .Net Type or null if no mapping exists.</returns>
        public SmalltalkClass GetSmalltalkClass(Type type)
        {
            if (type == null)
                return null;
            SmalltalkClass result;
            if (this.TypeClassMap.TryGetValue(type, out result))
                return result;

            // System.Type is hardcoded here. We need this so we can handle all concrete derived types,
            // otherwise we will need to map every single derived type manually ... and that may give errors!
            if ((type.IsSubclassOf(typeof(Type)) || (type == typeof(Type))) && (this.SystemType != null))
                return this.SystemType;

            // This is the interesting part ... ask the Smalltalk code to return the ST class for a .Net type
            // IMPROVE: This has the potention to end in a loop if the ST code is not carefull enough not to ask
            // for the class of a class that needs to be resolved by ST code. Add a recursive check or similar!
            // NB: If we do recursion test, we must be aware of multiple threads resolving the same class ... :-/
            dynamic runtime = this.Runtime;
            // Callback into Smalltalk. See SmalltalkRuntime>>getSmalltalkClassForType:
            result = runtime.GetSmalltalkClassForType(type);
            // Register the class, so we won't have to do another lookup
            this.TypeClassMap[type] = result;

            return result;
        }


        /// <summary>
        /// Register a .Net type identified by its type-name to be mapped to the given Smalltalk class.
        /// </summary>
        /// <param name="cls">Required. Smalltalk class to map to.</param>
        /// <param name="typeName">Required. Type-name of the .Net type to be mapped to Smalltalk class. See remarks.</param>
        /// <remarks>
        /// The type-name can be one of:
        /// - Reserved name: "true", "false", "null", "nil", "native", "class", "symbol", "pool", "type"
        /// - C# alias, e.g. "string", "bool", "int", "float" etc.
        /// - IronSmalltak type prefixed with "_", e.g. "_SmalltalkRuntime".
        /// - mscorlib type (without any prefixes), e.g. "System.DateTime".
        /// - Any other type using assembly qualified name, e.g. "System.Numerics.BigInteger, System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        /// </remarks>
        public void RegisterClass(SmalltalkClass cls, string typeName)
        {
            if (cls == null)
                throw new ArgumentNullException("cls");
            if (System.String.IsNullOrWhiteSpace(typeName))
                throw new SmalltalkDefinitionException(System.String.Format(
                    "Invalid type name. Check native type mapping for class {0}",
                    cls.Name), new ArgumentNullException("typeName"));

            // Special case handling for Smalltalk specific classes.
            if (typeName == "true")
            {
                this.True = cls;
                return;
            }
            if (typeName == "false")
            {
                this.False = cls;
                return;
            }
            if ((typeName == "null") || (typeName == "nil"))
            {
                this.UndefinedObject = cls;
                return;
            }
            if (typeName == "native")
            {
                this.Native = cls;
                return;
            }
            if (typeName == "class")
            {
                this.Class = cls;
                return;
            }
            if (typeName == "symbol")
            {
                this.Symbol = cls;
                return;
            }
            if (typeName == "pool")
            {
                this.Pool = cls;
                return;
            }
            if (typeName == "type")
            {
                this.SystemType = cls;
                return;
            }
            //struct, enum, delegate, event ... do we need to handle those?

            // Generic classes ... remap C# convenience names to FCL names.
            Type type;
            if (typeName.StartsWith(NativeTypeClassMap.IstTypenamePrefix))
            {
                type = NativeTypeClassMap.GetIstType(typeName);
            }
            else
            {
                typeName = NativeTypeClassMap.RemapTypeName(typeName);
                try
                {
                    type = Type.GetType(typeName, true, false);
                }
                catch (Exception ex)
                {
                    throw new SmalltalkDefinitionException(System.String.Format(
                        "Cannot find type {0}. Check native type mapping for class {1}",
                        typeName, cls.Name), ex);
                }
            }

            // Some very common types are cached directly
            if (type == typeof(object))
                this.Object = cls;
            else if (type == typeof(int))
                this.SmallInteger = cls;
            else if (type == typeof(char))
                this.Character = cls;
            else if (type == typeof(string))
                this.String = cls;
            else if (type == typeof(double))
                this.Float = cls;
            else if (type == typeof(decimal))
                this.SmallDecimal = cls;
            else if (type == typeof(BigInteger))
                this.BigInteger = cls;
            else if (type == typeof(BigDecimal))
                this.BigDecimal = cls;

            // Add the type to the map ...
            this.TypeClassMap[type] = cls;
        }

        private static string RemapTypeName(string name)
        {
            if(name == "bool") 
                return "System.Boolean";	
            if(name == "byte") 
                return "System.Byte";	
            if(name == "sbyte") 
                return "System.SByte";	
            if(name == "char") 
                return "System.Char";	
            if(name == "decimal") 
                return "System.Decimal";	
            if(name == "double") 
                return "System.Double";	
            if(name == "float") 
                return "System.Single";	
            if(name == "int") 
                return "System.Int32";	
            if(name == "uint") 
                return "System.UInt32";	
            if(name == "long") 
                return "System.Int64";	
            if(name == "ulong") 
                return "System.UInt64";	
            if(name == "object") 
                return "System.Object";	
            if(name == "short") 
                return "System.Int16";	
            if(name == "ushort") 
                return "System.UInt16";	
            if(name == "string") 
                return "System.String";
            return name;
        }

        /// <summary>
        /// Get and resoleve the .Net Type for the given type name.
        /// </summary>
        /// <param name="name">Type-name of the .Net type to be resolved. See remarks.</param>
        /// <remarks>
        /// The type-name can be one of:
        /// - C# alias, e.g. "string", "bool", "int", "float" etc.
        /// - IronSmalltak type prefixed with "_", e.g. "_SmalltalkRuntime".
        /// - mscorlib type (without any prefixes), e.g. "System.DateTime".
        /// - Any other type using assembly qualified name, e.g. "System.Numerics.BigInteger, System.Numerics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        /// </remarks>
        /// <returns>Returns System.Type with the given name or null of a type did not exist (could not be loaded).</returns>
        public static Type GetType(string name)
        {
            if(name == "bool") 
                return typeof(bool);	
            if(name == "byte") 
                return typeof(byte);	
            if(name == "sbyte") 
                return typeof(sbyte);	
            if(name == "char") 
                return typeof(char);	
            if(name == "decimal") 
                return typeof(decimal);	
            if(name == "double") 
                return typeof(double);	
            if(name == "float") 
                return typeof(float);	
            if(name == "int") 
                return typeof(int);	
            if(name == "uint") 
                return typeof(uint);	
            if(name == "long") 
                return typeof(long);	
            if(name == "ulong") 
                return typeof(ulong);	
            if(name == "object") 
                return typeof(object);	
            if(name == "short") 
                return typeof(short);	
            if(name == "ushort") 
                return typeof(ushort);	
            if(name == "string") 
                return typeof(string);
            if ((name != null) && name.StartsWith(NativeTypeClassMap.IstTypenamePrefix))
                return NativeTypeClassMap.GetIstType(name);
            return Type.GetType(name, false, false);
        }

        private static readonly string IstTypenamePrefix = "_";

        private static readonly Dictionary<string, Type> IstTypes = new Dictionary<string, Type>();

        private static readonly string[] IstNamespaces = new string[] {
            "IronSmalltalk.{0}, IronSmalltalk.Runtime",
            "IronSmalltalk.Common.{0}, IronSmalltalk.Common",
            "IronSmalltalk.Runtime.{0}, IronSmalltalk.Runtime",
            "IronSmalltalk.Runtime.Internal.{0}, IronSmalltalk.Runtime",
            "IronSmalltalk.Runtime.Bindings.{0}, IronSmalltalk.Runtime"
        };

        private static Type GetIstType(string name)
        {
            if (System.String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException();

            Type result;
            if (NativeTypeClassMap.IstTypes.TryGetValue(name, out result))
            {
                if (result == null)
                    throw new SmalltalkDefinitionException(System.String.Format(
                        "Cannot fint IronSmalltalk type {0}", name));
                return result;
            }

            string postfix = name.Substring(NativeTypeClassMap.IstTypenamePrefix.Length);
            for (int i = 0; i < NativeTypeClassMap.IstNamespaces.Length; i++)
			{
                string typename = NativeTypeClassMap.IstNamespaces[i];
                typename = System.String.Format(typename, postfix);

                result = Type.GetType(typename, false, false);
                if (result != null)
                {
                    NativeTypeClassMap.IstTypes[name] = result;
                    return result;
                }
			}

            NativeTypeClassMap.IstTypes[name] = null;
            throw new SmalltalkDefinitionException(System.String.Format(
                                    "Cannot fint IronSmalltalk type {0}", name));
        }
    }
}
