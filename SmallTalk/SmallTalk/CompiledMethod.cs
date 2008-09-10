using System.Collections.Generic;

namespace SmallTalk
{
    public class CompiledMethod
    {
        #region Variables

        private byte[] _code;
        private List<SmallObjectAlpha> _literalFrame;

        #endregion

        #region Constructors

        public CompiledMethod(byte[] code, List<SmallObjectAlpha> literalFrame)
        {
            _code = code;
            _literalFrame = literalFrame;
        }

        #endregion

        #region Properties

        public byte this[int index]
        {
            get
            {
                return _code[index];
            }
            set
            {
                _code[index] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get an object from the literal frame.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SmallObjectAlpha GetFrame(int index)
        {
            return _literalFrame[index];
        }

        #endregion
    }
}