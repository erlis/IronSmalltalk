using System;
using System.Collections.Generic;

namespace IronSmalltalk
{
    public class SmallObject : ISmallObject
    {
        #region Variables

        private SmallClass _class;
        private Dictionary<SmallSymbol, SmallObject> _symbolTable;
        private ICodeContext _codeContext;

        #endregion

        #region Constructors

        private SmallObject()
        {
            _class = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="cls">The class for which this object is an instance.</param>
        public SmallObject(SmallClass cls)
            : this()
        {
            _class = cls;
            _symbolTable = new Dictionary<SmallSymbol, SmallObject>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The class for which this object is an instance.
        /// </summary>
        public SmallClass Class
        {
            get
            {
                return _class;
            }
            protected set
            {
                _class = value;
            }
        }

        /// <summary>
        /// Instance variables.
        /// </summary>
        public Dictionary<SmallSymbol, SmallObject> SymbolTable
        {
            get
            {
                return _symbolTable;
            }
        }

        public ICodeContext ParentContext
        {
            get
            {
                return _codeContext;
            }
            set
            {
                _codeContext = value;
            }
        }

        #endregion

        #region Methods

        public virtual ICodeBlock FindSelector(string selectorName)
        {
            return _class.FindSelector(selectorName);
        }

        public ICodeBlock FindSuperSelector(string selectorName)
        {
            if (_class.Class == null)
            {
                throw new Exception(string.Format("Class '{0}' does not inherit from anything.", _class));
            }
            return _class.Class.FindSelector(selectorName);
        }

        /// <summary>
        /// Only sends unary messages right now.
        /// </summary>
        /// <param name="message"></param>
        public SmallObject SendMessage(string message)
        {
            return FindSelector(message).Execute(this);
        }

        #endregion
    }
}