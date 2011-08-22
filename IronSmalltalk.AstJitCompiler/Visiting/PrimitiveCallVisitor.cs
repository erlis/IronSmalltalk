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
using System.Linq.Expressions;
using System.Reflection;
using IronSmalltalk.AstJitCompiler.Internals;
using IronSmalltalk.Compiler.SemanticAnalysis;
using IronSmalltalk.Runtime.Behavior;
using IronSmalltalk.Runtime.Internal;
using IronSmalltalk.Runtime.CodeGeneration.Bindings;
using IronSmalltalk.Runtime.Execution.Internals;
using ASTJIT = IronSmalltalk.AstJitCompiler.Internals;
using System.Dynamic;

namespace IronSmalltalk.Runtime.CodeGeneration.Visiting
{
    /// <summary>
    /// Encoder visitor for visiting and generating primitive calls.
    /// </summary>
    /// <remarks>
    /// This is the place where primitive calls are encoded to expressions and where most of the interop to the .Net framework happens.
    /// </remarks>
    public partial class PrimitiveCallVisitor : NestedEncoderVisitor<List<Expression>>
    {
        /// <summary>
        /// Indicates if there is Smalltalk fallback code after the primitive call.
        /// </summary>
        public bool HasFallbackCode { get; private set; }

        /// <summary>
        /// The enclosing method visitor that created this visitor and defines the context of this visitor.
        /// </summary>
        public MethodVisitor MethodVisitor { get; private set; }

        public PrimitiveCallVisitor(MethodVisitor enclosingVisitor, bool hasFallbackCode)
            : base(enclosingVisitor)
        {
            this.HasFallbackCode = hasFallbackCode;
            this.MethodVisitor = enclosingVisitor;
        }

