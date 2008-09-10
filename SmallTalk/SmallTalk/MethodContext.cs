using System;
using System.Collections.Generic;

namespace SmallTalk
{
    /// <summary>
    /// Represents the execution of a CompiledMethod in response to a message.
    /// </summary>
    public class MethodContext : ICloneable
    {
        #region Variables

        private CompiledMethod _method;
        private int _instructionPointer;
        private SmallObjectAlpha _receiver;
        private List<SmallObjectAlpha> _arguments;
        private Stack<SmallObjectAlpha> _stack;
        private MethodContext _sender;

        #endregion

        #region Constructors

        public MethodContext(MethodContext sender, CompiledMethod method, int instructionPointer, SmallObjectAlpha receiver, List<SmallObjectAlpha> arguments, Stack<SmallObjectAlpha> stack)
        {
            _method = method;
            _instructionPointer = instructionPointer;
            _receiver = receiver;
            _arguments = arguments;
            _stack = stack;
        }

        public MethodContext(MethodContext clone)
            : this(clone.Sender, clone.Method, clone.InstructionPointer, clone.Receiver, clone.Arguments, clone.Stack)
        {
        }

        public MethodContext(MethodContext sender, CompiledMethod method, SmallObjectAlpha receiver, List<SmallObjectAlpha> arguments)
            : this(sender, method, 0, receiver, arguments, new Stack<SmallObjectAlpha>())
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The context that created this one.
        /// </summary>
        public MethodContext Sender
        {
            get
            {
                return _sender;
            }
            private set
            {
                _sender = value;
            }
        }

        /// <summary>
        /// The CompiledMethod whose bytecodes are being executed.
        /// </summary>
        public CompiledMethod Method
        {
            get
            {
                return _method;
            }
            private set
            {
                _method = value;
            }
        }

        /// <summary>
        /// The location of the next bytecode to be executed in the CompiledMethod.
        /// </summary>
        public int InstructionPointer
        {
            get
            {
                return _instructionPointer;
            }
            set
            {
                _instructionPointer = value;
            }
        }

        /// <summary>
        /// The receiver of the message that invoked the CompiledMethod.
        /// </summary>
        public SmallObjectAlpha Receiver
        {
            get
            {
                return _receiver;
            }
            private set
            {
                _receiver = value;
            }
        }

        /// <summary>
        /// The arguments of the message that invoked the CompiledMethod.
        /// This is also used to store temporary variables (the come at the end of the list.)
        /// </summary>
        public List<SmallObjectAlpha> Arguments
        {
            get
            {
                return _arguments;
            }
            private set
            {
                _arguments = value;
            }
        }

        /// <summary>
        /// The stack.
        /// </summary>
        public Stack<SmallObjectAlpha> Stack
        {
            get
            {
                return _stack;
            }
            set
            {
                _stack = value;
            }
        }

        #endregion

        #region Methods

        public object Clone()
        {
            return new MethodContext(this);
        }

        #endregion
    }
}