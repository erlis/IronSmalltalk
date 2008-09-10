using System.Collections.Generic;

namespace SmallTalk
{
    /// <summary>
    /// Represents a block in a source method that is not part of an optimized control structure.
    /// </summary>
    public class BlockContext : MethodContext
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

        public BlockContext(MethodContext sender, CompiledMethod method, int instructionPointer, SmallObjectAlpha receiver, List<SmallObjectAlpha> arguments, Stack<SmallObjectAlpha> stack)
        {
            _method = method;
            _instructionPointer = instructionPointer;
            _receiver = receiver;
            _arguments = arguments;
            _stack = stack;
        }

        public BlockContext(MethodContext clone)
            : this(clone.Sender, clone.Method, clone.InstructionPointer, clone.Receiver, clone.Arguments, clone.Stack)
        {
        }

        public BlockContext(MethodContext sender, CompiledMethod method, SmallObjectAlpha receiver, List<SmallObjectAlpha> arguments)
            : this(method, 0, receiver, arguments, new Stack<SmallObjectAlpha>())
        {
        }

        public BlockContext(MethodContext context)
        {
            _method = context.Method;
            _instructionPointer = 0;
            _receiver = context.Receiver;
            _arguments = context.Arguments;
            _stack = new Stack<SmallObjectAlpha>();
            _sender = context.Sender;
            
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