        /// <summary>
        /// Visits the Primitive Call node.
        /// </summary>
        /// <param name="node">The node to visit.</param>
        /// <remarks>
        /// This is the place where primitive calls are encoded to expressions and where most of the interop to the .Net framework happens.
        /// </remarks>
        public override List<Expression> VisitPrimitiveCall(Compiler.SemanticNodes.PrimitiveCallNode node)
        {
            // Large case with the API conventions we support;
            // Each generates an Expression to perform the primitive call.
            Expression primitiveCall;
            if (node.ApiConvention.Value == "primitive:")
            {
                if (node.ApiParameters.Count < 1)
                    throw new PrimitiveSemanticException(CodeGenerationErrors.WrongNumberOfParameters, node);
                List<string> parameters = new List<string>();
                for (int i = 1; i < node.ApiParameters.Count; i++)
                    parameters.Add(node.ApiParameters[i].Value);

                primitiveCall = this.GenerateBuildinPrimitive(node.ApiParameters[0].Value, parameters);

                if (primitiveCall == null)
                    throw new PrimitiveSemanticException(String.Format(CodeGenerationErrors.WrongPrimitive, node.ApiParameters[0].Value), node);
            }
            else if (node.ApiConvention.Value == "static:")
            {
                if (node.ApiParameters.Count < 2)
                    throw new PrimitiveSemanticException(CodeGenerationErrors.WrongNumberOfParameters, node);

                primitiveCall = this.GenerateStaticMethodCall(
                    this.GetDefiningType(node),
                    node.ApiParameters[1].Value, 
                    this.GetArgumentTypes(node, 2),
                    BindingFlags.InvokeMethod | BindingFlags.Static);
            }
            else if (node.ApiConvention.Value == "call:")
            {
                if (node.ApiParameters.Count < 3)
                    throw new PrimitiveSemanticException(CodeGenerationErrors.WrongNumberOfParameters, node);

                primitiveCall = this.GenerateInstanceMethodCall(
                    this.GetDefiningType(node),
                    node.ApiParameters[1].Value,
                    this.GetArgumentTypes(node, 2),
                    BindingFlags.InvokeMethod | BindingFlags.Instance);
            }
            else if (node.ApiConvention.Value == "ctor:")
            {
                if (node.ApiParameters.Count < 1)
                    throw new PrimitiveSemanticException(CodeGenerationErrors.WrongNumberOfParameters, node);

                primitiveCall = this.GenerateConstructorCall(this.GetDefiningType(node), this.GetArgumentTypes(node, 1));
            }
            else if (node.ApiConvention.Value == "get_property:")
            {
                if (node.ApiParameters.Count < 3)
                    throw new PrimitiveSemanticException(CodeGenerationErrors.WrongNumberOfParameters, node);

                string typeName = node.ApiParameters[node.ApiParameters.Count - 1].Value;
                // Get the parameter type, if we fail to find one, throw an exception now! 
                Type returnType = NativeTypeClassMap.GetType(typeName);
                if (returnType == null)
                    throw new ASTJIT.PrimitiveInvalidTypeException(String.Format(CodeGenerationErrors.WrongTypeName, typeName), node);

                primitiveCall = this.GeneratePropertyCall(
                    this.GetDefiningType(node), 
                    node.ApiParameters[1].Value, 
                    returnType, 
                    this.GetArgumentTypes(node, 2, node.ApiParameters.Count - 3),
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Static);
            }
            else if (node.ApiConvention.Value == "set_property:")
            {
                if (node.ApiParameters.Count < 3)
                    throw new PrimitiveSemanticException(CodeGenerationErrors.WrongNumberOfParameters, node);

                string typeName = node.ApiParameters[node.ApiParameters.Count - 1].Value;
                // Get the parameter type, if we fail to find one, throw an exception now! 
                Type returnType = NativeTypeClassMap.GetType(typeName);
                if (returnType == null)
                    throw new ASTJIT.PrimitiveInvalidTypeException(String.Format(CodeGenerationErrors.WrongTypeName, typeName), node);

                primitiveCall = this.GeneratePropertyCall(
                    this.GetDefiningType(node),
                    node.ApiParameters[1].Value,
                    returnType,
                    this.GetArgumentTypes(node, 2, node.ApiParameters.Count - 3), 
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Static);
            }
            else if (node.ApiConvention.Value == "get_field:")
            {
                if (node.ApiParameters.Count != 2)
                    throw new PrimitiveSemanticException(CodeGenerationErrors.WrongNumberOfParameters, node);

                primitiveCall = this.GenerateFieldCall(this.GetDefiningType(node), node.ApiParameters[1].Value,
                    BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Static);
            }
            else if (node.ApiConvention.Value == "set_field:")
            {
                if (node.ApiParameters.Count != 2)
                    throw new PrimitiveSemanticException(CodeGenerationErrors.WrongNumberOfParameters, node);

                primitiveCall = this.GenerateFieldCall(this.GetDefiningType(node), node.ApiParameters[1].Value,
                    BindingFlags.SetField | BindingFlags.Instance | BindingFlags.Static);
            }
            else
            {
                throw new PrimitiveSemanticException(CodeGenerationErrors.UnexpectedCallingconvention, node);
            }

            // We need to handle void returns, because Smalltalk always needs a valid receiver.
            NameBinding selfBinding = this.MethodVisitor.GetBinding(SemanticConstants.Self);
            if (primitiveCall.Type == typeof(void))
                primitiveCall = Expression.Block(primitiveCall, selfBinding.GenerateReadExpression(this));
            else if (primitiveCall.Type != typeof(object))
                primitiveCall = Expression.Convert(primitiveCall, typeof(object));
            // A successful primitive call must return directly without executing any other statements.
            primitiveCall = this.Return(primitiveCall);

            List<Expression> result = new List<Expression>();
            if (this.HasFallbackCode)
            {
                // This is the case, where some Smalltalk fall-back code follows the primitive call.
                // In this case, we encapsulate the primitive call in a try-catch block, similar to:
                //      object _exception;          // optional ... defined by the Smalltalk method
                //      try
                //      {
                //          return (object) primitiveCall();
                //      } catch (Exception exception) {
                //          _exception = exception;         // optional ... only if "_exception" variable is declared.
                //      };
                Expression handler;
                // This is the special hardcoded temp variable we are looking for.
                NameBinding exceptionVariable = this.MethodVisitor.GetLocalVariable("_exception");
                ParameterExpression expetionParam = Expression.Parameter(typeof(Exception), "exception");
                if (!exceptionVariable.IsErrorBinding && (exceptionVariable is IAssignableBinding))
                    // Case of handler block that sets the "_exception" temporary variable and has "self" as the last statement.
                    handler = Expression.Block(
                        ((IAssignableBinding)exceptionVariable).GenerateAssignExpression(expetionParam, this),
                        Expression.Convert(selfBinding.GenerateReadExpression(this), typeof(object)));
                else
                    // Case of handler block that has "self" as the one and only statement
                    handler = Expression.Convert(selfBinding.GenerateReadExpression(this), typeof(object));
                // Create the try/catch block.
                result.Add(Expression.TryCatch(primitiveCall,
                    Expression.Catch(typeof(BlockResult), Expression.Rethrow(typeof(object))),
                    Expression.Catch(expetionParam, handler)));
            }
            else
            {
                // This is the case, where none Smalltalk fall-back code follows the primitive call.
                // The API call is called directly without any encapsulation in a try-catch block, similar to:
                //      return (object) primitiveCall();
                // If it fails, it fails and an exception is thrown. The sender of the Smalltalk method 
                // is responsible for handling exceptions manually.
                result.Add(primitiveCall);
            }

            return result;
        }

