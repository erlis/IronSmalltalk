using System.Collections.Generic;

namespace SmallTalk
{
    public class Interpreter
    {
        #region Variables

        /// <summary>
        /// Objects are referenced by number, and they can be both added and removed.
        /// Do not use a list, as if an object is removed from the middle of the list
        /// it would mess up the id values of the other objects.
        /// </summary>
        private Dictionary<int, SmallObjectAlpha> _objects;

        private Stack<MethodContext> _contextStack;

        /// <summary>
        /// The previous method context, which created the ActiveContext.
        /// </summary>
        private MethodContext _sender;

        #endregion

        #region Constructors

        public Interpreter()
        {
            _objects = new Dictionary<int, SmallObjectAlpha>();
            _contextStack = new Stack<MethodContext>();
            _sender = null;
        }

        #endregion

        #region Properties

        private MethodContext ActiveContext
        {
            get
            {
                return _contextStack.Peek();
            }
        }

        #endregion

        #region Methods

        public void Run()
        {
            while (true)
            {
                Cycle();
            }
        }

        private void Cycle()
        {
            // Step 1: Fetch the bytecode from the CompiledMethod indicated by the instruction pointer.
            byte code = ActiveContext.Method[ActiveContext.InstructionPointer];

            // Step 2: Increment the instruction pointer.
            ActiveContext.InstructionPointer++;

            // Step 3: Perform the function specified by the bytecode.
            Execute(code);
        }

        private void Execute(byte code)
        {
        }

        private void Suspend()
        {
            _contextStack.Push(new MethodContext(ActiveContext, null, null, null));
        }

        #endregion
    }
}