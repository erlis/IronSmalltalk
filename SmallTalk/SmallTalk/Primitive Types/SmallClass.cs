using System;
using System.Collections.Generic;

namespace SmallTalk
{
    public class SmallClass : SmallObject
    {
        #region Variables

        private Dictionary<string, ICodeBlock> _selectors;

        #endregion

        #region Constructors

        public SmallClass(SmallClass baseClass)
            : base(baseClass)
        {
            _selectors = new Dictionary<string, ICodeBlock>();
        }

        public SmallClass()
            : base(null)
        {
        }

        #endregion

        #region Methods

        protected void AttachSelector(string selectorName, ICodeBlock codeBlock)
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

        protected void DetachSelector(string selectorName)
        {
            if (_selectors.ContainsKey(selectorName))
            {
                _selectors.Remove(selectorName);
            }
            else
            {
                throw new Exception(string.Format("Selector '{0}' is not defined.", selectorName));
            }
        }

        public override ICodeBlock FindSelector(string selectorName)
        {
            foreach (string name in _selectors.Keys)
            {
                if (selectorName == name)
                {
                    return _selectors[name];
                }
            }
            if ((Class == null) || (Class == this))
            {
                throw new Exception(string.Format("Selector '{0}' is not defined.", selectorName));
            }
            return Class.FindSelector(selectorName);
        }

        #endregion
    }
}