        #region Helpers

        private Type[] GetArgumentTypes(Compiler.SemanticNodes.PrimitiveCallNode node, int start)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            return this.GetArgumentTypes(node, start, node.ApiParameters.Count - start);
        }

        private Type[] GetArgumentTypes(Compiler.SemanticNodes.PrimitiveCallNode node, int start, int count)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            if (start < 0)
                throw new ArgumentOutOfRangeException("start");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if ((start + count) > node.ApiParameters.Count)
                throw new ArgumentOutOfRangeException("count");

            // Then get type definitions for each parameter we are to pass to the member.
            Type[] argumentTypes = new Type[count];
            for (int i = 0; i < argumentTypes.Length; i++)
            {
                string typeName = node.ApiParameters[i+start].Value;
                // Special case, for lazy people, the first parameter type can be "this", meaning the same as the defining type.
                if ((i == 0) && (typeName == "this"))
                    typeName = node.ApiParameters[0].Value;
                // Get the parameter type, if we fail to find one, throw an exception now! 
                Type type = NativeTypeClassMap.GetType(typeName);
                if (type == null)
                    throw new ASTJIT.PrimitiveInvalidTypeException(String.Format(CodeGenerationErrors.WrongTypeName, typeName), node);
                argumentTypes[i] = type;
            }

