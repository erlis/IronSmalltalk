using System.Collections.Generic;

namespace SmallTalk
{
    /// <summary>
    /// A class contains a number of messages.
    /// Each message is composed of a number of selectors and a code block.
    /// Each selector has a name and an argument.
    /// </summary>
    public class Message
    {
        #region Variables

        private List<Selector> _selectors;
        private ICodeBlock _codeBlock;

        #endregion

        #region Constructors

        public Message(string selectors, ICodeBlock codeBlock)
        {
            _selectors = ParseSelectors(selectors);
        }

        #endregion

        #region Properties

        public List<Selector> Selectors
        {
            get
            {
                return _selectors;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Selectors are of the format "name:parameter name:parameter etc." with optional space
        /// after the colon.
        /// </summary>
        /// <param name="selectors"></param>
        /// <returns></returns>
        private static List<Selector> ParseSelectors(string selectors)
        {
            return null;
        }

        #endregion
    }

    /// <summary>
    /// Each selector has a name and a parameter.
    /// </summary>
    public class Selector
    {
        #region Variables

        private string _name;
        private string _parameter;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new selector.
        /// </summary>
        /// <param name="name">The name of the selector.</param>
        /// <param name="parameter">The name of the selector's parameter.</param>
        public Selector(string name, string parameter)
        {
            _name = name;
            _parameter = parameter;
        }

        #endregion

        #region Properties

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Parameter
        {
            get
            {
                return _parameter;
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format("{0}: {1}", _name, _parameter);
        }

        #endregion
    }
}