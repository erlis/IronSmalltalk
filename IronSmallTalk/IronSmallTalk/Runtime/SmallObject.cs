using System.Collections.Generic;

namespace IronSmalltalk.Runtime
{
    public class SmallObject
    {
        #region Variables

        private SmallObject _self;
        private SmallClass _class;
        private List<string> _instanceVariables;

        #endregion

        #region Constructors

        public SmallObject(SmallClass cls)
        {
            _self = this;
            _class = cls;
        }

        #endregion

        #region Properties

        public SmallObject Self
        {
            get
            {
                return _self;
            }
        }

        public SmallClass Class
        {
            get
            {
                return _class;
            }
        }

        public SmallClass Super
        {
            get
            {
                return _class.Class;
            }
        }

        #endregion
    }
}