            return argumentTypes;
        }

        /// <summary>
        /// Get the Type that is supposed to define the member we are about to invoke.
        /// </summary>
        /// <param name="node">Parse node describing the primitive call.</param>
        /// <returns>Type that is supposed to define the member we are about to invoke.</returns>
        private Type GetDefiningType(Compiler.SemanticNodes.PrimitiveCallNode node)
        {
            String definingTypeName = node.ApiParameters[0].Value;
            // Get the type that is expected to implement the member we are looking for.
            Type definingType = NativeTypeClassMap.GetType(definingTypeName);
            if (definingType == null)
                throw new ASTJIT.PrimitiveInvalidTypeException(String.Format(CodeGenerationErrors.WrongTypeName, definingTypeName), node);
            return definingType;
        }

        /// <summary>
        /// Get the expression needed to perform a static method call.
        /// </summary>
        /// <param name="type">Type that is expected to define the method we are looking for.</param>
        /// <param name="methodName">Name of the method to invoke.</param>
        /// <param name="argumentTypes">Types of arguments that we are to pass to the method.</param>
        /// <returns>Expression for the call</returns>
        private Expression GenerateStaticMethodCall(Type type, string methodName, Type[] argumentTypes, BindingFlags bindingFlags)
        {
            // Lookup the method ... matching argument types.
            MethodInfo method = type.GetMethod(methodName, BindingFlags.FlattenHierarchy | BindingFlags.Public | bindingFlags,
                null, argumentTypes, null);
            // If no method found, throw an exception.
            if (method == null)
                throw new PrimitiveInvalidMemberException(String.Format(CodeGenerationErrors.MissingMethod, type.Name, methodName));
            return Expression.Call(method, this.GetArguments(argumentTypes));
        }

        /// <summary>
        /// Get the expression needed to perform an instance method call.
        /// </summary>
        /// <param name="type">Type that is expected to define the method we are looking for.</param>
        /// <param name="methodName">Name of the method to invoke.</param>
        /// <param name="argumentTypes">Types of arguments that we are to pass to the method.</param>
        /// <returns>Expression for the call</returns>
        private Expression GenerateInstanceMethodCall(Type type, string methodName, Type[] argumentTypes, BindingFlags bindingFlags)
        {
            Type[] matchTypes = new Type[argumentTypes.Length - 1];
            Array.Copy(argumentTypes, 1, matchTypes, 0, matchTypes.Length);
            // Lookup the method ... matching argument types.
            MethodInfo method = type.GetMethod(methodName, BindingFlags.FlattenHierarchy | BindingFlags.Public | bindingFlags,
                null, matchTypes, null);
            // If no method found, throw an exception.
            if (method == null)
                throw new PrimitiveInvalidMemberException(String.Format(CodeGenerationErrors.MissingMethod, type.Name, methodName));
            List<Expression> args = this.GetArguments(argumentTypes);
            Expression instance = args[0];
            args.RemoveAt(0);
            return Expression.Call(instance, method, args);
        }

        /// <summary>
        /// Get the expression needed to perform a constructor call.
        /// </summary>
        /// <param name="type">Type that is expected to define the method we are looking for.</param>
        /// <param name="methodName">Name of the method to invoke.</param>
        /// <param name="argumentTypes">Types of arguments that we are to pass to the method.</param>
        /// <returns>Expression for the call</returns>
        private Expression GenerateConstructorCall(Type type, Type[] argumentTypes)
        {
            // Lookup the constructor ... matching argument types.
            ConstructorInfo ctor = type.GetConstructor(
                BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance,
                null, argumentTypes, null);
            // If no constructor found, throw an exception.
            if (ctor == null)
                throw new PrimitiveInvalidMemberException(String.Format(CodeGenerationErrors.MissingConstructor, type.Name));
            return Expression.New(ctor, this.GetArguments(argumentTypes));
        }

        /// <summary>
        /// Get the expression needed to perform a property get or set operation.
        /// </summary>
        /// <param name="type">Type that is expected to define the method we are looking for.</param>
        /// <param name="methodName">Name of the method to invoke.</param>
        /// <param name="argumentTypes">Types of arguments that we are to pass to the method.</param>
        /// <returns>Expression for the call</returns>
        private Expression GeneratePropertyCall(Type type, string propertyName, Type returnType, Type[] argumentTypes, BindingFlags bindingFlags)
        {
            // First, resolve the name of default properties
            if (propertyName == "this")
                propertyName = this.GetDefaultMemberName(type, typeof(PropertyInfo)) ?? propertyName;
            // Lookup the property ... matching argument types.
            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.FlattenHierarchy | BindingFlags.Public | bindingFlags,
                    null, returnType, argumentTypes, null);
            // If no property found, throw an exception.
            if (property == null)
                throw new PrimitiveInvalidMemberException(String.Format(CodeGenerationErrors.MissingProperty, type.Name, propertyName));

            bool isStatic = property.GetAccessors()[0].IsStatic;

            if ((bindingFlags & BindingFlags.GetProperty) == BindingFlags.GetProperty)
            {
                if ((argumentTypes == null) || (argumentTypes.Length == 0))
                {
                    if (isStatic)
                        return Expression.Property(null, property);
                    List<Expression> args = this.GetArguments(new Type[] { type });
                    return Expression.Property(args[0], property);
                } else {
                    List<Type> types = new List<Type>();
                    if (isStatic)
                        types.Add(typeof(object));
                    else
                        types.Add(type);
                    types.AddRange(argumentTypes);
                    List<Expression> args = this.GetArguments(types.ToArray());
                    Expression instance = args[0];
                    args.RemoveAt(0);
                    if (isStatic)
                        instance = null;
                    return Expression.Property(instance, property, args);
                }
            } else {
                if ((argumentTypes == null) || (argumentTypes.Length == 0))
                {
                    if (isStatic)
                    {
                        List<Expression> args = this.GetArguments(new Type[] { typeof(object), property.PropertyType });
                        return Expression.Assign(Expression.Property(null, property), args[1]);
                    }
                    else
                    {
                        List<Expression> args = this.GetArguments(new Type[] { type, property.PropertyType });
                        return Expression.Assign(Expression.Property(args[0], property), args[1]);
                    }
                } else {
                    List<Type> types = new List<Type>();
                    if (isStatic)
                        types.Add(typeof(object));
                    else
                        types.Add(type);
                    types.AddRange(argumentTypes);
                    types.Add(property.PropertyType);
                    List<Expression> args = this.GetArguments(types.ToArray());
                    Expression instance = args[0];
                    args.RemoveAt(0);
                    Expression value = args[args.Count - 1];
                    args.RemoveAt(args.Count - 1);
                    if (isStatic)
                        instance = null;
                    return Expression.Assign(Expression.Property(instance, property, args), value);
                }
            }
        }

        /// <summary>
        /// Get the expression needed to perform a field get or set operation.
        /// </summary>
        /// <param name="type">Type that is expected to define the method we are looking for.</param>
        /// <param name="methodName">Name of the method to invoke.</param>
        /// <param name="argumentTypes">Types of arguments that we are to pass to the method.</param>
        /// <returns>Expression for the call</returns>
        private Expression GenerateFieldCall(Type type, string fieldName, BindingFlags bindingFlags)
        {
            // Lookup the field ... matching argument types.
            FieldInfo field = type.GetField(fieldName, BindingFlags.FlattenHierarchy | BindingFlags.Public | bindingFlags);
            // If no field found, throw an exception.
            if (field == null)
                throw new PrimitiveInvalidMemberException(String.Format(CodeGenerationErrors.MissingField, type.Name, fieldName));

            if ((bindingFlags & BindingFlags.GetField) == BindingFlags.GetField)
            {
                if (field.IsStatic)
                    return Expression.Field(null, field);
                List<Expression> args = this.GetArguments(new Type[] { type });
                return Expression.Field(args[0], field);
            } else {
                if (field.IsStatic)
                {
                    List<Expression> args = this.GetArguments(new Type[] { typeof(object), field.FieldType });
                    return Expression.Assign(Expression.Field(null, field), args[1]);
                }
                else
                {
                    List<Expression> args = this.GetArguments(new Type[] { type, field.FieldType });
                    return Expression.Assign(Expression.Field(args[0], field), args[1]);
                }
            }
        }

        /// <summary>
        /// This converts the arguments that were passed in to expressions with the correct types.
        /// </summary>
        /// <param name="argumentTypes">Collection of types to convert to.</param>
        /// <returns>Collection of argument expressions that can be passed to the member call.</returns>
        private List<Expression> GetArguments(Type[] argumentTypes)
        {
            List<Expression> args = new List<Expression>(argumentTypes.Length);

            // There are two options:
            if (argumentTypes.Length == this.MethodVisitor.PassedArguments.Length)
            {
                // 1. Defined exactly the same number of arguments as there were passed to the method,
                //      then simply convert and map each argument passed to us to an argument that we are passing to the method.
                for (int i = 0; i < argumentTypes.Length; i++)
                    args.Add(this.Convert(this.MethodVisitor.PassedArguments[i], argumentTypes[i]));
                return args;
            }
            if (argumentTypes.Length == (this.MethodVisitor.PassedArguments.Length + 1))
            {
                // 2. Exactly one more argument was defined than passed to the method,
                //      implying that the first defined argument is mapped to the receiver (self),
                //      and the remaining arguments are mapped to the arguments passed to the method.
                args.Add(this.Convert(this.MethodVisitor.SelfArgument, argumentTypes[0]));

                for (int i = 1; i < argumentTypes.Length; i++)
                    args.Add(this.Convert(this.MethodVisitor.PassedArguments[i - 1], argumentTypes[i]));
                return args;
            }
            // Some mismatch :-/
            throw new PrimitiveSemanticException(CodeGenerationErrors.WrongNumberOfParameters);
        }

        private Expression Convert(DynamicMetaObject parameter, Type type)
        {
            if (type == null)
                return parameter.Expression;
            else if (type == typeof(object))
                return Expression.Convert(parameter.Expression, typeof(object));

            Type limitingType = (parameter.Value != null) ? parameter.Value.GetType() : parameter.LimitType;
            if ((limitingType != null) && (limitingType != typeof(object)))
            {
                // This is the tricky part .... 
                //
                // Example that will NOT WORK:
                //      Int16 i16 = 123;
                //      Object o16 = i16;
                //      Int32 i32 = (Int32) o16     // *** FAILS *** ... even if object currently has an Int16, it's an object and no cast to Int32!
                //  
                // Example that works:
                //      Int16 i16 = 123;
                //      Object o16 = i16;
                //      Int32 i32 = (Int32) ((Int16) o16)     // OK! First cast Object=>Int16 then Int16=>Int32

                // 1. Create an polymorphic inlined cache restrictions ... as long as the arguments are of the given type,
                //      we can "hardcode" the cast into the expression code (there is no way to do dynamic cast - cast is a static thing)
                BindingRestrictions restrictions = BindingRestrictions.GetTypeRestriction(parameter.Expression, limitingType);
                if (parameter.Restrictions != null)
                    restrictions = parameter.Restrictions.Merge(restrictions);
                if (this.RootVisitor.BindingRestrictions != null)
                    restrictions = this.RootVisitor.BindingRestrictions.Merge(restrictions);
                this.RootVisitor.BindingRestrictions = restrictions;
                if (limitingType == type)
                    // No need for double cast ... the argument is already in the given type.
                    return Expression.ConvertChecked(parameter.Expression, type);
                else
                    // First, cast to the arguments concrete type, then cast to the wanted type
                    return Expression.ConvertChecked(Expression.ConvertChecked(parameter.Expression, limitingType), type);
            }
            else
            {
                return Expression.ConvertChecked(parameter.Expression, type);
            }
        }

        private Expression Convert(Expression expression, Type type)
        {
            if (type == null)
                return expression;
            else if (type == typeof(object))
                return Expression.Convert(expression, typeof(object));
            else
                return Expression.ConvertChecked(expression, type);
        }

        private string GetDefaultMemberName(Type type, Type memberType)
        {
            foreach (var member in type.GetDefaultMembers())
            {
                if (memberType.IsAssignableFrom(member.GetType()))
                    return member.Name;
            }
            return null;
        }

        #endregion
    }
}
