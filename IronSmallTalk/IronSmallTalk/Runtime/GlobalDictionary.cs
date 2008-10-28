using Microsoft.Scripting;
using Microsoft.Scripting.Ast;
using System;
using System.Collections.Generic;

namespace IronSmalltalk
{
    public class GlobalDictionary : Dictionary<string, SmallObject>
    {
        #region Variables

        private static GlobalDictionary _instance;

        #endregion

        #region Constructors

        static GlobalDictionary()
        {
            _instance = new GlobalDictionary();
        }

        private GlobalDictionary()
            : base()
        {
        }

        #endregion

        #region Methods

        public static bool Contains(string name)
        {
            return _instance.ContainsKey(name);
        }

        public static new SmallObject Add(string name, SmallObject value)
        {
            if (_instance.ContainsKey(name))
            {
                Set(name, value);
            }
            else
            {
                (_instance as Dictionary<string, SmallObject>).Add(name, value);
            }

            return value;
        }

        public static SmallObject Set(string name, SmallObject value)
        {
            if (!Contains(name))
            {
                throw new Exception(string.Format("'{0}' has not been defined.", name));
            }
            _instance[name] = value;

            return value;
        }

        public static SmallObject Get(string name)
        {
            if (Contains(name))
            {
                return _instance[name];
            }
            return null;
        }

        public static new void Remove(string name)
        {
            if (!Contains(name))
            {
                throw new Exception(string.Format("'{0}' has not been defined.", name));
            }
            {
                (_instance as Dictionary<string, SmallObject>).Remove(name);
            }
        }

        #endregion
    }
}