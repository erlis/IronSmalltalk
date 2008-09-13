using System;
using System.Collections.Generic;

namespace IronSmalltalk
{
    public class SmallClass : SmallObject
    {
        #region Variables

        private Dictionary<SmallSymbol, ICodeBlock> _selectors;

        #endregion

        #region Constructors

        public SmallClass(SmallClass baseClass)
            : base(baseClass)
        {
            _selectors = new Dictionary<SmallSymbol, ICodeBlock>();
        }

        public SmallClass()
            : base(null)
        {
        }

        #endregion

        #region Methods

        protected void AttachSelector(SmallSymbol selectorName, ICodeBlock codeBlock)
        {
            if (_selectors.ContainsKey(selectorName))
            {
                _selectors[selectorName] = codeBlock;
            }
            else
            {
                _selectors.Add(selectorName, codeBlock);
            }
        }

        protected void DetachSelector(SmallSymbol selectorName)
        {
            if (_selectors.ContainsKey(selectorName))
            {
                _selectors.Remove(selectorName);
            }
            else
            {
                throw new MessageNotUnderstood(this.GetType(), selectorName);
            }
        }

        public override ICodeBlock FindSelector(SmallSymbol selectorName)
        {
            foreach (SmallSymbol name in _selectors.Keys)
            {
                if (selectorName.Equals(name))
                {
                    return _selectors[name];
                }
            }
            if ((Class == null) || (Class == this))
            {
                throw new MessageNotUnderstood(this.GetType(), selectorName);
            }
            return Class.FindSelector(selectorName);
        }

        #endregion
    }
}