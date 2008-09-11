using Microsoft.Scripting.Actions;
using Microsoft.Scripting.Generation;
using Microsoft.Scripting.Runtime;
using System;

namespace IronSmalltalk
{
    /// <summary>
    /// Binder for the IronSmalltalk language context.
    /// </summary>
    public class IronSmalltalkBinder : DefaultBinder
    {
        #region Constructors

        public IronSmalltalkBinder(ScriptDomainManager manager)
            : base(manager)
        {
        }

        #endregion

        #region Methods

        public override bool CanConvertFrom(Type fromType, Type toType, NarrowingLevel level)
        {
            return toType.IsAssignableFrom(fromType);
        }

        public override bool PreferConvert(Type t1, Type t2)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}