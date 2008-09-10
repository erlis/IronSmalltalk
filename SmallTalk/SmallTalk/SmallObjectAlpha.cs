using System.Collections.Generic;

namespace SmallTalk
{
    /// <summary>
    /// A SmallTalk object.
    /// </summary>
    public class SmallObjectAlpha
    {
        #region Variables

        private SmallClass _class;
        private List<SmallObjectAlpha> _instanceVariables;

        #endregion

        #region Constructors

        public SmallObjectAlpha()
        {
            _class = null;
            _instanceVariables = new List<SmallObjectAlpha>();
        }

        public SmallObjectAlpha(SmallClass cls, int numVars)
        {
            _class = cls;
            _instanceVariables = new List<SmallObjectAlpha>();
            for (int index = 0; index < numVars; index++)
            {
                _instanceVariables.Add(null);
            }
        }

        #endregion

        #region Properties

        public SmallClass Class
        {
            get
            {
                return _class;
            }
            private set
            {
                _class = value;
            }
        }

        public List<SmallObjectAlpha> InstanceVariables
        {
            get
            {
                return _instanceVariables;
            }
            private set
            {
                _instanceVariables = value;
            }
        }

        #endregion

        #region Methods

        /*
        public CompiledMethod FindMessage(SmallSymbol message)
        {
            return _class.FindSelector(message);
        }

        public CompiledMethod FindSuperMessage(SmallSymbol message)
        {
            return _class.SuperClass.FindSelector(message);
        }
        */

        #endregion
    }
}